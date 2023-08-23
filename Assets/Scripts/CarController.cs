using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private float steeringAngle;

    public float maxSteeringAngle = 30;
    public float motorForce = 250;

    public PlayerInputActions playerInputActions;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider bottomLeftWheelCollider;
    public WheelCollider bottomRightWheelCollider;

    public GameObject frontLeftWheel;
    public GameObject frontRightWheel;
    public GameObject bottomLeftWheel;
    public GameObject bottomRightWheel;


    private void Awake()
    {
        playerInputActions = new();
        playerInputActions.Movements.Enable();

    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        UpdateWheel();
    }

    public void Accelerate()
    {
        bottomLeftWheelCollider.motorTorque = motorForce;
        bottomRightWheelCollider.motorTorque = motorForce;
    }

    public void UpdateWheel()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheel.transform);
        UpdateWheelPose(frontRightWheelCollider, frontRightWheel.transform);
        UpdateWheelPose(bottomLeftWheelCollider, bottomLeftWheel.transform);
        UpdateWheelPose(bottomRightWheelCollider, bottomRightWheel.transform);
    }

    public void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        _collider.GetWorldPose(out Vector3 _pos, out Quaternion _quat);

        _transform.SetPositionAndRotation(_pos, _quat);
    }

    public void Steer()
    {
        InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);

        float _input = playerInputActions.Movements.LeftRight.ReadValue<float>();
        steeringAngle += _input;
        steeringAngle = Mathf.Clamp(steeringAngle, -30f, 30f);
        Debug.Log(steeringAngle);

        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }

}
