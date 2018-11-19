using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightingCollider2D))]
public class LightingCollider2DEditor : Editor {

	override public void OnInspectorGUI() {
		LightingCollider2D script = target as LightingCollider2D;
		
		script.dayHeight = EditorGUILayout.Toggle("Day Height", script.dayHeight);
		if (script.dayHeight)  {
			script.height = EditorGUILayout.FloatField("Height", script.height);
		}
		script.ambientOcclusion = EditorGUILayout.Toggle("Ambient Occlusion", script.ambientOcclusion);
	}
}
