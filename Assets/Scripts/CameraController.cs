using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offsetVector;
    public float speed;
    public SelectedCar car;

    private Rigidbody rb;

    private void FixedUpdate()
    {
        if (car.isCarChosen)
        {
            player = car.currentCar.transform;
            rb = player.GetComponent<Rigidbody>();

            Vector3 playerForwardVector = (rb.velocity + player.transform.forward).normalized;
            transform.position = Vector3.Lerp(transform.position,
                player.position + player.transform.TransformVector(offsetVector)
                + playerForwardVector * (-5f), speed * Time.deltaTime);
            transform.LookAt(player);
        }
    }
}
