using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Audio Sources
    public AudioSource runningSound;   // Sound for when the car is running
    public AudioSource idleSound;     // Sound for when the car is idle
    public AudioSource reverseSound;  // Sound for when the car is moving in reverse
    public AudioSource crashSound;    // Sound for collision or crashes

    // Pitch settings for audio
    public float idlePitch;            // Pitch for the idle sound
    public float runningMinPitch;      // Minimum pitch for running sound
    public float runningMaxPitch;      // Maximum pitch for running sound

    // Volume settings for audio
    public float runningMaxVol;        // Maximum volume for running sound
    public float idleMaxVol;           // Maximum volume for idle sound
    public float revMaxVol;            // Maximum volume for reverse sound
    public float crashMaxVol;          // Maximum volume for crash sound

    // Array of collision sounds
    public AudioClip[] collisionSounds; // Array of collision sound effects

    public SelectedCar selectedCar;    // Reference to the SelectedCar script
    private CarController controller;  // Reference to the CarController script

    // Variables for controlling audio based on car speed and direction
    private float speedRatio;    // Ratio of car speed
    private float carSpeed;      // Magnitude of car speed
    private float revLimiter;    // Reverse limiter value

    void Start()
    {
        // Initialize the CarController reference and set initial volumes
        controller = GetComponent<CarController>();
        runningSound.volume = 0f;
        reverseSound.volume = 0f;
        crashSound.volume = 0f;

        // Set the pitch for the idle sound
        idleSound.pitch = idlePitch;
    }

    void Update()
    {
        if (selectedCar.isCarChosen)
        {
            crashSound.volume = crashMaxVol;
            speedRatio = Mathf.Abs(controller.GetSpeedRatio());
            carSpeed = Mathf.Abs(controller.speed);
            float _direction = controller.movingDirection;

            // Adjust volume of idle sound based on speed ratio
            idleSound.volume = Mathf.Lerp(.05f, idleMaxVol, speedRatio);

            if (_direction > 0.5f && carSpeed > 5f)
            {
                // Adjust volume and pitch of running sound for forward motion
                runningSound.volume = Mathf.Lerp(0, runningMaxVol, speedRatio);
                runningSound.pitch = Mathf.Lerp(runningSound.pitch,
                    Mathf.Lerp(runningMinPitch, runningMaxPitch, speedRatio) +
                    revLimiter, Time.deltaTime);
                reverseSound.volume = 0;
            }
            else if (_direction < -0.5f && carSpeed > 5f)
            {
                // Adjust volume and pitch of reverse sound for reverse motion
                reverseSound.volume = Mathf.Lerp(reverseSound.volume, revMaxVol, speedRatio);
                reverseSound.pitch = 1f;
                runningSound.volume = 0;
            }
            else if (carSpeed <= 5f)
            {
                // Set volumes to 0 if the car speed is very low
                runningSound.volume = 0f;
                reverseSound.volume = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play a random collision sound when a collision occurs
        int randomIndex = Random.Range(0, collisionSounds.Length);
        crashSound.PlayOneShot(collisionSounds[randomIndex]);
    }
}
