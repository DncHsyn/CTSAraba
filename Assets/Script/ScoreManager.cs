using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text killText;

    [Header("Score Settings")]
    public int pointsPerKill = 100;

    private int score = 0;
    private int kills = 0;

    private void Start()
    {
        UpdateUI();
    }

    public void AddKill()
    {
        kills++;

        score += pointsPerKill;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }

        if (killText != null)
        {
            killText.text = "KILLS: " + kills;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetKills()
    {
        return kills;
    }
}
