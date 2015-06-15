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
        scroeText.text = "Score: " + score + " HP: " + playerHitPoints + " Krystite: " + playerKrystite + "   Arrow keys to move and rotate, Left CTRL to Mine, Left Alt to Fire.";
    }

    public void AddScore(int Points)
    {
        score += Points;

        UpdateScore();

        if (score > highScore)
            highScore = score;
    }

    public void PlayerHit(int Points)
    {
        playerHitPoints -= Points;
        UpdateScore();
    }

    public void PlayerCollected(int Amount)
    {
        playerKrystite += Amount;
        score += 10;
        UpdateScore();
    }

    public void PlayerUsed(int Amount)
    {
        playerKrystite -= Amount;
    }
}