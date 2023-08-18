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

        GUILayout.Label("Ekran G�r�nt�s�", BigText);
        if (GUILayout.Button("Ekran G�r�nt�s� AL"))
        {
            TakeScreenshot();
        }
        GUILayout.Label("��z�n�rl�k: " + GetResolution());

        if (GUILayout.Button("Klas�re Git"))
        {
            ShowFolder();
        }
        GUILayout.Label("Dosya Yolu: " + directory);
    }

    [MenuItem("4usGAME/Ekran G�r�nt�s�/Pencere Modu")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EkranGoruntusuEditor));
    }

   
   // [MenuItem("4usGAME/Ekran G�r�nt�s�/Klas�re Git")]
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
   

   // [MenuItem("4usGAME/Ekran G�r�nt�s�/Ekran G�r�nt�s� AL")]
    private static void TakeScreenshot()
    {
        Directory.CreateDirectory(directory);
        var currentTime = System.DateTime.Now;
        var filename = currentTime.ToString().Replace('/', '-').Replace(':', '_') + ".png";
        var path = directory + filename;
        ScreenCapture.CaptureScreenshot(path);
        latestScreenshotPath = path;
        Debug.Log($"Ba�ar�l�... Dosya Yolu: <b>{path}</b> ��z�n�rl�k: <b>{GetResolution()}</b>");
    }

    private static string GetResolution()
    {
        Vector2 size = UnityEditor.Handles.GetMainGameViewSize();
        Vector2Int sizeInt = new Vector2Int((int)size.x, (int)size.y);
        return $"{sizeInt.x.ToString()}x{sizeInt.y.ToString()}";
    }

}