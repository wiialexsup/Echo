using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CreateDemoScene
{
    private const string GroundLayerName = "Ground";
    private const string RevealLayerName = "Revealable";
    private static Sprite squareSprite;

    [MenuItem("Echo Platformer/Create Demo Level")]
    public static void Create() => Create(true);

    public static void Create(bool showDialog)
    {
        squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/WhiteSquare.png");
        int groundLayer = EnsureLayer(GroundLayerName);
        int revealLayer = EnsureLayer(RevealLayerName);
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        Camera cam = new GameObject("Main Camera", typeof(Camera)).GetComponent<Camera>();
        cam.tag = "MainCamera";
        cam.orthographic = true;
        cam.orthographicSize = 4.8f;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.backgroundColor = new Color(0.015f, 0.025f, 0.06f);
        cam.clearFlags = CameraClearFlags.SolidColor;

        GameObject player = MakeBox("Player", new Vector2(-6.5f, -0.8f), new Vector2(0.8f, 1.15f), new Color(0.25f, 0.85f, 1f), 0, false);
        player.tag = "Player";
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        player.AddComponent<PlayerController>();
        player.AddComponent<AudioSource>();
        player.AddComponent<Sonar>();
        GameObject check = new GameObject("GroundCheck");
        check.transform.SetParent(player.transform);
        check.transform.localPosition = new Vector3(0, -0.53f, 0);
        SetObjectReference(player.GetComponent<PlayerController>(), "groundCheck", check.transform);
        SetLayerMask(player.GetComponent<PlayerController>(), "groundLayer", 1 << groundLayer);
        SetLayerMask(player.GetComponent<Sonar>(), "revealLayer", 1 << revealLayer);

        MakeBox("Ground_Start", new Vector2(-5.4f, -2.0f), new Vector2(5.8f, 0.8f), new Color(0.1f, 0.22f, 0.35f), groundLayer, false);
        MakeBox("Ground_Middle", new Vector2(1.3f, -2.0f), new Vector2(3.0f, 0.8f), new Color(0.1f, 0.22f, 0.35f), groundLayer, true);
        GameObject moving = MakeBox("MovingPlatform", new Vector2(0.0f, -0.1f), new Vector2(2.4f, 0.42f), new Color(0.15f, 0.75f, 0.9f), groundLayer, true);
        moving.AddComponent<MovingPlatform>();
        MakeBox("Ground_End", new Vector2(6.3f, -1.3f), new Vector2(4.8f, 0.8f), new Color(0.1f, 0.22f, 0.35f), groundLayer, true);

        GameObject exit = MakeBox("Exit", new Vector2(7.5f, -0.55f), new Vector2(0.75f, 1.1f), new Color(0.2f, 1f, 0.55f), revealLayer, true);
        exit.GetComponent<BoxCollider2D>().isTrigger = true;
        exit.AddComponent<LevelExit>();

        MakeBox("Hidden_Pillar_1", new Vector2(-2.5f, -0.8f), new Vector2(0.5f, 2.2f), new Color(0.55f, 0.4f, 0.9f), revealLayer, true);
        MakeBox("Hidden_Pillar_2", new Vector2(4.5f, -0.55f), new Vector2(0.5f, 1.5f), new Color(0.55f, 0.4f, 0.9f), revealLayer, true);
        GameObject instructions = new GameObject("Instructions");
        instructions.AddComponent<InstructionsUI>();
        instructions.AddComponent<LevelCompleteUI>();

        EditorSceneManager.SaveScene(scene, "Assets/Level1.unity");
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Level1.unity", true) };
        Selection.activeGameObject = player;
        if (showDialog) EditorUtility.DisplayDialog("Echo Platformer", "Готово! Сцена сохранена как Assets/Level1.unity. Нажми Play.", "OK");
    }

    private static GameObject MakeBox(string name, Vector2 position, Vector2 scale, Color color, int layer, bool reveal)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        obj.transform.localScale = new Vector3(scale.x, scale.y, 1);
        obj.layer = layer;
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = squareSprite;
        sr.color = color;
        sr.sortingOrder = reveal ? -1 : 0;
        BoxCollider2D col = obj.AddComponent<BoxCollider2D>();
        if (reveal) obj.AddComponent<SonarReveal>();
        if (layer == LayerMask.NameToLayer(GroundLayerName)) obj.layer = layer;
        return obj;
    }

    private static int EnsureLayer(string name)
    {
        for (int i = 0; i < 32; i++) if (LayerMask.LayerToName(i) == name) return i;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");
        for (int i = 8; i < layers.arraySize; i++)
        {
            SerializedProperty p = layers.GetArrayElementAtIndex(i);
            if (string.IsNullOrEmpty(p.stringValue)) { p.stringValue = name; tagManager.ApplyModifiedProperties(); return i; }
        }
        return 0;
    }

    private static void SetObjectReference(Object target, string property, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        so.FindProperty(property).objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetLayerMask(Object target, string property, int mask)
    {
        SerializedObject so = new SerializedObject(target);
        so.FindProperty(property).intValue = mask;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
