using UnityEngine;

public class WinUI : MonoBehaviour
{
    public static WinUI Instance;
    public GameObject winPanel;

    void Awake()
    {
        Instance = this;
        winPanel.SetActive(false);
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
    }
}
