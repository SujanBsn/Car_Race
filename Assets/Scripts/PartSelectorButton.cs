using System.Linq;
using UnityEngine;

public class PartSelectorButton : MonoBehaviour
{
    // References to selected part and car customization information.
    public SelectedPart selectedPart; // Reference to the selected part.
    public SelectedCar selectedCar; // Reference to the selected car.

    public Material material; // The material associated with this button.

    private void Update()
    {
        GetMaterial();
    }

    // Update the material based on the selected car's parts.
    void GetMaterial()
    {
        bool found = false;

        // Loop through the part names in the selected car's parts.
        for (int i = 0; i < selectedCar.carParts.partName.Count(); i++)
        {
            // Check if the part name matches the name of this button.
            if (selectedCar.carParts.partName[i] == gameObject.name)
            {
                material = selectedCar.carParts.partMaterial[i]; // Get the associated material.
                found = true;
            }
        }

        // If no matching part is found, hide the button.
        if (!found)
            gameObject.SetActive(false);
    }

    // Set the selected part to the material associated with this button.
    public void Setpart()
    {
        Debug.Log(material); // Log the selected material for debugging.
        selectedPart.SelectedMaterial = material; // Set the selected material.
    }
}
