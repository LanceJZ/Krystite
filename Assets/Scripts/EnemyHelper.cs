using System.Collections;
using UnityEngine;

public class EnemyHelper : MonoBehaviour
{
    // Use this for class variables.

    public AudioClip shotClip;
    public GameObject shot;
    public GameObject krystiteObj;
    public GameObject rockDetectObj;
    public GameObject playerDetectObj;
    public GameObject explodeFX;
    public Vector3 spawnPosition;
    public Vector2 target;
    public float angle;
    public int krystite;
    public bool moveToTarget;
    public bool getKrystite;
    public bool mineRock;
    public bool attackPlayer;
    public bool outOfRange;
    public bool krystiteFull;
    public bool findRock;

    private EnemyKrystiteDetect krystiteDetect; // Enemy Rock Detect sets this.
    private EnemyRockDetect rockDetect;
    private EnemyPlayerDetect playerDetect;
    private Score score;
    private CheckBounds checkBounds;
    private RockControl rockControl;
    private Transform targetKrystite;
    private Transform targetRock;
    private Transform targetPlayer;
    private Transform targetBoss;
    private float targetAngle;
    private float targetTimer;
    private float maxSpeed;
    private float mineTimer;
    private int points;
    private int health;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        score = GameObject.FindGameObjectWithTag("GameController").GetComponent<Score>(); //Give score to player when player hits.
        rockControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<RockControl>();
        checkBounds = new CheckBounds(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(),
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>(), 51);

        GameObject obj = Instantiate(rockDetectObj);
        rockDetect = obj.GetComponent<EnemyRockDetect>();
        rockDetect.helper = this;
        rockDetect.helperTransform = transform;

        GameObject objPlayerDetect = Instantiate(playerDetectObj);
        playerDetect = objPlayerDetect.GetComponent<EnemyPlayerDetect>();
        playerDetect.helper = this;
        playerDetect.helperTransform = transform;

