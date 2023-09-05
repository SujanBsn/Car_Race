using UnityEngine;
public class BrokenGlass : MonoBehaviour
{
    public Material[] glassMaterials; // An array of materials to use for different levels of damage.
    public float damageThreshold; // Adjust this value to set the damage threshold.

    private float damageTaken;
    private Renderer glassRenderer;
    private int currentMaterialIndex = 0;

    private void Start()
    {
        glassRenderer = gameObject.GetComponent<Renderer>();

        if (glassRenderer == null)
        {
            Debug.LogError("BrokenGlass script requires a Renderer component.");
        }
        else
        {
            glassRenderer.material = glassMaterials[currentMaterialIndex];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float collisionIntensity = collision.relativeVelocity.magnitude;
        damageTaken += collisionIntensity;
        ApplyDamage(damageTaken);
    }

    public void ApplyDamage(float _damageAmount)
    {
        int newMaterialIndex = Mathf.FloorToInt((_damageAmount / damageThreshold) * (glassMaterials.Length - 1));
        newMaterialIndex = Mathf.Clamp(newMaterialIndex, 0, glassMaterials.Length - 1);
        
        if (newMaterialIndex != currentMaterialIndex)
        {
            glassRenderer.material = glassMaterials[newMaterialIndex];
            currentMaterialIndex = newMaterialIndex;
        }
    }
}
