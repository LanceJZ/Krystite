using System.Collections;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    // Use this for class variables.

    public GameObject superShotObj;
    public AudioClip shotClip;
    public GameObject explodeFX;
    public int krystite;
    public int health;

    private Score score;
    private Transform playerTransform;
    private Transform[] gunsTransform = new Transform[4];
    private Vector3 rotate;
    private float maxSpeed;
    private float maxTurnRate;
    private float gunTimer;
    private float gunTimerAmount;
    private float moveTimer;
    private float moveTimerAmount;
    private float rotateTimer;
    private float rotateTimerAmount;
    private int points;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            gunsTransform[i] = gameObject.transform.GetChild(i).transform;
        }

        maxSpeed = 10;
        maxTurnRate = 3;
        gunTimerAmount = 3;
        moveTimerAmount = 5;
        rotateTimerAmount = 3;
        health = 100;
        points = 1000;
    }

    // Use this for initialization when game starts.
    private void Start()
    {
        score = GameObject.FindGameObjectWithTag("GameController").GetComponent<Score>(); //Give score to player when player hits.
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame.
    private void Update()
    {
        if (rotateTimer < Time.time)
        {
            rotate = Vector3.forward * Random.Range(-maxTurnRate, maxTurnRate);
            rotateTimer = Time.time + rotateTimerAmount;
        }

        if (gunTimer < Time.time)
        {
            int gun = Random.Range(0, 3);
            GameObject spawn = Instantiate(superShotObj);
            spawn.GetComponent<Shot>().SetUp(gunsTransform[gun].position, gunsTransform[gun].rotation, 2);
            spawn.GetComponentInChildren<AudioSource>().volume = 0.005f;
            spawn.GetComponentInChildren<AudioSource>().clip = shotClip;
            spawn.GetComponentInChildren<AudioSource>().loop = false;
            spawn.GetComponentInChildren<AudioSource>().Play();
            spawn.tag = "BossShot";

            gunTimer = Time.time + gunTimerAmount;
        }
    }

    // Update is called by physics.
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().angularVelocity = rotate;

        if (moveTimer < Time.time)
        {
            if (playerTransform != null)
                GetComponent<Rigidbody>().AddForce((playerTransform.position * 20 - transform.position * 20) * Random.Range(maxSpeed * 0.5f, maxSpeed));

            moveTimer = Time.time + Random.Range(moveTimerAmount * 0.25f, moveTimerAmount);
        }
    }

    // When Collider is triggered by other.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerSuperShot")
        {
            health -= 13;

            if (health <= 0)
            {
                score.AddScore(points);
                Instantiate(explodeFX, transform.position, Quaternion.identity);
                GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyController>().BoulderDestroyed();
                Destroy(gameObject.transform.parent.gameObject);
            }
            else
            {
                transform.FindChild("Damage").GetComponent<ParticleSystem>().Play();
                transform.FindChild("Damage").GetComponent<ParticleSystem>().emissionRate = 10;

                if (health < 87)
                    transform.FindChild("Damage").GetComponent<ParticleSystem>().emissionRate = 26;

                if (health < 74)
                    transform.FindChild("Damage").GetComponent<ParticleSystem>().emissionRate = 52;

                if (health < 61)
                    transform.FindChild("Damage").GetComponent<ParticleSystem>().emissionRate = 66;

                if (health < 48)
                    transform.FindChild("Damage").GetComponent<ParticleSystem>().emissionRate = 100;

                if (health < 35)
                    transform.FindChild("Damage").GetComponent<ParticleSystem>().emissionRate = 113;

                if (health < 22)
                    transform.FindChild("Damage").GetComponent<ParticleSystem>().emissionRate = 126;
            }
        }
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }
}