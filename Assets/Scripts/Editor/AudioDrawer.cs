using Audio;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// Custom Property Drawer to draw audio clip selection dropdown GUI in the editor
    /// </summary>

    [CustomPropertyDrawer(typeof(AudioAttribute))]
    public class AudioDrawer : PropertyDrawer
    {
        private string[] _choices ;
        private Rect _toggle;
        private bool _isDrawable;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.color = Color.white;
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            AudioAttribute audio = attribute as AudioAttribute;
            bool newState = true;
            bool oldState = GUI.enabled;
            if (audio is { IsCustomAudio: true })
            {
                _isDrawable = audio.IsCustomAudio;
            }
            else
            {
                if (audio != null)
                {
                    SerializedProperty sProperty = property.serializedObject.FindProperty(audio.target);

           

                    if (sProperty == null) Debug.LogWarning("[AudioDrawer] Invalid Property Name for Attribute.", property.serializedObject.targetObject);
                    else newState = sProperty.boolValue != audio.IsCustomAudio;
                    GUI.enabled = newState;

                    if (sProperty != null)
                    {
                        _isDrawable = sProperty.boolValue;
                    }
                }
            }

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                if (audio != null)
                {
                    switch (audio.audioType)
                    {
                        case AudioType.BGM:
                            if (_isDrawable)
                            {
                                DisPlayBGMNames(position, property, label);
                            }
                            else
                            {
                                property.intValue = -1;
                            }

                            break;

                        case AudioType.SFX:

                            if (_isDrawable)
                            {
                                DisPlaySfxNames(position, property, label);
                            }
                            else
                            {
                                property.intValue = -1;
                            }

                            break;
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
            EditorGUI.EndProperty();

            GUI.enabled = oldState;
        }


        private void DisPlaySfxNames(Rect position, SerializedProperty property, GUIContent label)
        {

            _choices = AudioAssetCollection.Instance.GetSFXNames();
            if (_choices.Length > 0)
            {

                property.intValue = EditorGUI.Popup(position,label.text, property.intValue, _choices);

            }
            else
            {
                GUI.color = Color.red;
                GUI.Label(position, "Cannot Find SFX Audios, Please check the AudioAssetCollection");
                property.intValue = -1;
            }
        }
        void DisPlayBGMNames(Rect position, SerializedProperty property, GUIContent label)
        {

            _choices = AudioAssetCollection.Instance.GetBGMNames();
            if (_choices.Length > 0)
            {

                property.intValue = EditorGUI.Popup(position, label.text,property.intValue, _choices);

            }
            else
            {
                GUI.color = Color.red;
                GUI.Label(position, "Cannot Find BGM Audios, Please check the AudioAssetCollection");
                property.intValue = -1;
            }
        }
  
    }
}