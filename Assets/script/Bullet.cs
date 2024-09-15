using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float force;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.VelocityChange);
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "IceWall(Clone)")
        {
            other.GetComponent<IceWallScript>().health = 0;
            Destroy(gameObject);
        }
    }
}
