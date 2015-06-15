using UnityEngine;
using System.Collections;

public class EnemyBossBuilder : MonoBehaviour
{
    // Use this for class variables.

    public GameObject BossObj;
    public int krystite;

    private CheckBounds checkBounds;

    // Use this for initialization when scene loads.
    void Awake()
    {
        checkBounds = new CheckBounds(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(),
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>(), 51);
    }

    // Use this for initialization when game starts.
    void Start()
    {
        // random location
        if (Random.Range(0, 10) > 5)
            transform.position = new Vector3(Random.Range(-checkBounds.gameBounds.x * 0.25f, -checkBounds.gameBounds.x * 0.85f),
                Random.Range(-checkBounds.gameBounds.y * 0.25f, -checkBounds.gameBounds.y * 0.85f), 0);
        else
            transform.position = new Vector3(Random.Range(checkBounds.gameBounds.x * 0.25f, checkBounds.gameBounds.x * 0.85f),
                Random.Range(checkBounds.gameBounds.y * 0.25f, checkBounds.gameBounds.y * 0.85f), 0);
    }

    // Update is called once per frame.
    void Update()
    {
        if (krystite >= 10)
        {
            Instantiate(BossObj, transform.position, transform.rotation);

            krystite -= 10;
        }
    }

    // Update is called by physics.
    void FixedUpdate()
    {
    }

    // When Collider is triggered by other.
    void OnTriggerEnter(Collider other)
    {
    }

    // When object other leaves Collider.
    void OnTriggerExit(Collider other)
    {
    }

    public void RecieveKrystite(int Amount)
    {
        krystite = Amount;
    }
}