using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraTrigger))]
public class CameraTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraTrigger cameraTrigger = (CameraTrigger)target;

        if (GUILayout.Button("Preview Camera Trigger"))
        {
            if (cameraTrigger != null)
            {
                cameraTrigger.hallwayCamera.transform.SetParent(cameraTrigger.cameraTransform);
                cameraTrigger.hallwayCamera.transform.localPosition = Vector3.zero;
                cameraTrigger.hallwayCamera.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
