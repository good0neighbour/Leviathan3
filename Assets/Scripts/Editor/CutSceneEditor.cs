using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CutScene))]
public class CutSceneEditor : ListEditorBase
{
    private List<CutScene.CutSceneInfo> _actions = null;
    private CutScene _cutScene = null;
    private string _status = null;


    private void OnEnable()
    {
        _cutScene = (CutScene)target;
        _actions = _cutScene.GetActionsForEditor();
        Current = (byte)(_actions.Count - 1);
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current Language", GUILayout.MaxWidth(110.0f));
        EditorGUI.BeginChangeCheck();
        _cutScene.CurrentLanguage = (LanguageTypes)EditorGUILayout.EnumPopup(_cutScene.CurrentLanguage, GUILayout.MaxWidth(100.0f));
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_cutScene);
            _status = null;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button($"Create {_cutScene.CurrentLanguage.ToString()} Json", GUILayout.MaxWidth(210.0f)))
        {
            Language.LanguageJson lanJson = new Language.LanguageJson();
            List<string> text = new List<string>();
            foreach (CutScene.CutSceneInfo item in _actions)
            {
                text.Add(item.Text);
            }
            lanJson.Text = text.ToArray();
            File.WriteAllText($"{Application.dataPath}/Resources/Languages/{_cutScene.name}_{_cutScene.CurrentLanguage.ToString()}.Json", JsonUtility.ToJson(lanJson, true));
            AssetDatabase.Refresh();
            _status = $"Saved \"Resources/Languages/{_cutScene.name}_{_cutScene.CurrentLanguage.ToString()}.Json\"";
        }

        EditorGUILayout.LabelField(_status);

        EditorGUILayout.Space(30.0f);

        for (byte i = 0; i < _actions.Count; ++i)
        {
            CutScene.CutSceneInfo element = _actions[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Index {i.ToString()}", GUILayout.MaxWidth(120.0f));
            EditorGUILayout.LabelField("Duration", GUILayout.MaxWidth(55.0f));
            EditorGUI.BeginChangeCheck();
            element.Duration = EditorGUILayout.FloatField(element.Duration, GUILayout.MaxWidth(50.0f));
            EditorGUILayout.Space(10.0f, false);
            EditorGUILayout.LabelField("Image", GUILayout.MaxWidth(40.0f));
            element.Image = (Sprite)EditorGUILayout.ObjectField(element.Image, typeof(Sprite), false, GUILayout.MaxWidth(120.0f));
            EditorGUILayout.Space(10.0f, false);
            EditorGUILayout.LabelField("Audio", GUILayout.MaxWidth(35.0f));
            element.Audio = (AudioClip)EditorGUILayout.ObjectField(element.Audio, typeof(AudioClip), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (element.Image != null)
            {
                GUILayout.Box(element.Image.texture, GUILayout.MaxWidth(120.0f), GUILayout.MaxHeight(60.0f), GUILayout.MinWidth(10.0f), GUILayout.MinHeight(5.0f));
                element.Text = EditorGUILayout.TextField(element.Text, GUILayout.MinHeight(20.0f), GUILayout.MaxHeight(60.0f));
            }
            else
            {
                element.Text = EditorGUILayout.TextField(element.Text, GUILayout.MinHeight(20.0f));
            }
            EditorGUILayout.EndHorizontal();


            if (EditorGUI.EndChangeCheck())
            {
                _actions[i] = element;
                _cutScene.SetActions(_actions.ToArray());
                EditorUtility.SetDirty(_cutScene);
                _status = null;
            }

            EditorGUILayout.Space(30.0f);
        }

        EditorGUILayout.LabelField("Next Scene");
        EditorGUI.BeginChangeCheck();
        _cutScene.NextScene = EditorGUILayout.TextField(_cutScene.NextScene, GUILayout.MinHeight(20.0f));
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_cutScene);
            _status = null;
        }

        EditorGUILayout.Space(30.0f);

        ListEditor(_cutScene, _actions, () => _cutScene.SetActions(_actions.ToArray()), "CutScene");
    }
}
