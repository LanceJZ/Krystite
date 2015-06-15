using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Use this for initialization
    public bool turnedOn;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Render(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerTransform != null)
            transform.position = playerTransform.position;
        else
            Render(false);
    }

    public void PointDirection(float Angle) // Point Arrow to direction of rock.
    {
        transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
    }

    public void Render(bool render)
    {
        GetComponentInChildren<SpriteRenderer>().enabled = render;
        turnedOn = render;
    }
}