using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource runningSound;
    public AudioSource idleSound;
    public AudioSource reverseSound;
    public AudioSource crashSound;

    public float runningMaxVol;
    public float idleMaxVol;
    public float revMaxVol;
    public float crashMaxVol;

    public AudioClip[] collisionSounds;

    public SelectedCar selectedCar;
    private CarController controller;

    private float speedRatio;
    private float carSpeed;
    private float revLimiter;

    void Start()
    {
        controller = GetComponent<CarController>();
        runningSound.volume = 0f;
        reverseSound.volume = 0f;
        crashSound.volume = 0f;
    }

    void Update()
    {
        if (selectedCar.isCarChosen)
        {
            crashSound.volume = crashMaxVol;
            speedRatio = Mathf.Abs(controller.GetSpeedRatio());
            carSpeed = Mathf.Abs(controller.speed);
            float _direction = controller.movingDirection;

            idleSound.volume = Mathf.Lerp(.1f, idleMaxVol, speedRatio);

            if (_direction > 0.5f && carSpeed > 5f)
            {
                runningSound.volume = Mathf.Lerp(0, runningMaxVol, speedRatio);
                runningSound.pitch = Mathf.Lerp(runningSound.pitch, Mathf.Lerp(1f, 1.5f, speedRatio) +
                    revLimiter, Time.deltaTime);
                reverseSound.volume = 0;
            }
            else if (_direction < -0.5f && carSpeed > 5f)
            {
                reverseSound.volume = Mathf.Lerp(reverseSound.volume, revMaxVol, speedRatio);
                reverseSound.pitch = 1f;
                runningSound.volume = 0;
            }
            else if (carSpeed <= 5f)
            {
                runningSound.volume = 0f;
                reverseSound.volume = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        int randomIndex = Random.Range(0, collisionSounds.Length);
        crashSound.PlayOneShot(collisionSounds[randomIndex]);
    }
}
