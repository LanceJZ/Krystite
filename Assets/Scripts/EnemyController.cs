using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Use this for initialization

    public GameObject helper;
    public GameObject bossBuilder;
    public float spawnDelay;

    private float spawnTimer;

    private void Awake()
    {
        spawnDelay = 45;
    }

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(bossBuilder);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (spawnTimer < Time.time)
        {
            Instantiate(helper);
            spawnTimer = Time.time + spawnDelay;
        }
    }
}