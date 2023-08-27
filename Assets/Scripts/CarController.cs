using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GearState
{
    Neutral,
    Running,
    CheckingChange,
    Changing
}


public class CarController : MonoBehaviour
{

    public float maxSteeringAngle;
    public float motorPower;
    public float brakePower;
    public float idleRPM;
    public float redlineRPM;
    public float differentialRatio;
    public float increaseGearRPM;
    public float decreaseGearRPM;

    public float[] gearRatio;
    public float[] speedPerGear;

    public AnimationCurve steeringCurve;
    public AnimationCurve hpCurve;
    public PlayerInputActions playerInputActions;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI rpmText;
    public TextMeshProUGUI gearText;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider bottomLeftWheelCollider;
    public WheelCollider bottomRightWheelCollider;

    public GameObject frontLeftWheel;
    public GameObject frontRightWheel;
    public GameObject bottomLeftWheel;
    public GameObject bottomRightWheel;
    
    private Rigidbody playerRb;

    private float acclInput = 0f;
    private float steerInput = 0f;
    private float brakeInput = 0f;

    private float steeringAngle = 0f;
    private float currentRPM = 0f;
    private float currentTorque = 0f;
    private float wheelRPM = 0f;
    private float speed = 0f;

    private GearState gearState = GearState.Running;

    private int currentGear;

    private void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerInputActions = new();
        playerInputActions.Movements.Enable();
    }

    private void FixedUpdate()
    {
        speed = playerRb.velocity.magnitude * 3.6f;
        speedText.text = speed.ToString("#");
        rpmText.text = currentRPM.ToString("#");
        gearText.text = (currentGear + 1).ToString();

        CheckInput();
        Steer();

        if (SystemInfo.supportsAccelerometer)
        {
            TiltSteer();
        }
        Brake();
        Accelerate();
        UpdateWheel();
    }
    public void UpdateWheel()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheel);
        UpdateWheelPose(frontRightWheelCollider, frontRightWheel);
        UpdateWheelPose(bottomLeftWheelCollider, bottomLeftWheel);
        UpdateWheelPose(bottomRightWheelCollider, bottomRightWheel);
    }

    public void UpdateWheelPose(WheelCollider _collider, GameObject _obj)
    {
        _collider.GetWorldPose(out Vector3 _pos, out Quaternion _quat);

        _obj.transform.SetPositionAndRotation(_pos, _quat);
    }

    public void CheckInput()
    {
        float _movingDirection = 0f;
        acclInput = playerInputActions.Movements.Accl.ReadValue<float>();
        steerInput = playerInputActions.Movements.LeftRight.ReadValue<float>();

        _movingDirection = Vector3.Dot(transform.forward, playerRb.velocity);
        if (_movingDirection < -0.5f && acclInput > 0)
            brakeInput = Mathf.Abs(acclInput);
        else if (_movingDirection > 0.5f && acclInput < 0)
            brakeInput = Mathf.Abs(acclInput);
        else
            brakeInput = 0;
    }

    public void Brake()
    {
        frontLeftWheelCollider.brakeTorque = brakePower * brakeInput * .7f;
        frontRightWheelCollider.brakeTorque = brakePower * brakeInput * .7f;

        bottomLeftWheelCollider.brakeTorque = brakePower * brakeInput * .3f;
        bottomRightWheelCollider.brakeTorque = brakePower * brakeInput * .3f;
    }
    public void Accelerate()
    {
        currentTorque = CalculateTorque();
        bottomLeftWheelCollider.motorTorque = currentTorque * acclInput;
        bottomRightWheelCollider.motorTorque = currentTorque * acclInput;
    }
    public float CalculateTorque()
    {
        float _torque = 0f; 
        if (gearState == GearState.Running)
        {
            if (currentRPM > increaseGearRPM && speed >= speedPerGear[currentGear])
                StartCoroutine(ChangeGear(1));
            else if (currentGear < decreaseGearRPM && speed <= speedPerGear[currentGear])
                StartCoroutine(ChangeGear(-1));
        } 

        wheelRPM = Mathf.Abs(bottomLeftWheelCollider.rpm + bottomRightWheelCollider.rpm) *
            .5f * gearRatio[currentGear] * differentialRatio;

        currentRPM = Mathf.Lerp(currentRPM, Mathf.Max(idleRPM - 100, wheelRPM), Time.deltaTime * 3f);
        _torque = hpCurve.Evaluate(currentRPM / redlineRPM) * motorPower / currentRPM *
            gearRatio[currentGear] * differentialRatio * 5252f;
        return _torque;
    }

    public void Steer()
    {
        steeringAngle = steerInput * steeringCurve.Evaluate(speed);

        if(speed>=35)
            steeringAngle += Vector3.SignedAngle(transform.forward, playerRb.velocity + transform.forward, Vector3.up);

        steeringAngle = Mathf.Clamp(steeringAngle, -steeringCurve.Evaluate(speed), steeringCurve.Evaluate(speed));

        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }

    public void TiltSteer()
    {
        InputSystem.EnableDevice(Accelerometer.current);
        float _sensitivity = 0.5f;

        steerInput *= steeringCurve.Evaluate(speed) * _sensitivity;
        steeringAngle = steerInput;

        if (speed >= 35)
            steeringAngle += Vector3.SignedAngle(transform.forward, playerRb.velocity + transform.forward, Vector3.up);

        steeringAngle = Mathf.Clamp(steeringAngle, -steeringCurve.Evaluate(speed), steeringCurve.Evaluate(speed));
        
        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }

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
                if (currentRPM > decreaseGearRPM|| currentGear <= 0)
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
}
