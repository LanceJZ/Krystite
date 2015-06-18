using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Use this for initialization

    public GameObject helper;
    public GameObject bossBuilder;
    public float spawnDelay;
    public int wave;

    private float spawnTimer;

    private void Awake()
    {
        spawnDelay = 45;
    }

    private void Start()
    {
        SpawnBoulder();
    }

    // Update is called once per frame
    private void Update()
    {
        if (spawnTimer < Time.time)
        {
            int boss = 0;
            GameObject[] bossBuilders = GameObject.FindGameObjectsWithTag("BossBuilder");

            if (bossBuilders.Length > 1)
                boss = Random.Range(0, bossBuilders.Length);

            Transform spawnTransform = bossBuilders[boss].GetComponent<Transform>();
            Instantiate(helper, spawnTransform.position, spawnTransform.rotation);
            spawnTimer = Time.time + spawnDelay;
        }
    }

    private void SpawnBoulder()
    {
        Instantiate(bossBuilder);
    }

    public void BoulderDestroyed()
    {
        SpawnBoulder();
    }
}