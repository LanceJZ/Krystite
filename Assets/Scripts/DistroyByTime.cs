using System.Collections;
using UnityEngine;

public class DistroyByTime : MonoBehaviour
{
    public float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}