using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    // Use this for class variables.

    public AudioClip shotClip;
    public GameObject shot;
    public float angle;

    // Use this for initialization when scene loads.
    private void Awake()
    {
    }

    // Use this for initialization when game starts.
    private void Start()
    {
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

    public void Shoot(float Angle)
    {
        angle = Angle;
        GameObject thisShotObj = Instantiate(shot);
        thisShotObj.GetComponentInChildren<AudioSource>().clip = shotClip;
        thisShotObj.GetComponentInChildren<AudioSource>().loop = false;
        thisShotObj.GetComponentInChildren<AudioSource>().Play();
        thisShotObj.transform.GetChild(0).tag = "EnemyShot";
        Vector3 velocity = Vector3.zero;
        float sinRot = Mathf.Sin(Angle);
        float cosRot = Mathf.Cos(Angle);
        velocity.x = cosRot * 5;
        velocity.y = sinRot * 5;
        thisShotObj.GetComponentInChildren<Shot>().SetUp(transform.position, velocity);
    }
}