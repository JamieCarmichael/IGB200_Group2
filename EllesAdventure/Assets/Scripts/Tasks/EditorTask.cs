using UnityEngine;
using UnityEditor;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>
/// 

[CustomEditor(typeof(Task))]
public class EditorTask : Editor
{
    SubTask.SubtaskType newSubTask = SubTask.SubtaskType.TalkToNPC;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Task myScript = (Task)target;


        newSubTask = (SubTask.SubtaskType)EditorGUILayout.EnumPopup("Subtask Type", newSubTask);
        if (GUILayout.Button("Add Subtask"))
        {
            myScript.AddSubtask(newSubTask);
        }
    }
}