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

    public MeshRenderer frontLeftWheel;
    public MeshRenderer frontRightWheel;
    public MeshRenderer bottomLeftWheel;
    public MeshRenderer bottomRightWheel;

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
        Steer();
        Accelerate();
        UpdateWheel();

        Debug.Log(speed.ToString("#.##") + "  " +
            steeringAngle.ToString("#.##") + "  " +
            playerInputActions.Movements.Accl.ReadValue<float>() + "  " +
            playerInputActions.Movements.LeftRight.ReadValue<float>());
    }
    public void UpdateWheel()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheel);
        UpdateWheelPose(frontRightWheelCollider, frontRightWheel);
        UpdateWheelPose(bottomLeftWheelCollider, bottomLeftWheel);
        UpdateWheelPose(bottomRightWheelCollider, bottomRightWheel);
    }

    public void UpdateWheelPose(WheelCollider _collider, MeshRenderer _mesh)
    {
        _collider.GetWorldPose(out Vector3 _pos, out Quaternion _quat);

        _mesh.transform.SetPositionAndRotation(_pos, _quat);
    }

    public void Accelerate()
    {
        float _input = playerInputActions.Movements.Accl.ReadValue<float>();

        bottomLeftWheelCollider.motorTorque = motorPower * _input;
        bottomRightWheelCollider.motorTorque = motorPower * _input;
    }


    public void Steer()
    {
        //InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);

        float _input = playerInputActions.Movements.LeftRight.ReadValue<float>();

        steeringAngle = _input * steeringCurve.Evaluate(speed);
        steeringAngle = Mathf.Clamp(steeringAngle, -36f, 36f);
        Debug.Log(steeringAngle);

        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }

}
