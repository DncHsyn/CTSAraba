using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 180f;

    public TMP_Text timerText;
    public TMP_Text resultText;

    public CarHealth playerHealth;

    private float remainingTime;
    private bool gameEnded = false;

    void Start()
    {
        remainingTime = gameDuration;

        resultText.gameObject.SetActive(false);

        UpdateTimerText();
    }

    void Update()
    {
        if (gameEnded)
        {
            return;
        }

        // Player öldüyse oyunu kaybet
        if (playerHealth == null ||
            playerHealth.currentHealth <= 0)
        {
            EndGame(false);
            return;
        }

        // Süreyi azalt
        remainingTime -= Time.deltaTime;

        // Süre sıfırın altına düşmesin
        remainingTime = Mathf.Max(remainingTime, 0f);

        UpdateTimerText();

        // Süre bittiyse oyunu kazan
        if (remainingTime <= 0f)
        {
            EndGame(true);
        }
    }

    void UpdateTimerText()
    {
        int totalSeconds = Mathf.CeilToInt(remainingTime);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text =
            minutes.ToString("00") + ":" +
            seconds.ToString("00");
    }

    void EndGame(bool playerWon)
    {
        gameEnded = true;

        resultText.gameObject.SetActive(true);

        if (playerWon)
        {
            resultText.text = "KAZANDINIZ!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "KAYBETTİNİZ!";
            resultText.color = Color.red;
        }

        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}