using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AddStickers : MonoBehaviour
{
    // The texture to be used as a sticker.
    public Texture stickerTexture;

    // References to selected car, selected part, and part ID.
    public SelectedCar selectedCar; // Reference to the selected car.
    public SelectedPart selectedPart; // Reference to the selected part.
    public int partId; // The part ID associated with this sticker.

    private void Update()
    {
        CheckForNullSticker();
    }

    // Select the part for sticker customization.
    public void SelectpartForSticker()
    {
        selectedPart.partIdForSticker = partId;
    }

    // Add the selected sticker to the part.
    public void AddSticker()
    {
        Material _stickerMat = selectedCar.carParts.partSticker[selectedPart.partIdForSticker];

        // Enable the "Base_Map" keyword and set the sticker texture.
        _stickerMat.EnableKeyword("Base_Map");
        _stickerMat.SetTexture("Base_Map", stickerTexture);
    }

    // Clear the sticker from the part.
    public void ClearSticker()
    {
        Material _stickerMat = selectedCar.carParts.partSticker[selectedPart.partIdForSticker];

        // Enable the "Base_Map" keyword and set the sticker texture to null.
        _stickerMat.EnableKeyword("Base_Map");
        _stickerMat.SetTexture("Base_Map", null);
    }

    // Check for null stickers and handle visibility.
    public void CheckForNullSticker()
    {
        if (gameObject.CompareTag("stickerPart"))
        {
            bool found = false;

            // Loop through part names in the selected car's parts.
            for (int i = 0; i < selectedCar.carParts.partName.Count(); i++)
            {
                if (selectedCar.carParts.partName[i] == gameObject.name)
                {
                    found = true;
                }
            }

            // If no matching part is found, hide the sticker object.
            if (!found)
                gameObject.SetActive(false);
        }

        // Try to get a DecalProjector component.
        gameObject.TryGetComponent<DecalProjector>(out DecalProjector _projector);

        if (_projector != null)
        {
            Texture _stickerTexture = _projector.material.GetTexture("Base_Map");

            // If the sticker texture is null, set the fade factor to 0.
            if (_stickerTexture == null)
                _projector.fadeFactor = 0f;
            else
                _projector.fadeFactor = 1f; // Otherwise, set the fade factor to 1.
        }
    }
}
