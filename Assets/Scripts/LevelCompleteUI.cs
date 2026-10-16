using UnityEngine;

public class LevelCompleteUI : MonoBehaviour
{
    private static bool completed;
    public static void Show() => completed = true;

    private void OnGUI()
    {
        if (!completed) return;
        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 34,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
        GUI.Label(new Rect(0, Screen.height / 2f - 40, Screen.width, 80), "УРОВЕНЬ ПРОЙДЕН!", style);
        if (GUI.Button(new Rect(Screen.width / 2f - 70, Screen.height / 2f + 45, 140, 38), "Заново"))
        {
            completed = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
