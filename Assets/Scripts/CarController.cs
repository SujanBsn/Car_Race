using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{

    public float maxSteeringAngle = 36;
    public float motorPower = 300;

    public AnimationCurve steeringCurve;
    public PlayerInputActions playerInputActions;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider bottomLeftWheelCollider;
    public WheelCollider bottomRightWheelCollider;

    public GameObject frontLeftWheel;
    public GameObject frontRightWheel;
    public GameObject bottomLeftWheel;
    public GameObject bottomRightWheel;

    private float steeringAngle;
    private float speed;

    private void Awake()
    {
        playerInputActions = new();
        playerInputActions.Movements.Enable();
    }

    private void FixedUpdate()
    {

        speed = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;

        //Steer();
        TiltSteer();
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

    public void Accelerate()
    {
        float _input = 1f;//playerInputActions.Movements.Accl.ReadValue<float>();

        bottomLeftWheelCollider.motorTorque = motorPower * _input;
        bottomRightWheelCollider.motorTorque = motorPower * _input;
    }


    public void Steer()
    {
        float _input = playerInputActions.Movements.K_LeftRight.ReadValue<float>();

        steeringAngle = _input * steeringCurve.Evaluate(speed);
        steeringAngle = Mathf.Clamp(steeringAngle, -steeringCurve.Evaluate(speed), steeringCurve.Evaluate(speed));

        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }

    public void TiltSteer()
    {
        InputSystem.EnableDevice(Accelerometer.current);
        float _sensitivity = 0.5f;

        float _input = playerInputActions.Movements.S_LeftRight.ReadValue<float>();
        _input *= steeringCurve.Evaluate(speed) * _sensitivity;

        steeringAngle = _input;
        steeringAngle = Mathf.Clamp(steeringAngle, -steeringCurve.Evaluate(speed), steeringCurve.Evaluate(speed));
        
        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
        Debug.Log(steeringAngle.ToString("#.#"));
    }
}
