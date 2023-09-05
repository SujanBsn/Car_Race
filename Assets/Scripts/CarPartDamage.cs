using UnityEngine;

public class CarPartDamage : MonoBehaviour
{
    public float damageThreshold; // Adjust this value to set the damage threshold.
    public float speedLimit;
    private new HingeJoint hingeJoint;
    private JointLimits initialLimits;
    private Rigidbody partRb;

    private float accumulatedDamage;
    private bool isFallen = false;
    private bool isBroken = false;

    private void Start()
    {
        partRb = GetComponent<Rigidbody>();
        hingeJoint = GetComponent<HingeJoint>();
        initialLimits = hingeJoint.limits;
        DisableHingeJoint();
    }

    private void FixedUpdate()
    {
        if (isBroken && gameObject.GetComponent<Rigidbody>().velocity.magnitude >= speedLimit)
        {
            isFallen = true;
            hingeJoint.breakForce = 0f;
            hingeJoint.breakTorque = 0f;
            partRb.drag = .5f;
            partRb.angularDrag = .5f;

            partRb.useGravity = true;
            Destroy(gameObject, 3.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float collisionIntensity = collision.relativeVelocity.magnitude;
        accumulatedDamage += collisionIntensity;
        ApplyDamage(accumulatedDamage);
    }

    public void ApplyDamage(float _damageAmount)
    {
        if (_damageAmount >= damageThreshold && !isFallen)
        {
            EnableHingeJoint(_damageAmount);
        }
    }

    private void EnableHingeJoint(float _damageAmount)
    {
        isBroken = true;
        hingeJoint.limits = initialLimits;

        if (_damageAmount >= damageThreshold * 1.8)
        {
            hingeJoint.breakForce = 0f;
            hingeJoint.breakTorque = 0f;
            partRb.drag = .5f;

            partRb.useGravity = true;
            Destroy(gameObject, 3.0f);
        }
    }

    private void DisableHingeJoint()
    {
        hingeJoint.limits = new JointLimits { min = 0f, max = 0f }; // Adjust the angles as needed.
    }
}
