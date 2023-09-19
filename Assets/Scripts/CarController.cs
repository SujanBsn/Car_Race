using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//Enum for different gear states
public enum GearState
{
    Neutral,
    Running,
    CheckingChange,
    Changing
}

public class CarController : MonoBehaviour
{
    // Public variables for tweaking car behavior in the Inspector.
    public float maxSteeringAngle; // Maximum steering angle of the car.
    public float movingDirection; // Direction of car movement.
    public float motorPower; // Motor power of the car.
    public float brakePower; // Brake power of the car.
    public float speed; // Current speed of the car.
    public float idleRPM; // Idle RPM (Revolutions Per Minute) of the car's engine.
    public float redlineRPM; // Redline RPM of the car's engine.
    public float differentialRatio; // Differential ratio of the car.
    public float increaseGearRPM; // RPM at which to shift up a gear.
    public float decreaseGearRPM; // RPM at which to shift down a gear.
    public float frontBrakeStrength; // Strength of front brakes.
    public float rearBrakeStrength; // Strength of rear brakes.

    // Arrays defining gear ratios and speeds per gear.
    public float[] gearRatio;
    public float[] speedPerGear;

    // Input actions for player control.
    public PlayerInputActions playerInputActions;

    // Animation curves for steering and engine performance.
    public AnimationCurve steeringCurve;
    public AnimationCurve hpCurve;

    // UI text elements to display speed, RPM, and current gear.
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI rpmText;
    public TextMeshProUGUI gearText;

    // Wheel colliders for the car's wheels.
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider bottomLeftWheelCollider;
    public WheelCollider bottomRightWheelCollider;

    // GameObjects representing the car's wheels and brake lights.
    public GameObject frontLeftWheel;
    public GameObject frontRightWheel;
    public GameObject bottomLeftWheel;
    public GameObject bottomRightWheel;
    public GameObject brakeLight_L;
    public GameObject brakeLight_R;

    // Reference to the car's rigidbody.
    private Rigidbody playerRb;

    // Input variables for acceleration, steering, and braking.
    private float acclInput;
    private float steerInput;
    private float brakeInput;

    // Variables for steering angle, current RPM, current torque, and wheel RPM.
    private float steeringAngle;
    private float currentRPM;
    private float currentTorque;
    private float wheelRPM;

    // Current gear state and gear index.
    private GearState gearState = GearState.Running;
    private int currentGear;

    private void Start()
    {
        // Get the car's rigidbody component.
        playerRb = gameObject.GetComponent<Rigidbody>();

        // Initialize the player input actions.
        playerInputActions = new PlayerInputActions();
        playerInputActions.Movements.Enable();
    }

    private void FixedUpdate()
    {
        // Calculate the current speed of the car.
        speed = playerRb.velocity.magnitude * 3.6f; // Conversion from m/s to km/h.

        // Update the UI text elements with speed, RPM, and current gear.
        speedText.text = speed.ToString("#");
        rpmText.text = currentRPM.ToString("#");
        gearText.text = (currentGear + 1).ToString(); // Adding 1 to display gear number.

        // Check player input and control the car.
        CheckInput();
        if (SystemInfo.supportsAccelerometer)
            TiltSteer(); // Tilt-based steering for mobile devices.
        Steer(); // Apply steering.
        Brake(); // Apply braking.
        Accelerate(); // Apply acceleration.
        UpdateWheel(); // Update wheel positions and rotation.
    }

