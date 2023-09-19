using UnityEngine;

public class GameManager : MonoBehaviour
{
    // References to various UI and game objects.
    public GameObject CustomizeCanvas; // The canvas for car customization.
    public GameObject ColourObjects; // GameObjects for color customization.
    public GameObject StickerObjects; // GameObjects for sticker customization.
    public GameObject CarSelectionCanvas; // The canvas for car selection.
    public GameObject RacingCanvas; // The canvas for the racing game.
    public GameObject Vehicles; // The parent object for car models.
    public GameObject Terrain; // The terrain object for racing.
    public GameObject currentCar; // The currently displayed car.

    // Transform positions for car customization.
    public Transform rotatePos; // Position for rotating the car during customization.
    public Transform beginPos; // Starting position for the car in the game.

    // Reference to the selected car and its customization.
    public SelectedCar selectedCar;

    // Arrays for storing available car models and their parts.
    public GameObject[] cars; // Array of car prefabs.
    public CarParts[] parts; // Array of car parts for each car model.

    // Current car ID and index for tracking car selection.
    int currentId = 0;

    private void Awake()
    {
        // Initialize the game state when the game starts.
        CarSelectionCanvas.SetActive(true); // Show the car selection canvas.
        CustomizeCanvas.SetActive(false); // Hide the customization canvas.
        RacingCanvas.SetActive(false); // Hide the racing canvas.
        Vehicles.SetActive(false); // Hide car models.
        Terrain.SetActive(false); // Hide the terrain.
        selectedCar.isCarChosen = false; // Initialize car selection status.

        // Set the initial car selection.
        SetSelection();
    }

    private void Update()
    {
        // Rotate the current car when a car is not yet chosen.
        if (!selectedCar.isCarChosen)
            currentCar.transform.Rotate(new Vector3(0, 1, 0), 0.5f);
    }

    // Return to color selection from customization.
    public void ReturnFromColorSelectButton()
    {
        foreach (Transform t in ColourObjects.transform)
        {
            t.gameObject.SetActive(true); // Activate color selection objects.
        }

        CustomizeCanvas.SetActive(false); // Hide customization canvas.
        StickerObjects.SetActive(false); // Hide sticker customization objects.
        CarSelectionCanvas.SetActive(true); // Show car selection canvas.
    }

    // Return to sticker selection from customization.
    public void ReturnFromStickerButton()
    {
        foreach (Transform t in StickerObjects.transform)
        {
            t.gameObject.SetActive(true); // Activate sticker selection objects.
        }

        StickerObjects.SetActive(false); // Hide sticker customization objects.
        ColourObjects.SetActive(true); // Show color customization objects.
    }

    // Handle the color selection process.
    public void ColorSelectButton()
    {
        CarSelectionCanvas.SetActive(false); // Hide car selection canvas.
        CustomizeCanvas.SetActive(true); // Show customization canvas.
        ColourObjects.SetActive(true); // Show color customization objects.
    }

    // Handle the sticker selection process.
    public void StickerSelectButton()
    {
        ColourObjects.SetActive(false); // Hide color customization objects.
        StickerObjects.SetActive(true); // Show sticker customization objects.
    }

    // Navigate to the previous car in the car selection list.
    public void PreviousCarButton()
    {
        if (currentId > 0)
            currentId--; // Decrement the current car index.
        else
            currentId = cars.Length - 1; // Wrap around to the last car.

        // Update the car selection.
        SetSelection();
    }

    // Navigate to the next car in the car selection list.
    public void NextCarButton()
    {
        if (currentId < cars.Length - 1)
            currentId++; // Increment the current car index.
        else
            currentId = 0; // Wrap around to the first car.

        // Update the car selection.
        SetSelection();
    }

    // Set the selected car and its associated parts.
    public void SetSelection()
    {
        selectedCar.currentCar = cars[currentId]; // Set the current car model.
        selectedCar.carParts = parts[currentId]; // Set the current car's parts.
        selectedCar.currentCarID = currentId; // Set the current car's ID.
        Destroy(currentCar); // Destroy the current displayed car.
        CreateCarToDisplay(); // Create and display the selected car.
    }

    // Start the game with the selected car.
    public void StartGameButton()
    {
        CarSelectionCanvas.SetActive(false); // Hide car selection canvas.
        rotatePos.gameObject.SetActive(false); // Disable car rotation position.
        Terrain.SetActive(true); // Show the game terrain.
        Vehicles.SetActive(true); // Show car models.
        RacingCanvas.SetActive(true); // Show the racing canvas.
        selectedCar.isCarChosen = true; // Mark the car as chosen.

        // Destroy the current displayed car and position the selected car.
        Destroy(currentCar);
        cars[currentId].transform.position = beginPos.position;
        cars[currentId].transform.rotation = beginPos.rotation;
        cars[currentId].SetActive(true); // Activate the selected car.
    }

    // Create and display the selected car for customization.
    public void CreateCarToDisplay()
    {
        currentCar = Instantiate(cars[currentId]); // Instantiate the car model.
        currentCar.GetComponent<CarController>().enabled = false; // Disable car control.
        currentCar.transform.position = rotatePos.position; // Set the car's position.
        currentCar.SetActive(true); // Activate the selected car.
    }
}
