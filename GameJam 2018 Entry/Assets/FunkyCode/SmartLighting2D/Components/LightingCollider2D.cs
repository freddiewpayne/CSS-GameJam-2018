using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingCollider2D : MonoBehaviour {
	public bool dayHeight = false;
	public float height = 1f;
	public bool ambientOcclusion = true;

	private Polygon2D polygon;
	private Mesh mesh;
	private float meshDistance = 0f;

	public bool moved = false;
	public Vector2 movedPosition = Vector2.zero;

	static public List<LightingCollider2D> GetList() {
		List<LightingCollider2D> result = new List<LightingCollider2D>();
		foreach (LightingCollider2D buffer in Object.FindObjectsOfType(typeof(LightingCollider2D))) {
			result.Add(buffer);
		}
		return(result);
	}

	void Start() {
		mesh = GetMesh();
		
		// Optimization
		foreach (Vector2D id in GetPolygon().pointsList) {
			meshDistance = Mathf.Max(meshDistance, Vector2.Distance(id.ToVector2(), Vector2.zero));
		}
	}

	public void Update() {
		Vector2 position = transform.transform.position;
		if (movedPosition != position) {
			movedPosition = position;
			moved = true;
		} else {
			moved = false;
		}

		if (moved) {
			foreach (LightingSource2D id in LightingSource2D.GetList()) {
				if (Vector2.Distance (id.transform.position, position) < meshDistance + id.lightSize) {
					id.update = true;
					LightingBuffer2D.GetBuffer (id.GetTextureSize(), id).lightSource = id;
				}
			}
		}
	}

	public Mesh GetMesh() {
		if (mesh == null) {
			if (GetPolygon().pointsList.Count > 2) {
				mesh = PolygonTriangulator2D.Triangulate (GetPolygon(), Vector2.zero, Vector2.zero, PolygonTriangulator2D.Triangulation.Advanced);
			}
		}
		return(mesh);
	}

	public Polygon2D GetPolygon() {
		if (polygon == null) {
			polygon = Polygon2DList.CreateFromGameObject (gameObject)[0];
		}
		return(polygon);
	}

	public Polygon2D GenerateShadow(Polygon2D poly, float sunDirection) {
		Polygon2D convexHull = new Polygon2D ();
		foreach (Vector2D p in poly.pointsList) {
			Vector2D vA = p.Copy();
			vA.Push (sunDirection, height);
			
			convexHull.pointsList.Add (vA);
			convexHull.pointsList.Add (p);
		}

		convexHull.pointsList = Math2D.GetConvexHull (convexHull.pointsList);
		return(convexHull);
	}
}
