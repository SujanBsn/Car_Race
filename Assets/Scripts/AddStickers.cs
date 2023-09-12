using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AddStickers : MonoBehaviour
{
    public Texture stickerTexture;
    public SelectedCar selectedCar;
    public SelectedPart selectedPart;
    public int partId;

    private void Update()
    {
        CheckForNullSticker();
    }

    public void SelectpartForSticker()
    {
        selectedPart.partIdForSticker = partId;
    }

    public void AddSticker()
    {
        Material _stickerMat = selectedCar.carParts.
            partSticker[selectedPart.partIdForSticker];

        _stickerMat.EnableKeyword("Base_Map");
        _stickerMat.SetTexture("Base_Map", stickerTexture);
    }

    public void ClearSticker()
    {
        Material _stickerMat = selectedCar.carParts.
            partSticker[selectedPart.partIdForSticker];

        _stickerMat.EnableKeyword("Base_Map");
        _stickerMat.SetTexture("Base_Map", null);
    }

    public void CheckForNullSticker()
    {
        if (!gameObject.CompareTag("sticker"))
        {
            bool found = false;
            for (int i = 0; i < selectedCar.carParts.partName.Count(); i++)
            {
                if (selectedCar.carParts.partName[i] == gameObject.name)
                {
                    found = true;
                }
            }
            if (!found)
                gameObject.SetActive(false);
        }

        gameObject.TryGetComponent<DecalProjector>(out DecalProjector _projector);
        if (_projector != null)
        {
            Texture _stickerTexture = _projector.material.GetTexture("Base_Map");
            if (_stickerTexture == null)
                _projector.fadeFactor = 0f;
            else
                _projector.fadeFactor = 1f;
        }
    }

}
