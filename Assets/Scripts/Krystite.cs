using System.Collections;
using UnityEngine;

public class Krystite : MonoBehaviour
{
    // Use this for class variables.

    private CheckBounds checkBounds;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        checkBounds = new CheckBounds(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(),
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>(), 52);
    }

    // Use this for initialization when game starts.
    private void Start()
    {
    }

    // Update is called once per frame.
    private void Update()
    {
        if (transform.position.x > checkBounds.gameBounds.x || transform.position.x < -checkBounds.gameBounds.x ||
            transform.position.y > checkBounds.gameBounds.y || transform.position.y < -checkBounds.gameBounds.y)
            Destroy();
    }

    // Update is called by physics.
    private void FixedUpdate()
    {
    }

    // When Collider is triggered by other.
    private void OnTriggerEnter(Collider other)
    {
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }

    public void Destroy()
    {
        GetComponent<SphereCollider>().enabled = false;
        GameObject.Destroy(gameObject);
    }

    public void Setup(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        velocity.x += Random.Range(-25, 25);
        velocity.y += Random.Range(-25, 25);
        GetComponent<Rigidbody>().AddRelativeForce(velocity);
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 1;
    }
}