    // Update the visual representation of the wheels based on wheel colliders.
    public void UpdateWheel()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheel);
        UpdateWheelPose(frontRightWheelCollider, frontRightWheel);
        UpdateWheelPose(bottomLeftWheelCollider, bottomLeftWheel);
        UpdateWheelPose(bottomRightWheelCollider, bottomRightWheel);
    }

    // Update the position and rotation of a wheel GameObject based on a wheel collider.
    public void UpdateWheelPose(WheelCollider _collider, GameObject _obj)
    {
        _collider.GetWorldPose(out Vector3 _pos, out Quaternion _quat);
        _obj.transform.SetPositionAndRotation(_pos, _quat);
    }

    // Check player input for acceleration, steering, and braking.
    public void CheckInput()
    {
        acclInput = playerInputActions.Movements.Accl.ReadValue<float>();
        steerInput = playerInputActions.Movements.LeftRight.ReadValue<float>();
        movingDirection = Vector3.Dot(transform.forward, playerRb.velocity);

        // Determine if the player is applying brakes based on movement direction.
        if (movingDirection < -0.5f && acclInput > 0)
            brakeInput = Mathf.Abs(acclInput);
        else if (movingDirection > 0.5f && acclInput < 0)
            brakeInput = Mathf.Abs(acclInput);
        else
            brakeInput = 0;

        // Activate brake lights when braking.
        if (acclInput < 0)
        {
            brakeLight_L.SetActive(true);
            brakeLight_R.SetActive(true);
        }
        else
        {
            brakeLight_L.SetActive(false);
            brakeLight_R.SetActive(false);
        }
    }

    // Apply braking to the car's wheel colliders.
    public void Brake()
    {
        frontLeftWheelCollider.brakeTorque = brakePower * brakeInput * frontBrakeStrength;
        frontRightWheelCollider.brakeTorque = brakePower * brakeInput * frontBrakeStrength;

        bottomLeftWheelCollider.brakeTorque = brakePower * brakeInput * rearBrakeStrength;
        bottomRightWheelCollider.brakeTorque = brakePower * brakeInput * rearBrakeStrength;
    }

    // Calculate the torque applied to the car's wheels based on RPM and throttle input.
    public float CalculateTorque()
    {
        if (gearState == GearState.Running)
        {
            // Check if it's time to shift gears.
            if (currentRPM > increaseGearRPM && speed >= speedPerGear[currentGear])
                StartCoroutine(ChangeGear(1));
            else if (currentGear < decreaseGearRPM && speed <= speedPerGear[currentGear])
                StartCoroutine(ChangeGear(-1));
        }

        // Calculate wheel RPM based on current gear and differential ratio.
        wheelRPM = Mathf.Abs(bottomLeftWheelCollider.rpm + bottomRightWheelCollider.rpm) *
            0.5f * gearRatio[currentGear] * differentialRatio;

        // Smoothly interpolate current RPM towards a target RPM.
        currentRPM = Mathf.Lerp(currentRPM, Mathf.Max(idleRPM - 100, wheelRPM), Time.deltaTime * 3f);

        // Calculate torque using an RPM curve, motor power, and gear ratios.
        float _torque = hpCurve.Evaluate(currentRPM / redlineRPM) * motorPower / currentRPM *
            gearRatio[currentGear] * differentialRatio * 5252f;

        return _torque;
    }

    // Apply acceleration to the car's wheel colliders.
    public void Accelerate()
    {
        currentTorque = CalculateTorque();
        bottomLeftWheelCollider.motorTorque = currentTorque * acclInput;
        bottomRightWheelCollider.motorTorque = currentTorque * acclInput;
    }

    // Apply steering input to the car's front wheels.
    public void Steer()
    {
        // Smoothly interpolate steering angle based on speed and input.
        steeringAngle = Mathf.LerpAngle(steeringAngle,
            steerInput * steeringCurve.Evaluate(speed), Time.deltaTime * 3f);
        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }

    // Tilt-based steering for mobile devices using accelerometer input.
    public void TiltSteer()
    {
        InputSystem.EnableDevice(Accelerometer.current);
        steeringAngle = Mathf.LerpAngle(steeringAngle,
            steerInput * steeringCurve.Evaluate(speed), 2f);
        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }

    // Coroutine to handle gear changes with delays.
    IEnumerator ChangeGear(int _gearChange)
    {
        gearState = GearState.CheckingChange;
        if (currentGear + _gearChange >= 0 && currentGear <= 6)
        {
            if (_gearChange > 0)
            {
                yield return new WaitForSeconds(1.0f);
                if (currentRPM < increaseGearRPM || currentGear >= gearRatio.Length - 1)
                {
                    gearState = GearState.Running;
                    yield break;
                }
            }
            if (_gearChange < 0)
            {
                yield return new WaitForSeconds(0.1f);
                if (currentRPM > decreaseGearRPM || currentGear <= 0)
                {
                    gearState = GearState.Running;
                    yield break;
                }

            }
            gearState = GearState.Changing;
            yield return new WaitForSeconds(0.1f);
            currentGear += _gearChange;
        }
        gearState = GearState.Running;
    }

    // Calculate a speed ratio for the car's speedometer display.
    public float GetSpeedRatio()
    {
        var _gas = Mathf.Clamp(Mathf.Abs(acclInput), 0.75f, 1f) * Mathf.Sign(acclInput);
        return speed * _gas / 160; // Adjusted for display scaling.
    }

    // Restart the game by reloading the main scene.
    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }
}
