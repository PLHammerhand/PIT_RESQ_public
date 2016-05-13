using UnityEditor;
using System.Collections;
using UnityEngine;

//[CustomEditor(typeof(Waves))]
//[CanEditMultipleObjects]
//public class WaveEditor : Editor
//{
//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();

//		Waves waves = target as Waves;

//		SerializedProperty enemiesArray = serializedObject.FindProperty("enemies");

//		EditorGUILayout.PropertyField(enemiesArray, true);

//		serializedObject.ApplyModifiedProperties();
//	}
//}

//[CustomPropertyDrawer(typeof(Wave))]
//[CanEditMultipleObjects]
//public class WaveDrawer : PropertyDrawer
//{
//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//	{
//		label = EditorGUI.BeginProperty(position, label, property);
//		Rect contentPosition = EditorGUI.PrefixLabel(position, label);
//		EditorGUI.indentLevel = 0;
//		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("enemies"), GUIContent.none);
//		EditorGUI.EndProperty();
//	}
//}

[CustomPropertyDrawer(typeof(EnemyCount))]
[CanEditMultipleObjects]
public class EnemyCountDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);
		Rect contentPosition = EditorGUI.PrefixLabel(position, label);
		contentPosition.width *= 0.75f;
		EditorGUI.indentLevel = 0;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("enemy"), GUIContent.none);
		contentPosition.x += contentPosition.width + 5;
		contentPosition.width /= 4f;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("count"), GUIContent.none);
		EditorGUI.EndProperty();
	}
}