        moveToTarget = true;
        findRock = true;
        maxSpeed = 5;
    }

    // Use this for initialization when game starts.
    private void Start()
    {
        Setup(50);
        points = 50;
        health = 100;
    }

    // Update is called once per frame.
    private void Update()
    {
        if (findRock)
        {
            if (targetTimer < Time.time)
            {
                targetTimer = Time.time + 0.1f + Random.Range(0, 0.25f);
                angle = rockControl.FindClosestTarget(transform.position) * Mathf.Rad2Deg + 90;
                GetComponent<Rigidbody>().MoveRotation(Quaternion.AngleAxis(angle, Vector3.forward));
                rockDetect.Reset();
            }
        }

        if (krystiteFull)
        {
            mineRock = false;
            moveToTarget = true;
            GetTarget(targetBoss.position);
        }
        else
        {
            if (getKrystite)
            {
                if (targetKrystite != null)
                {
                    GetTarget(targetKrystite.position);
                    mineRock = false;
                    moveToTarget = true;
                }
                else
                {
                    getKrystite = false;
                    mineRock = true;
                    moveToTarget = false;
                    mineTimer = Time.time + Random.Range(3, 6);

                    if (targetRock == null)
                    {
                        findRock = true;
                        mineRock = false;
                        moveToTarget = true;
                    }
                }
            }
            else
            {
                if (mineRock)
                {
                    if (targetRock != null)
                    {
                        moveToTarget = false;
                        GetTarget(targetRock.position);

                        if (mineTimer < Time.time)
                        {
                            Shoot();
                            mineTimer = Time.time + Random.Range(3, 6);
                        }
                    }
                    else
                    {
                        mineRock = false;
                        getKrystite = true;
                    }
                }
                else
                {
                    findRock = true;
                    moveToTarget = true;
                }
            }
        }

        if (moveToTarget)
        {
            maxSpeed = 5;
            GetComponentInChildren<ParticleSystem>().Play();
            GetComponent<Rigidbody>().drag = 0.15f;
        }
        else
        {
            maxSpeed = 0;
            GetComponentInChildren<ParticleSystem>().Stop();
            GetComponent<Rigidbody>().drag = 10;
        }

        if (attackPlayer)
        {
            if (targetPlayer != null)
                GetTarget(targetPlayer.position);
            else
                attackPlayer = false;

            if (mineTimer < Time.time)
            {
                Shoot();
                mineTimer = Time.time + Random.Range(2, 4);
            }
        }
    }

    // Update is called by physics.
    private void FixedUpdate()
    {
        if (GetComponent<Rigidbody>().velocity.x < maxSpeed)
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * Random.Range(0, maxSpeed));
        }

        if (GetComponent<Rigidbody>().velocity.x > -maxSpeed)
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * -Random.Range(0, maxSpeed));
        }

        if (GetComponent<Rigidbody>().velocity.y < maxSpeed)
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * Random.Range(0, maxSpeed));
        }

        if (GetComponent<Rigidbody>().velocity.x < -maxSpeed)
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * Random.Range(0, maxSpeed));
        }

        GetComponent<Rigidbody>().velocity = checkBounds.BounceOffEdge(transform.position, GetComponent<Rigidbody>().velocity);
        GetComponent<Rigidbody>().MovePosition(checkBounds.CheckOffWorld(transform.position));
    }

    // When Collider is triggered by other.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Krystite")
        {
            krystite++;
            other.GetComponent<Krystite>().Destroy();
            getKrystite = false;
            mineRock = true;
            moveToTarget = false;
            mineTimer = Time.time + 2.5f;

            if (outOfRange)
            {
                rockDetect.Reset();
                moveToTarget = true;
                mineRock = false;
                GetTarget(targetRock.position);
                outOfRange = false;
            }

            if (krystite >= 10)
            {
                krystiteFull = true;
                ClosestBossBuilder();
            }
        }

        if (other.tag == "Rock")
            MineRock(other.transform);

        if (other.tag == "PlayerSuperShot")
        {
            health -= 50;

            if (health <= 0)
            {
                if (krystiteDetect != null)
                    krystiteDetect.Destroy();

                if (rockDetect != null)
                    rockDetect.Destroy();

                if (playerDetect != null)
                    playerDetect.Destroy();

                score.AddScore(points);
                Instantiate(explodeFX, transform.position, Quaternion.identity);

                for (int i = 0; i < krystite; i++)
                {
                    GameObject spawn = Instantiate(krystiteObj); //Have the Krystite go the direction of the shot.
                    spawn.GetComponent<Krystite>().Setup(transform.position, other.GetComponent<Rigidbody>().velocity * 5);
                }

                Destroy(gameObject.transform.parent.gameObject);
            }
        }

        if (other.tag == "BossBuilder")
        {
            other.GetComponent<EnemyBossBuilder>().RecieveKrystite(10);
            krystite -= 10;
            krystiteFull = false;
            findRock = true;
        }
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }

    private void OnDestroy()
    {
    }

    private void ClosestBossBuilder()
    {
        int closest = 0;

        GameObject[] bossBuilders = GameObject.FindGameObjectsWithTag("BossBuilder");

        for (int i = 0; i < bossBuilders.Length - 1; i++)
        {
            if (Vector3.Distance(transform.position, bossBuilders[i].GetComponent<Transform>().position) <
                Vector3.Distance(transform.position, bossBuilders[i + 1].GetComponent<Transform>().position))
            {
                closest = i;
            }
        }

        targetBoss = bossBuilders[closest].GetComponent<Transform>();
    }

    private void GetTarget(Vector3 Target)
    {
        Vector3 heading = Target - transform.position;
        target = Target;
        Vector3 rockHeading2D = new Vector2(heading.x, heading.y).normalized;
        angle = Mathf.Atan2(rockHeading2D.y, rockHeading2D.x) * Mathf.Rad2Deg + 90;
        GetComponent<Rigidbody>().MoveRotation(Quaternion.AngleAxis(angle, Vector3.forward));
    }

    private void Shoot()
    {
        for (int i = 0; i < 2; i++)
            FireShot(i);
    }

    private void FireShot(int gun)
    {
        GameObject thisShotObj = Instantiate(shot);
        thisShotObj.GetComponentInChildren<AudioSource>().volume = 0.005f;
        thisShotObj.GetComponentInChildren<AudioSource>().clip = shotClip;
        thisShotObj.GetComponentInChildren<AudioSource>().loop = false;
        thisShotObj.GetComponentInChildren<AudioSource>().Play();
        thisShotObj.tag = "EnemyShot";
        thisShotObj.GetComponentInChildren<Shot>().SetUp(gameObject.transform.GetChild(gun).transform.position,
            gameObject.transform.GetChild(gun).transform.rotation, 1.75f);
    }

    public void SendToKrystite(Transform Target)
    {
        targetKrystite = Target;
        moveToTarget = true;
        getKrystite = true;
        findRock = false;
    }

    public void MineRock(Transform Target)
    {
        targetRock = Target;
        moveToTarget = false;
        mineRock = true;
        findRock = false;
    }

    public void AttackPlayer(Transform Target)
    {
        targetPlayer = Target;
        moveToTarget = false;
        mineRock = false;
        findRock = false;
        attackPlayer = true;
    }

    public void PlayerGone()
    {
        if (krystiteDetect != null)
            mineRock = true;
        else
            findRock = true;

        attackPlayer = false;
    }

    public void Setup(int Points)
    {
        Vector3 position = Vector3.zero;
        points = Points;

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

    public void SetKrystite(EnemyKrystiteDetect KrystiteDetectscript)
    {
        krystiteDetect = KrystiteDetectscript;
    }
}