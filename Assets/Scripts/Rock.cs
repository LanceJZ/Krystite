using System.Collections;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // Use this for class variables.

    public GameObject explodeFX;
    public GameObject krystite;
    public Vector3 spawnPosition;
    public float dropChance;
    public int amountOfKrystite;

    private float dropTimer;
    private CheckBounds checkBounds;
    private RockControl rockControl;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        checkBounds = new CheckBounds(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(),
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>(), 50);
        rockControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<RockControl>();
        amountOfKrystite = Random.Range(1, 10);
        dropChance = 15.25f;
    }

    // Use this for initialization when game starts.
    private void Start()
    {
    }

    // Update is called once per frame.
    private void Update()
    {
    }

    // Update is called by physics.
    private void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.z = 0.0f;
        GetComponent<Rigidbody>().MovePosition(position);

        GetComponent<Rigidbody>().velocity = checkBounds.BounceOffEdge(transform.position, GetComponent<Rigidbody>().velocity);
        GetComponent<Rigidbody>().MovePosition(checkBounds.CheckOffWorld(transform.position));
    }

    // When Collider is triggered by other.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rock" || other.tag == "Respawn")
            return;

        if (other.tag == "EnemyShot" || other.tag == "PlayerShot")
        {
            if (amountOfKrystite > 0)
            {
                if (Random.Range(0, 100) < dropChance && dropTimer < Time.time)
                {
                    GameObject spawn = Instantiate(krystite); //Have the Krystite go the direction of the shot.
                    spawn.GetComponent<Krystite>().Setup(transform.position, other.GetComponent<Rigidbody>().velocity * 5);
                    dropTimer = Time.time + amountOfKrystite;
                    amountOfKrystite--;
                }
            }
            else
            {
                Instantiate(explodeFX, transform.position, Quaternion.identity);
                rockControl.RockDestroyed();
                Destroy(gameObject);
            }
        }
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }

    public void SetTumble(float tumble, float speed)
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;

        float rockSpeed = Random.Range(speed * 0.5f, speed);
        float angle = Random.Range(0, 6.28318530718f);
        Vector3 velocity = Vector3.zero;
        float sinRot = Mathf.Sin(angle);
        float cosRot = Mathf.Cos(angle);
        velocity.x = cosRot * rockSpeed;
        velocity.y = sinRot * rockSpeed;

        GetComponent<Rigidbody>().velocity = velocity;
    }

    public void Setup(Vector3 scale)
    {
        Vector3 position = Vector3.zero;
        transform.localScale = scale;

        if (Random.Range(0, 10) > 5)
        {
            position.x = Random.Range(-checkBounds.screenBounds.x, -checkBounds.gameBounds.x);
        }
        else
        {
            position.x = Random.Range(checkBounds.screenBounds.x, checkBounds.gameBounds.x);
        }

        if (Random.Range(0, 10) > 5)
        {
            position.y = Random.Range(-checkBounds.screenBounds.y, -checkBounds.gameBounds.y);
        }
        else
        {
            position.y = Random.Range(checkBounds.screenBounds.y, checkBounds.gameBounds.y);
        }

        transform.position = position;
        spawnPosition = position;
    }
}