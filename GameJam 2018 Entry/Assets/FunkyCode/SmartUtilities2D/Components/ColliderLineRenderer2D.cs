using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderLineRenderer2D : MonoBehaviour {
	public Color color = Color.white;
	public float lineWidth = 1;

	private bool connectedLine = true; // For Edge Collider

	private Polygon2D polygon = null;
	private Mesh mesh = null;
	private float lineWidthSet = 1;
	private Material material;

	const float lineOffset = -0.01f;

	void Start () {
		if (GetComponent<EdgeCollider2D>() != null) {
			connectedLine = false;
		}
		
		Max2D.Check();
		material = new Material(Max2D.lineMaterial);

		GenerateMesh();
		Draw();
	}
	
	public void Update() {
		if (lineWidth != lineWidthSet) {
			if (lineWidth < 0.01f) {
				lineWidth = 0.01f;
			}
			GenerateMesh();
		}

		Draw();
	}

		public Polygon2D GetPolygon() {
		if (polygon == null) {
			polygon = Polygon2DList.CreateFromGameObject (gameObject)[0];
		}
		return(polygon);
	}

	public void GenerateMesh() {
		lineWidthSet = lineWidth;

		mesh = Max2DMesh.GeneratePolygon2DMeshNew(transform, GetPolygon(), lineOffset, lineWidth, connectedLine);
	}

	public void Draw() {
		//material.color = color;
		material.SetColor ("_Emission", color);

		Max2DMesh.Draw(mesh, transform, material);
	}
}