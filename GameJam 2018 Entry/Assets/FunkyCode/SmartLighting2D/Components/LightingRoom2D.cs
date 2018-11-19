using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingRoom2D : MonoBehaviour {
	public Color color = Color.black;

	private Mesh mesh;

	static public List<LightingRoom2D> GetList() {
		List<LightingRoom2D> result = new List<LightingRoom2D>();
		foreach (LightingRoom2D buffer in Object.FindObjectsOfType(typeof(LightingRoom2D))) {
			result.Add(buffer);
		}
		return(result);
	}

	public Mesh GetMesh() {
		if (mesh == null) {
			mesh = PolygonTriangulator2D.Triangulate (Polygon2DList.CreateFromGameObject (gameObject)[0], Vector2.zero, Vector2.zero, PolygonTriangulator2D.Triangulation.Advanced);
		}	
		return(mesh);
	}
}
