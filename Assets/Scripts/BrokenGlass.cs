using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    public Material[] glassMaterials; // An array of materials to use for different levels of damage.
    public float damageThreshold; // Adjust this value to set the damage threshold.

    private float damageTaken;
    private Renderer glassRenderer; // Reference to the Renderer component.
    private int currentMaterialIndex = 0; // Index of the current material.

    private void Start()
    {
        // Try to get the Renderer component attached to this object.
        gameObject.TryGetComponent<Renderer>(out glassRenderer);

        if (glassRenderer == null)
        {
            // Display an error message if the Renderer component is not found.
            Debug.LogError("BrokenGlass script requires a Renderer component.");
        }
        else
        {
            // Set the initial material to the first material in the array.
            glassRenderer.material = glassMaterials[currentMaterialIndex];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Calculate the collision intensity based on relative velocity.
        float collisionIntensity = collision.relativeVelocity.magnitude;
        damageTaken += collisionIntensity;

        // Apply damage based on the collision intensity.
        ApplyDamage(damageTaken);
    }

    public void ApplyDamage(float _damageAmount)
    {
        // Check if damage taken exceeds a certain threshold, and destroy the object if necessary.
        if (damageTaken >= damageThreshold * 2)
            Destroy(gameObject);

        // Calculate the index of the new material based on the damage amount and threshold.
        int newMaterialIndex = Mathf.FloorToInt((_damageAmount / damageThreshold) * (glassMaterials.Length - 1));

        // Ensure the newMaterialIndex stays within valid bounds.
        newMaterialIndex = Mathf.Clamp(newMaterialIndex, 0, glassMaterials.Length - 1);

        // Update the material if it's different from the current material.
        if (newMaterialIndex != currentMaterialIndex)
        {
            glassRenderer.material = glassMaterials[newMaterialIndex];
            currentMaterialIndex = newMaterialIndex;
        }
    }
}
