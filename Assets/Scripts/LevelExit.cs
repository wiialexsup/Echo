using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private bool finished;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished || !other.CompareTag("Player")) return;
        finished = true;
        Debug.Log("Уровень пройден!");
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Отключаем управление персонажем
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.enabled = false;
        }
        LevelCompleteUI.Show();
    }
}
