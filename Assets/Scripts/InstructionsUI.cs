using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = 16, normal = { textColor = Color.white } };
        GUI.Label(new Rect(18, 15, 560, 28), "A/D или ←/→ — движение     SPACE — прыжок     E — сонар", style);
    }
}
