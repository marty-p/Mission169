using UnityEngine;
using UnityEditor;
using Mission169;
using SlugLib;

[CustomEditor(typeof(EncounterController))]
public class EncounterControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EncounterController myScript = (EncounterController)target;
        if (GUILayout.Button("Snap camera to event"))
        {
            Camera.main.transform.position = myScript.transform.position;
        }
        if (GUILayout.Button("Snap encounter to camera"))
        {
            myScript.transform.position = Camera.main.transform.position;
            CameraUtils.DrawCameraEdgesAt(Selection.activeTransform.position);
        }
    }
}
