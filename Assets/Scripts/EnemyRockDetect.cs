using UnityEngine;
using System.Collections;

public class EnemyRockDetect : MonoBehaviour
{
    // Use this for initialization

    public GameObject KrystiteCollider;
    public EnemyHelper helper;
    public Transform helperTransform;
    public bool foundRock;

    void Awake()
    {
        foundRock = false;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = helperTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rock")
        {
            if (!foundRock)
            {
                foundRock = true;
                helper.MineRock(other.transform);
                GameObject krystiteDetect = Instantiate(KrystiteCollider);
                krystiteDetect.GetComponent<EnemyKrystiteDetect>().Setup(helper.GetComponent<EnemyHelper>(), other.transform);
                helper.SetKrystite(krystiteDetect.GetComponent<EnemyKrystiteDetect>());
            }
        }
    }

    // When object other leaves Collider.
    private void OnTriggerExit(Collider other)
    {
    }

    void OnTriggerStay(Collider other)
    {
    }

    public void Reset()
    {
        foundRock = false;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}