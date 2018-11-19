using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(LightingManager2D))]
public class SmartLighting2DManagerEditor : Editor {
	override public void OnInspectorGUI() {
		LightingManager2D script = target as LightingManager2D;
		script.enable = EditorGUILayout.Toggle("Enable", script.enable);
		script.darknessColor = EditorGUILayout.ColorField("Darkness Color", script.darknessColor);
		script.sunDirection = EditorGUILayout.FloatField("Sun Rotation", script.sunDirection);

		script.debug = EditorGUILayout.Toggle("Debug", script.debug);
	}
}
 