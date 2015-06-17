using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Use this for class variables.

    public AudioClip shotClip;
    public AudioClip thrustClip;
    public GameObject shot;
    public GameObject superShot;
    public GameObject explodeFX;
    public GameObject borderY;
    public GameObject borderX;
    public float shotDelay;
    public float maxSpeed;
    public float thrustAmount;
    public float rotateAmount;

    private CheckBounds checkBounds;
    private Game game;
    private Score score;
    private Arrow arrowRock;
    private Arrow arrowBoss;
    private Arrow arrowMiner;
    private RockControl rockControl;
    private Transform borderYTopTransform;
    private Transform borderXRightTransform;
    private Transform borderYBottomTransform;
    private Transform borderXLeftTransform;
    private Transform[] bossTransforms = new Transform[4];
    private float rotation;
    private float timer;
    private float shotTimer;
    private float arrowTimer;
    private int health;
    private int shotNumber;
    private int krystiteAmout;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        health = 100;
        shotDelay = 0.25f;
        maxSpeed = 15f;
        thrustAmount = 4.725f;
        rotateAmount = 0.925f;

        checkBounds = new CheckBounds(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(),
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>(), 55);
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        score = GameObject.FindGameObjectWithTag("GameController").GetComponent<Score>();
        rockControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<RockControl>();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().Setup(transform, GetComponent<Rigidbody>());

        GameObject spawn;
        spawn = Instantiate(borderY);
        borderYTopTransform = spawn.GetComponent<Transform>();
        spawn = Instantiate(borderY);
        borderYBottomTransform = spawn.GetComponent<Transform>();
        spawn = Instantiate(borderX);
        borderXRightTransform = spawn.GetComponent<Transform>();
        spawn = Instantiate(borderX);
        borderXLeftTransform = spawn.GetComponent<Transform>();

        score.PlayerHasHealth(100);
    }

    // Use this for initialization when game starts.
    private void Start()
    {
        arrowRock = GameObject.FindGameObjectWithTag("ArrowRock").GetComponent<Arrow>();
        arrowBoss = GameObject.FindGameObjectWithTag("ArrowBoss").GetComponent<Arrow>();
        arrowMiner = GameObject.FindGameObjectWithTag("ArrowMiner").GetComponent<Arrow>();

        score.AddScore(0);

        Vector3 position = Vector3.zero;
        position.x = checkBounds.gameBounds.x;
        position.y = checkBounds.gameBounds.y;

        borderXRightTransform.position = position;
        borderYTopTransform.position = position;

        position.x = -checkBounds.gameBounds.x;
        position.y = -checkBounds.gameBounds.y;

        borderXLeftTransform.position = position;
        borderYBottomTransform.position = position;
    }

    // Update is called once per frame.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (shotNumber < 6)
            {
                GameObject thisShotObj = Instantiate(shot);
                thisShotObj.GetComponentInChildren<AudioSource>().volume = 0.1f;
                thisShotObj.GetComponentInChildren<AudioSource>().clip = shotClip;
                thisShotObj.GetComponentInChildren<AudioSource>().loop = false;
                thisShotObj.GetComponentInChildren<AudioSource>().Play();
                //thisShotObj.transform.GetChild(0).tag = "PlayerShot";
                thisShotObj.GetComponent<Transform>().tag = "PlayerShot";
                Vector3 gun = gameObject.transform.GetChild(1).transform.position;
                thisShotObj.GetComponentInChildren<Shot>().SetUp(gun, GetComponent<Rigidbody>().velocity, transform.rotation, this, 2.5f);
                //thisShotObj.GetComponentInChildren<Shot>().shotSpeed = 400;
                shotNumber++;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (krystiteAmout >= 1)
            {
                GameObject thisShotObj = Instantiate(superShot);
                thisShotObj.GetComponentInChildren<AudioSource>().volume = 0.1f;
                thisShotObj.GetComponentInChildren<AudioSource>().clip = shotClip;
                thisShotObj.GetComponentInChildren<AudioSource>().loop = false;
                thisShotObj.GetComponentInChildren<AudioSource>().Play();
                Vector3 gun = gameObject.transform.GetChild(1).transform.position;
                thisShotObj.GetComponentInChildren<Shot>().SetUp(gun, GetComponent<Rigidbody>().velocity, transform.rotation, this, 5.25f);
                thisShotObj.GetComponentInChildren<Shot>().shotSpeed = 100;
                krystiteAmout -= 1;
                score.PlayerHasKrystite(krystiteAmout);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (krystiteAmout >= 5)
            {
                krystiteAmout -= 5;
                health += 50;

                if (health > 100)
                    health = 100;

                score.PlayerHasKrystite(krystiteAmout);
                score.PlayerHasHealth(health);
            }
        }

        if (arrowTimer < Time.time)
        {
            arrowRock.Render(true);
            arrowRock.PointDirection(rockControl.FindClosestTarget(transform.position) * Mathf.Rad2Deg - 90);

            int closest = 0;

            GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

            if (bosses.Length > 1)
            {
                for (int i = 0; i < bosses.Length - 1; i++)
                {
                    if (Vector3.Distance(transform.position, bosses[i + 1].GetComponent<Transform>().position) <
                        Vector3.Distance(transform.position, bosses[closest].GetComponent<Transform>().position))
                    {
                        closest = i + 1;
                    }
                }
            }

            if (bosses.Length > 0)
            {
                arrowBoss.Render(true);
                Vector3 rockHeading = bosses[closest].GetComponent<Transform>().position - transform.position;
                Vector2 rockHeading2D = new Vector2(rockHeading.x, rockHeading.y).normalized;
                float arrowangle = Mathf.Atan2(rockHeading2D.y, rockHeading2D.x);
                arrowBoss.PointDirection(arrowangle * Mathf.Rad2Deg - 90);
            }
            else
            {
                arrowBoss.Render(false);
            }

            closest = 0;
            GameObject[] miners = GameObject.FindGameObjectsWithTag("Enemy");

            if (miners.Length > 1)
            {
                for (int i = 0; i < miners.Length - 1; i++)
                {
                    if (Vector3.Distance(transform.position, miners[i + 1].GetComponent<Transform>().position) <
                        Vector3.Distance(transform.position, miners[closest].GetComponent<Transform>().position))
                    {
                        closest = i + 1;
                    }
                }
            }

            if (miners.Length > 0)
            {
                arrowMiner.Render(true);
                Vector3 rockHeading = miners[closest].GetComponent<Transform>().position - transform.position;
                Vector2 rockHeading2D = new Vector2(rockHeading.x, rockHeading.y).normalized;
                float arrowangle = Mathf.Atan2(rockHeading2D.y, rockHeading2D.x);
                arrowMiner.PointDirection(arrowangle * Mathf.Rad2Deg - 90);
            }
            else
            {
                arrowMiner.Render(false);
            }

            arrowTimer = Time.time + 0.1f;
        }

        Vector3 position = Vector3.zero;

        position.x = transform.position.x;
        position.y = borderYTopTransform.position.y;
        borderYTopTransform.position = position;
        position.y = borderYBottomTransform.position.y;
        borderYBottomTransform.position = position;

        position.y = transform.position.y;
        position.x = borderXLeftTransform.position.x;
        borderXLeftTransform.position = position;
        position.x = borderXRightTransform.position.x;
        borderXRightTransform.position = position;
    }

    // Update is called by physics.
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, -rotateAmount, 0));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, rotateAmount, 0));
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * thrustAmount, ForceMode.Acceleration);

            GetComponentInChildren<ParticleSystem>().Play();

            if (!GetComponent<AudioSource>().isPlaying && thrustClip != null)
            {
                GetComponent<AudioSource>().clip = thrustClip;
                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            GetComponent<AudioSource>().Stop();
            GetComponentInChildren<ParticleSystem>().Stop();
        }

        if (GetComponent<Rigidbody>().velocity.x > maxSpeed)
            GetComponent<Rigidbody>().velocity = new Vector3(maxSpeed, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);

        if (GetComponent<Rigidbody>().velocity.x < -maxSpeed)
            GetComponent<Rigidbody>().velocity = new Vector3(-maxSpeed, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);

        if (GetComponent<Rigidbody>().velocity.y > maxSpeed)
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, maxSpeed, GetComponent<Rigidbody>().velocity.z);

        if (GetComponent<Rigidbody>().velocity.y < -maxSpeed)
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, -maxSpeed, GetComponent<Rigidbody>().velocity.z);

        GetComponent<Rigidbody>().velocity = checkBounds.BounceOffEdge(transform.position, GetComponent<Rigidbody>().velocity);
        GetComponent<Rigidbody>().MovePosition(checkBounds.CheckOffWorld(transform.position));
    }

    // When Collider is triggered by other.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerShot")
            return;

        if (other.tag == "Rock")
            ShipHit(5);

        if (other.tag == "EnemyShot")
            ShipHit(10);

        if (other.tag == "Enemy")
            ShipHit(20);

        if (other.tag == "Krystite")
        {
            krystiteAmout++;
            score.PlayerCollectedKrystite();
            score.PlayerHasKrystite(krystiteAmout);
            other.GetComponent<Krystite>().Destroy();
        }

        if (other.tag == "Boss")
            ShipHit(10);

        if (other.tag == "BossShot")
            ShipHit(25);
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }

    private void ShipHit(int points)
    {
        if (health > points)
        {
            health -= points;
            score.PlayerHasHealth(health);
        }
        else
        {
            Instantiate(explodeFX, transform.position, Quaternion.identity);
            game.PlayerDestroyed();
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void ShotDone()
    {
        shotNumber--;
    }
}