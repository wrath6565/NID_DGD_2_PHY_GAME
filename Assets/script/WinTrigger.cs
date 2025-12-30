using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<WinUI>().ShowWin();
        }
    }
}
