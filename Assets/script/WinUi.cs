using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinUI : MonoBehaviour
{
    public GameObject winPanel;

    [Header("Timer Texts")]
    public TextMeshProUGUI liveTimerText;   // top HUD
    public TextMeshProUGUI timeText;        // win screen
    public TextMeshProUGUI playerNameText;

    private float timer;
    private bool timerRunning = false;

    void Awake()
    {
        winPanel.SetActive(false);
        timer = 0f;
    }

    void Update()
    {
        if (!timerRunning)
            return;

        timer += Time.deltaTime;

        UpdateLiveTimer();
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    void UpdateLiveTimer()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        liveTimerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ShowWin()
    {
        timerRunning = false;
        winPanel.SetActive(true);

        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        playerNameText.text = "Player: " + playerName;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timeText.text = $"Time: {minutes:00}:{seconds:00}";

        Time.timeScale = 0f;

        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
