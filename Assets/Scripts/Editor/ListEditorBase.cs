#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

public abstract class ListEditorBase : Editor
{
    protected byte Current = 0;
    private byte SwitchFrom = 0;
    private byte SwitchTo = 0;


    protected void ListEditor<T, S>(T target, List<S> list, GameDelegate setAction, string undoMessage)
        where T : Object where S : struct
    {
        if (GUILayout.Button("Add an Action to end", GUILayout.MinHeight(30.0f)))
        {
            Undo.RecordObject(target, $"{target.name}: {undoMessage} added");
            list.Add(new S());
            Current = (byte)(list.Count - 1);
            setAction.Invoke();
        }

        Space(20.0f);

        BeginHorizontal();
        LabelField("Index", GUILayout.MaxWidth(40.0f));
        Current = (byte)IntField(Current, GUILayout.MaxWidth(30.0f));
        if (Current <= list.Count && GUILayout.Button("Add here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(target, $"{target.name}: {undoMessage} added");
            list.Insert(Current, new S());
            Current = (byte)(list.Count - 1);
            setAction.Invoke();
        }
        if (Current < list.Count && GUILayout.Button("Delete here", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(target, $"{target.name}: {undoMessage} deleted");
            list.RemoveAt(Current);
            Current = (byte)(list.Count - 1);
            setAction.Invoke();
        }

        Space(20.0f);

        LabelField("Move From", GUILayout.MaxWidth(70.0f));
        SwitchFrom = (byte)IntField(SwitchFrom, GUILayout.MaxWidth(30.0f));
        LabelField("To", GUILayout.MaxWidth(20.0f));
        SwitchTo = (byte)IntField(SwitchTo, GUILayout.MaxWidth(30.0f));
        if (SwitchFrom < list.Count && SwitchTo < list.Count && GUILayout.Button("Move", GUILayout.MaxWidth(100.0f)))
        {
            Undo.RecordObject(target, $"{target.name}: {undoMessage} moved");
            S temp = list[SwitchFrom];
            list.RemoveAt(SwitchFrom);
            list.Insert(SwitchTo, temp);
            setAction.Invoke();
        }
        EndHorizontal();
    }
}
#endif