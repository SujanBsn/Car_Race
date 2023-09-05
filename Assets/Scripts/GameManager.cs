using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ColourPickerCanvas;
    public GameObject CarSelectionCanvas;
    public GameObject RacingCanvas;
    public GameObject Vehicles;
    public GameObject Terrain;
    public GameObject currentCar;

    public Transform rotatePos;

    public SelectedCar selectedCar;

    public GameObject[] cars;

    public CarParts[] parts;

    int currentId = 1;

    private void Awake()
    {
        CarSelectionCanvas.SetActive(true);
        ColourPickerCanvas.SetActive(false);
        RacingCanvas.SetActive(false);
        Vehicles.SetActive(false);
        Terrain.SetActive(false);
        selectedCar.isCarChosen = false;

        SetSelection();
    }

    private void Update()
    {
        if(!selectedCar.isCarChosen)
            currentCar.transform.Rotate(new(0, 1, 0), 1);
    }

    public void ReturnFromColorSelectButton()
    {
        foreach (Transform t in ColourPickerCanvas.transform)
        {
            t.gameObject.SetActive(true);
        }

        ColourPickerCanvas.SetActive(false);
        CarSelectionCanvas.SetActive(true);
    }

    public void ColorSelectButton()
    {
        CarSelectionCanvas.SetActive(false);
        ColourPickerCanvas.SetActive(true);
    }

    public void PreviousCarButton()
    {
        if (currentId > 0)
            currentId--;
        else
            currentId = cars.Length - 1;

        SetSelection();
    }

    public void NextCarButton()
    {
        if (currentId < cars.Length - 1) 
            currentId++;
        else
            currentId = 0;

        SetSelection();
    }

    public void SetSelection()
    {
        selectedCar.currentCar = cars[currentId];
        selectedCar.carParts = parts[currentId];
        selectedCar.currentCarID = currentId;
        Destroy(currentCar);
        CreateCar();
    }

    public void StartGameButton()
    {
        CarSelectionCanvas.SetActive(false);
        rotatePos.gameObject.SetActive(false);
        Terrain.SetActive(true);
        Vehicles.SetActive(true);
        RacingCanvas.SetActive(true);
        selectedCar.isCarChosen = true;

        Destroy(currentCar);
        cars[currentId].SetActive(true);
    }

    public void CreateCar()
    {
        currentCar = Instantiate(cars[currentId]);
        currentCar.GetComponent<CarController>().enabled = false;
        currentCar.transform.position = rotatePos.position;
        currentCar.SetActive(true);
    }
}
