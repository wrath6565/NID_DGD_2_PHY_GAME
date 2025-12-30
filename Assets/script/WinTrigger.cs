using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WinGame();
        }
    }

    void WinGame()
    {
        Time.timeScale = 0f;
        WinUI.Instance.ShowWin();
    }
}
