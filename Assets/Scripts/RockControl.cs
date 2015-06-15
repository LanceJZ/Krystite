using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockControl : MonoBehaviour
{
    // Use this for class variables.

    public GameObject rock;

    private List<Transform> rockLocationList;

    private int wave;

    private int rockCount;
    private int rocksHit;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        rockLocationList = new List<Transform>();
    }

    // Use this for initialization when game starts.
    private void Start()
    {
        SpawnNewWave();
    }

    // Update is called once per frame.
    private void Update()
    {
    }

    // Update is called by physics.
    private void FixedUpdate()
    {
    }

    // When Collider is triggered by other.
    private void OnTriggerEnter(Collider other)
    {
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }

    private void SpawnNewWave()
    {
        for (int i = 0; i < 25 + wave; i++)
        {
            Rock thisRock = Instantiate(rock).GetComponent<Rock>();
            thisRock.Setup(new Vector3(20, 20, 20));
            thisRock.SetTumble(0.5f, 0.25f);
            rockLocationList.Add(thisRock.transform);
            rockCount++;
            rocksHit = 0;
        }
    }

    private void ClearEmptyRocks()
    {
        for (int i = 0; i < rockLocationList.Count - 2; i++)
        {
            if (rockLocationList[i] == null)
                rockLocationList.RemoveAt(i);
        }
    }

    public float FindClosestTarget(Vector3 Seeker)
    {
        int closestRock = 0;
        bool rockFound = false;

        for (int i = 0; i < rockLocationList.Count - 2; i++)
        {
            if (rockLocationList[i + 1] != null && rockLocationList[closestRock] != null)
            {
                rockFound = true;

                if (Vector3.Distance(rockLocationList[i + 1].position, Seeker) < Vector3.Distance(rockLocationList[closestRock].position, Seeker))
                {
                    closestRock = i + 1;
                }
            }
        }

        Vector3 rockHeading2D = Vector3.zero;

        if (rockFound)
        {
            Vector3 rockHeading = rockLocationList[closestRock].position - Seeker;
            rockHeading2D = new Vector2(rockHeading.x, rockHeading.y).normalized;
        }
        else
            ClearEmptyRocks();

        return Mathf.Atan2(rockHeading2D.y, rockHeading2D.x);
    }

    public void RockDestroyed()
    {
        rockCount--;
        rocksHit++;
        ClearEmptyRocks();

        if (rockCount < rocksHit * 2)
        {
            wave++;
            SpawnNewWave();
        }
    }
}