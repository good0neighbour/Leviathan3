using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;
using System.Text;
using System.Xml.Linq;

[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    private CharacterData _charData = null;
    private CharacterData.Character[] _charArray = null;


    private void OnEnable()
    {
        _charData = (CharacterData)target;
        _charArray = _charData.GetCharacterList();
        if (_charArray.Length != (int)Characters.End)
        {
            CharacterData.Character[] newArray = new CharacterData.Character[(int)Characters.End];
            for (byte i = 0; i < (byte)Characters.End; ++i)
            {
                newArray[i] = _charArray[i];
            }
            _charArray = newArray;
            EditorUtility.SetDirty(_charData);
        }
    }


    public override void OnInspectorGUI()
    {
        for (byte i = 0; i < _charArray.Length; ++i)
        {
            Undo.RecordObject(_charData, "Character data moidified");

            BeginHorizontal();
            LabelField(_charArray[i].Name.ToString(), EditorStyles.boldLabel, GUILayout.MaxWidth(180.0f));
            if (_charArray[i].FullImage != null)
            {
                GUILayout.Box(_charArray[i].FullImage.texture, GUILayout.MaxWidth(64.0f), GUILayout.MaxHeight(64.0f), GUILayout.MinWidth(5.0f), GUILayout.MinHeight(5.0f));
            }
            if (_charArray[i].ButtonImage != null)
            {
                GUILayout.Box(_charArray[i].ButtonImage.texture, GUILayout.MaxWidth(64.0f), GUILayout.MaxHeight(64.0f), GUILayout.MinWidth(5.0f), GUILayout.MinHeight(5.0f));
            }
            if (_charArray[i].ProfileImage != null)
            {
                GUILayout.Box(_charArray[i].ProfileImage.texture, GUILayout.MaxWidth(64.0f), GUILayout.MaxHeight(64.0f), GUILayout.MinWidth(5.0f), GUILayout.MinHeight(5.0f));
            }
            if (_charArray[i].Sprite != null)
            {
                GUILayout.Box(_charArray[i].Sprite.texture, GUILayout.MaxWidth(64.0f), GUILayout.MaxHeight(64.0f), GUILayout.MinWidth(5.0f), GUILayout.MinHeight(5.0f));
            }
            if (GUILayout.Button("Update Images", GUILayout.MaxWidth(180.0f)))
            {
                _charArray[i].FullImage = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/Characters/{_charArray[i].Name.ToString()}FullImage.png");
                _charArray[i].ButtonImage = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/Characters/{_charArray[i].Name.ToString()}ButtonImage.png");
                _charArray[i].ProfileImage = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/Characters/{_charArray[i].Name.ToString()}ProfileImage.png");
                _charArray[i].Sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/Characters/{_charArray[i].Name.ToString()}Sprite.png");
            }
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Base Health", GUILayout.MaxWidth(180.0f));
            _charArray[i].BaseHealth = IntField(_charArray[i].BaseHealth);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Base Armor", GUILayout.MaxWidth(180.0f));
            _charArray[i].BaseArmor = (ushort)IntField(_charArray[i].BaseArmor);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Base Damage", GUILayout.MaxWidth(180.0f));
            _charArray[i].BaseDamage = (ushort)IntField(_charArray[i].BaseDamage);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Base Range Rate", GUILayout.MaxWidth(180.0f));
            _charArray[i].BaseRangeRate = (byte)IntField(_charArray[i].BaseRangeRate);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Health Increase", GUILayout.MaxWidth(180.0f));
            _charArray[i].HealthIncrease = (byte)IntField(_charArray[i].HealthIncrease);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Damage Increase", GUILayout.MaxWidth(180.0f));
            _charArray[i].DamageIncrease = (byte)IntField(_charArray[i].DamageIncrease);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Range Rate Increase", GUILayout.MaxWidth(180.0f));
            _charArray[i].RangeRateIncrease = (byte)IntField(_charArray[i].RangeRateIncrease);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Lv for Armor Increase", GUILayout.MaxWidth(180.0f));
            _charArray[i].LvForArmorIncrease = (byte)IntField(_charArray[i].LvForArmorIncrease);
            EndHorizontal();

            LabelField("  Game Play Data", EditorStyles.boldLabel);

            BeginHorizontal();
            LabelField("  Status", GUILayout.MaxWidth(180.0f));
            _charArray[i].Status = (CharacterStatus)EnumPopup(_charArray[i].Status, GUILayout.MaxWidth(180.0f));
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Type", GUILayout.MaxWidth(180.0f));
            _charArray[i].Type = (CharacterTypes)EnumPopup(_charArray[i].Type, GUILayout.MaxWidth(180.0f));
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Weapon", GUILayout.MaxWidth(180.0f));
            _charArray[i].Weapon = (CharacterWeapons)EnumPopup(_charArray[i].Weapon, GUILayout.MaxWidth(180.0f));
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Level", GUILayout.MaxWidth(180.0f));
            _charArray[i].Level = (byte)IntField(_charArray[i].Level, GUILayout.MaxWidth(180.0f));
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Recent Dialogue", GUILayout.MaxWidth(180.0f));
            _charArray[i].RecentDialogue = (DialogueScript)ObjectField(_charArray[i].RecentDialogue, typeof(DialogueScript), false, GUILayout.MaxWidth(180.0f));
            EndHorizontal();

            Space(20.0f);
        }
    }
}