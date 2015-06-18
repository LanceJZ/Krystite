using System.Collections;
using UnityEngine;

public class EnemyBossBuilder : MonoBehaviour
{
    // Use this for class variables.

    public GameObject BossObj;
    public GameObject explodeFX;
    public GameObject krystiteObj;
    public int krystite;
    public int points;
    public int health;

    private CheckBounds checkBounds;
    private Score score;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        checkBounds = new CheckBounds(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(),
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>(), 51);

        health = 100;
        points = 1000;
    }

    // Use this for initialization when game starts.
    private void Start()
    {
        // random location
        if (Random.Range(0, 10) > 5)
            transform.position = new Vector3(Random.Range(-checkBounds.gameBounds.x * 0.25f, -checkBounds.gameBounds.x * 0.85f),
                Random.Range(-checkBounds.gameBounds.y * 0.75f, -checkBounds.gameBounds.y * 0.85f), 0);
        else
            transform.position = new Vector3(Random.Range(checkBounds.gameBounds.x * 0.25f, checkBounds.gameBounds.x * 0.85f),
                Random.Range(-checkBounds.gameBounds.y * 0.75f, -checkBounds.gameBounds.y * 0.85f), 0);
    }

    // Update is called once per frame.
    private void Update()
    {
        if (krystite >= 20)
        {
            Instantiate(BossObj, transform.position, transform.rotation);

            krystite -= 20;
        }
    }

    // Update is called by physics.
    private void FixedUpdate()
    {
    }

    // When Collider is triggered by other.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerSuperShot")
        {
            health -= 5;

            if (health <= 0)
            {
                score.AddScore(points);

                for (int i = 0; i < krystite; i++)
                {
                    GameObject spawn = Instantiate(krystiteObj); //Have the Krystite go the direction of the shot.
                    spawn.GetComponent<Krystite>().Setup(transform.position, other.GetComponent<Rigidbody>().velocity * 5);
                }

                Instantiate(explodeFX, transform.position, Quaternion.identity);
                Destroy(gameObject.transform.parent.gameObject);
            }
            else
            {
                int emissionRate = 5;

                if (health < 90)
                    emissionRate = 20;

                if (health < 80)
                    emissionRate = 30;

                if (health < 70)
                    emissionRate = 40;

                if (health < 60)
                    emissionRate = 60;

                if (health < 50)
                    emissionRate = 70;

                if (health < 40)
                    emissionRate = 80;

                if (health < 30)
                    emissionRate = 100;

                if (health < 20)
                    emissionRate = 120;

                if (health < 10)
                    emissionRate = 150;

                for (int i = 0; i < 3; i++)
                {
                    transform.GetChild(i).GetComponent<ParticleSystem>().Play();
                    transform.GetChild(i).GetComponent<ParticleSystem>().emissionRate = emissionRate;
                }
            }
        }
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }

    public void RecieveKrystite(int Amount)
    {
        krystite += Amount;
    }
}