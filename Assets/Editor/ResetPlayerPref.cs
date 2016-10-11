using UnityEngine;
using UnityEditor;

public class AddMenu : EditorWindow {

    [MenuItem("Edit/Reset PlayerPrefs")]

    public static void DeletePlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }
}
