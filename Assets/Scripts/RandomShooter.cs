using System.Collections;
using UnityEngine;

public class RandomShooter : MonoBehaviour
{
    // Use this for class variables.

    public bool aimed;

    private float shotTimer;
    private float shotWait;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        shotWait = 2;
        aimed = false;
    }

    // Use this for initialization when game starts.
    private void Start()
    {
        shotTimer = ResetShotTimer(shotWait);
    }

    // Update is called once per frame.
    private void Update()
    {
        if (Time.time > shotTimer && !aimed)
        {
            ShootRandom();
            shotTimer = ResetShotTimer(shotWait);
        }
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

    public float ResetShotTimer(float wait)
    {
        return Random.Range(wait * 0.5f, wait) + Time.time;
    }

    public void ShootRandom()
    {
        float angle = Random.Range(0, 6.28318530718f);
        GetComponent<EnemyShooter>().Shoot(angle);
    }
}