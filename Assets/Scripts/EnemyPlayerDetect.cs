using UnityEngine;
using System.Collections;

public class EnemyPlayerDetect : MonoBehaviour
{
    // Use this for class variables.

    public EnemyHelper helper;
    public Transform helperTransform;

    bool foundPlayer;

    // Use this for initialization when scene loads.
    void Awake()
    {
        foundPlayer = false;
    }

    // Use this for initialization when game starts.
    void Start()
    {
    }

    // Update is called once per frame.
    void Update()
    {
        transform.position = helperTransform.position;
    }

    // Update is called by physics.
    void FixedUpdate()
    {
    }

    // When Collider is triggered by other.
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!foundPlayer)
            {
                foundPlayer = true;
                helper.AttackPlayer(other.transform);
            }
        }
    }

    // When object other leaves Collider.
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (foundPlayer)
            {
                foundPlayer = false;
                helper.PlayerGone();
            }
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}