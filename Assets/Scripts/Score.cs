using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Use this for class variables.

    private int score;
    private int highScore;
    private int nextNewShip;
    private int playerHitPoints;
    private int playerKrystite;

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

    private void OnTriggerStay(Collider other)
    {
    }

    private void UpdateScore()
    {
        Text scroeText = gameObject.GetComponent<Text>();
        scroeText.text = " Arrow keys to move and rotate, Left CTRL to Mine, Left Alt to Fire. DownArrow to heal. \n" +
            "Score: " + score + " HP: " + playerHitPoints + " Krystite: " + playerKrystite;
    }

    public void AddScore(int Points)
    {
        score += Points;

        UpdateScore();

        if (score > highScore)
            highScore = score;
    }

    public void PlayerCollectedKrystite()
    {
        score += 10;
        UpdateScore();
    }

    public void PlayerHasKrystite(int Amount)
    {
        playerKrystite = Amount;
        UpdateScore();
    }

    public void PlayerHasHealth(int Amount)
    {
        playerHitPoints = Amount;
        UpdateScore();
    }
}