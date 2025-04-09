using UnityEngine;

public class Basketball : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.AddRelativeForce(new(0, 2 + rb.linearVelocity.y * 0.2f, 0), ForceMode.Impulse);
    }
}
