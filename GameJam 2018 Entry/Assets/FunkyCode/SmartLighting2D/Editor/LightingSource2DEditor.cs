using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightingSource2D))]
public class LightingSource2DEditor : Editor {

	override public void OnInspectorGUI() {
		LightingSource2D script = target as LightingSource2D;
		
		script.color = EditorGUILayout.ColorField("Color", script.color);
		script.lightSize = EditorGUILayout.FloatField("Size", script.lightSize);
		script.textureSize = (LightingSource2D.TextureSize)EditorGUILayout.EnumPopup("Buffer Size", script.textureSize);
		script.lightSprite = (LightingSource2D.LightSprite)EditorGUILayout.EnumPopup("Light Sprite", script.lightSprite);

		if (script.lightSprite == LightingSource2D.LightSprite.Custom) {
			script.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", script.sprite, typeof(Sprite), true);
		}

		script.rotationEnabled = EditorGUILayout.Toggle("Enable Rotation", script.rotationEnabled);
	}
}
