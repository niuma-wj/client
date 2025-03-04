using UnityEditor;
using UnityEngine;
 
public class AddMenu : EditorWindow {
    [MenuItem("Edit/(Custom) Reset PlayerPrefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}