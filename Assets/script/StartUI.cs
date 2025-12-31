using UnityEngine;
using TMPro;

public class StartUI : MonoBehaviour
{
    public GameObject startPanel;
    public TMP_InputField nameInputField;

    void Start()
    {
        Time.timeScale = 0f; // Pause game
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
            playerName = "Player";

        PlayerPrefs.SetString("PlayerName", playerName);

        startPanel.SetActive(false);
        Time.timeScale = 1f;

        FindObjectOfType<WinUI>().StartTimer();
    }

}
