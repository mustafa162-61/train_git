using System.IO;
using UnityEditor;
using UnityEngine;

public class EkranGoruntusuEditor : EditorWindow
{
    private static string directory = "Screenshots/";
    private static string latestScreenshotPath = "";
    private bool initDone = false;

    private GUIStyle BigText;

    void InitStyles()
    {
        initDone = true;
        BigText = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold
        };
    }

    private void OnGUI()
    {
        if (!initDone)
        {
            InitStyles();
        }

        GUILayout.Label("Ekran Görüntüsü", BigText);
        if (GUILayout.Button("Ekran Görüntüsü AL"))
        {
            TakeScreenshot();
        }
        GUILayout.Label("Çözünürlük: " + GetResolution());

        if (GUILayout.Button("Klasöre Git"))
        {
            ShowFolder();
        }
        GUILayout.Label("Dosya Yolu: " + directory);
    }

    [MenuItem("4usGAME/Ekran Görüntüsü/Pencere Modu")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EkranGoruntusuEditor));
    }

   
   // [MenuItem("4usGAME/Ekran Görüntüsü/Klasöre Git")]
    private static void ShowFolder()
    {
        if (File.Exists(latestScreenshotPath))
        {
            EditorUtility.RevealInFinder(latestScreenshotPath);
            return;
        }
        Directory.CreateDirectory(directory);
        EditorUtility.RevealInFinder(directory);
    }
   

   // [MenuItem("4usGAME/Ekran Görüntüsü/Ekran Görüntüsü AL")]
    private static void TakeScreenshot()
    {
        Directory.CreateDirectory(directory);
        var currentTime = System.DateTime.Now;
        var filename = currentTime.ToString().Replace('/', '-').Replace(':', '_') + ".png";
        var path = directory + filename;
        ScreenCapture.CaptureScreenshot(path);
        latestScreenshotPath = path;
        Debug.Log($"Baþarýlý... Dosya Yolu: <b>{path}</b> Çözünürlük: <b>{GetResolution()}</b>");
    }

    private static string GetResolution()
    {
        Vector2 size = UnityEditor.Handles.GetMainGameViewSize();
        Vector2Int sizeInt = new Vector2Int((int)size.x, (int)size.y);
        return $"{sizeInt.x.ToString()}x{sizeInt.y.ToString()}";
    }

}