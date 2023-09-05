using UnityEngine;

[CreateAssetMenu]
public class SelectedCar : ScriptableObject
{
    public CarParts carParts;
    public GameObject currentCar;
    public int currentCarID;
    public bool isCarChosen;
}
