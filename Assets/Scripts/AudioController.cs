using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource runningSound;
    public AudioSource idleSound;
    public AudioSource reverseSound;

    public float limiterSound = 1f;
    public float limiterFreq = 3f;
    public float limiterEngage = .8f;

    private CarController controller;

    private float speedRatio;
    private float revLimiter;

    void Start()
    {
        controller = GetComponent<CarController>();
        runningSound.volume = 0f;
        reverseSound.volume = 0f;
    }

    void Update()
    {
        speedRatio = Mathf.Abs(controller.GetSpeedRatio());

        float _direction = Mathf.Sign(controller.GetSpeedRatio());

        if (speedRatio > limiterEngage)
            revLimiter = (Mathf.Sin(Time.time * limiterFreq) + 1f) * 
                limiterSound * (speedRatio - limiterEngage);

        idleSound.volume = Mathf.Lerp(.1f, 1f, speedRatio);

        if (_direction > 0)
        {
            runningSound.volume = Mathf.Lerp(.3f, 1f, speedRatio);
            runningSound.pitch = Mathf.Lerp(runningSound.pitch, Mathf.Lerp(.3f, 1.5f, speedRatio) +
                revLimiter, Time.deltaTime);
            reverseSound.volume = 0;
        }
        else
        {
            reverseSound.volume = Mathf.Lerp(.3f, .5f, speedRatio);
            reverseSound.pitch = 0.3f;
            runningSound.volume = 0;
        }
    }
}
