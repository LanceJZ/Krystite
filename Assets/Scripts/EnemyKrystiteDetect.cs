using UnityEngine;
using System.Collections;

public class EnemyKrystiteDetect : MonoBehaviour
{
    // Use this for initialization

    EnemyHelper helper;
    Transform rock;
    Transform krystite;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (rock != null)
        {
            transform.position = rock.position;
        }
        else if (krystite == null)
        {
            GameObject.Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Krystite")
        {
            helper.SendToKrystite(other.transform);
            krystite = other.transform;
        }
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
        if (other == helper)
            helper.outOfRange = true;
    }

    void OnTriggerStay(Collider other)
    {
    }

    public void Setup(EnemyHelper Helper, Transform Rock)
    {
        helper = Helper;
        rock = Rock;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}