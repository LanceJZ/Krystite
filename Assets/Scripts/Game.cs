using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Use this for class variables.

    public Camera mainCamera;
    public GameObject player;
    public GameObject arrowRock;
    public GameObject arrowBoss;
    public GameObject arrowMiner;

    private bool gameOver;
    private bool gameStart;
    private bool spawnPlayer;

    // Use this for initialization when scene loads.
    private void Awake()
    {
        gameOver = false;
        spawnPlayer = true;
    }

    // Use this for initialization when game starts.
    private void Start()
    {
        Instantiate(player);
        Instantiate(arrowRock);
        Instantiate(arrowBoss);
        Instantiate(arrowMiner);
    }

    // Update is called once per frame.
    private void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
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

    public void PlayerDestroyed()
    {
        gameOver = true;
        Text scroeText = gameObject.GetComponent<Text>();
        scroeText.text = scroeText.text + " Game Over 'R' to restart game.";
    }
}