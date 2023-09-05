using System.Linq;
using UnityEngine;

public class PartSelectorButton : MonoBehaviour
{
    public SelectedPart selectedPart;
    public SelectedCar selectedCar;

    public Material material;

    private void Update()
    {
        GetMaterial();
    }

    void GetMaterial()
    {
        bool found = false;
        for (int i = 0; i < selectedCar.carParts.partName.Count(); i++)
        {
            if (selectedCar.carParts.partName[i] == gameObject.name)
            {
                material = selectedCar.carParts.partMaterial[i];
                found = true;
            }
        }
        if (!found)
            gameObject.SetActive(false);
    }

    public void Setpart()
    {
        Debug.Log(material);
        selectedPart.SelectedMaterial = material;
    }
}
