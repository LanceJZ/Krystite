using System.Collections;
using UnityEngine;

public class Shot : MonoBehaviour
{
    // Use this for class variables.

    public GameObject explodeFX;
    public float shotLife;
    public float shotSpeed;
    public bool fromEnemy;

    private float shotTimer;
    private CheckBounds checkBounds;
    private Player player;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        shotLife = 1.35f;
        shotSpeed = 300f;
    }

    // Use this for initialization when game starts.
    private void Start()
    {
        shotTimer = Time.time + shotLife;
    }

    // Update is called once per frame.
    private void Update()
    {
        if (shotTimer < Time.time)
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }

    // Update is called by physics.
    private void FixedUpdate()
    {
    }

    // When Collider is triggered by other.
    private void OnTriggerEnter(Collider other)
    {
        if (tag == "PlayerShot" && other.tag == "Player")
            return;

        if (tag == "PlayerSuperShot" && other.tag == "Player")
            return;

        if (tag == "PlayerSuperShot" && other.tag == "Rock")
            return;

        if (tag == "PlayerSuperShot" && other.tag == "EnemyShot")
            return;

        if (tag == "PlayerSuperShot" && other.tag == "Krystite")
            return;

        if (tag == "EnemyShot" && other.tag == "Enemy")
            return;

        if (tag == "EnemyShot" && other.tag == "PlayerShot")
            return;

        if (tag == "BossShot" && other.tag == "Boss")
            return;

        if (other.tag == "PlayerSuperShot")
            return;

        if (other.tag == "PlayerShot")
            return;

        if (other.tag == "EnemyShot")
            return;

        if (other.tag == "EnemyDetect")
            return;

        if (gameObject != null)
        {
            Instantiate(explodeFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (player != null)
            player.ShotDone();
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }

    public void SetUp(Vector3 Position, Vector3 Velocity, Quaternion Rotation, Player playerReference, float life)
    {
        player = playerReference;
        transform.position = Position;
        transform.rotation = Rotation;
        GetComponent<Rigidbody>().velocity = Velocity;
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shotSpeed);
        shotLife = life;
    }

    public void SetUp(Vector3 Position, Quaternion Rotation, float life)
    {
        transform.position = Position;
        transform.rotation = Rotation;
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shotSpeed);
        shotLife = life;
    }

    public void SetUp(Vector3 Position, Vector3 Velocity)
    {
        transform.position = Position;
        GetComponent<Rigidbody>().velocity = Velocity;
    }
}