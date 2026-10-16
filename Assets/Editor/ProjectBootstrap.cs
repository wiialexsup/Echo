using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class ProjectBootstrap
{
    static ProjectBootstrap()
    {
        EditorApplication.delayCall += EnsureDemoScene;
    }

    private static void EnsureDemoScene()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (System.IO.File.Exists("Assets/Level1.unity")) return;

        CreateDemoScene.Create(false);
        Debug.Log("Echo Platformer: создана демонстрационная сцена Assets/Level1.unity.");
    }
}
