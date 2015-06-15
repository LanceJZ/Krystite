using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothing = 5f;
    public Vector3 targetVelocity;
    private Transform target;
    private Rigidbody targetRigidbody;
    private Vector3 offset;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        Vector3 targetCamPos = transform.position;
        targetVelocity = Vector3.zero;

        if (targetRigidbody != null)
            targetVelocity = targetRigidbody.velocity * 0.33f;

        if (target != null)
        {
            targetCamPos = target.position + offset + targetVelocity;
        }

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    public void Setup(Transform Target, Rigidbody TargetRigidbody)
    {
        target = Target;
        targetRigidbody = TargetRigidbody;
        offset = transform.position - target.position;
    }
}