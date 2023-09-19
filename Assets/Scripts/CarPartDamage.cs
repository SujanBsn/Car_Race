using UnityEngine;

public class CarPartDamage : MonoBehaviour
{
    public float damageThreshold; // The damage threshold above which the part will become damaged.
    public float speedLimit; // The speed limit at which the part will detach when broken.
    private new HingeJoint hingeJoint; // The hinge joint component attached to this GameObject.
    private JointLimits initialLimits; // The initial joint limits of the hinge joint.
    private Rigidbody partRb; // The Rigidbody component attached to this GameObject.

    private float accumulatedDamage; // Accumulated damage from collisions.
    private bool isFallen = false; // Flag to track if the part has fallen off.
    private bool isBroken = false; // Flag to track if the part is broken.
    private bool hasHingeJoint = false; // Flag to check if the GameObject has a hinge joint.

    private void Start()
    {
        partRb = GetComponent<Rigidbody>(); // Get the Rigidbody component of this GameObject.
        if (TryGetComponent<HingeJoint>(out hingeJoint)) // Check if there is a HingeJoint component.
            hasHingeJoint = true; // Set the flag to true if a hinge joint is present.

        if (hasHingeJoint)
        {
            initialLimits = hingeJoint.limits; // Store the initial joint limits.
            DisableHingeJoint(); // Disable the hinge joint initially.
        }
    }

    private void FixedUpdate()
    {
        // Check if the part is broken, has a hinge joint, and is moving at a high speed.
        if (hasHingeJoint && isBroken && gameObject.GetComponent<Rigidbody>().velocity.magnitude >= speedLimit)
        {
            isFallen = true; // Set the fallen flag to true.
            hingeJoint.breakForce = 0f; // Disable the hinge joint's break force.
            hingeJoint.breakTorque = 0f; // Disable the hinge joint's break torque.
            partRb.drag = .5f; // Apply drag to the Rigidbody.
            partRb.angularDrag = .5f; // Apply angular drag to the Rigidbody.

            partRb.useGravity = true; // Enable gravity for the part.
            Destroy(gameObject, 3.0f); // Destroy the GameObject after 3 seconds.
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float collisionIntensity = collision.relativeVelocity.magnitude; // Calculate collision intensity.
        accumulatedDamage += collisionIntensity; // Accumulate collision damage.
        ApplyDamage(accumulatedDamage); // Apply the accumulated damage.
    }

    public void ApplyDamage(float _damageAmount)
    {
        // Check if the damage exceeds the damage threshold, the part hasn't fallen, and there's a hinge joint.
        if (_damageAmount >= damageThreshold && !isFallen && hasHingeJoint)
        {
            EnableHingeJoint(_damageAmount); // Enable the hinge joint with additional damage.
        }
    }

    private void EnableHingeJoint(float _damageAmount)
    {
        isBroken = true; // Set the broken flag to true.
        hingeJoint.limits = initialLimits; // Restore the initial hinge joint limits.

        // Check if the damage amount exceeds a threshold to fully break the part.
        if (_damageAmount >= damageThreshold * 1.8)
        {
            hingeJoint.breakForce = 0f; // Disable the hinge joint's break force.
            hingeJoint.breakTorque = 0f; // Disable the hinge joint's break torque.
            partRb.drag = .5f; // Apply drag to the Rigidbody.

            partRb.useGravity = true; // Enable gravity for the part.
            Destroy(gameObject, 3.0f); // Destroy the GameObject after 3 seconds.
        }
    }

    private void DisableHingeJoint()
    {
        hingeJoint.limits = new JointLimits { min = 0f, max = 0f }; // Disable the hinge joint limits.
    }
}
