using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class LightingMainBuffer2D : MonoBehaviour {
	static private LightingMainBuffer2D instance;

	private RenderTexture renderTexture;

	public Material material; // Static
	public Camera bufferCamera; // Static
	public Color darknessColor; // Static

	public int screenWidth = 640;
	public int screenHeight = 480;

	const float uv0 = 1.0f / 32f;
	const float uv1 = 1f - uv0;
	const float pi2 = Mathf.PI / 2;

	const float cameraOffset = 40f;

	static public LightingMainBuffer2D Get() {
		if (instance != null) {
			return(instance);
		}

		foreach(LightingMainBuffer2D mainBuffer in Object.FindObjectsOfType(typeof(LightingMainBuffer2D))) {
			instance = mainBuffer;
			return(instance);
		}

		GameObject setMainBuffer = new GameObject ();
		setMainBuffer.transform.parent = LightingManager2D.Get().transform;
		setMainBuffer.name = "Main Buffer";
		setMainBuffer.layer = LightingManager2D.lightingLayer;

		instance = setMainBuffer.AddComponent<LightingMainBuffer2D> ();
		instance.Initialize();
		
		return(instance);
	}

	public void Initialize() {
		SetUpRenderTexture ();
		SetUpRenderMaterial ();
		SetUpCamera ();
	}

	void SetUpRenderTexture() {
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		renderTexture = new RenderTexture (screenWidth, screenHeight, 16, RenderTextureFormat.ARGB32);
		renderTexture.Create ();
	}

	void SetUpRenderMaterial() {
		material = new Material (Shader.Find ("Particles/Multiply"));
		material.mainTexture = renderTexture;
	}

	void SetUpCamera() {
		bufferCamera = gameObject.AddComponent<Camera> ();
		bufferCamera.clearFlags = CameraClearFlags.Color;
		bufferCamera.backgroundColor = darknessColor;
		bufferCamera.cameraType = CameraType.Game;
		bufferCamera.orthographic = true;
		bufferCamera.targetTexture = renderTexture;
		bufferCamera.farClipPlane = 1f;
		bufferCamera.nearClipPlane = 0f;
		bufferCamera.allowMSAA = false;
		bufferCamera.allowHDR = false;
	}

	void Update () {
		if (Screen.width != screenWidth || Screen.height != screenHeight) {
			SetUpRenderTexture ();
		}

		bufferCamera.orthographicSize = Camera.main.orthographicSize;

		bufferCamera.backgroundColor = darknessColor;

		transform.position = new Vector3(0, 0, Camera.main.transform.position.z - cameraOffset);
		transform.rotation = Camera.main.transform.rotation;

		gameObject.SetActive (false);
		gameObject.SetActive (true);
	}

	public void OnRenderObject() {
		if (Camera.current != bufferCamera) {
			return;
		}

		float z = transform.position.z;

		Vector2D offset = new Vector2D(-Camera.main.transform.position);

		Max2D.Check();

		DrawSoftShadows(offset, z);

		DrawRooms(offset, z);

		DrawLightingBuffers(z);

		DrawOcclussion(offset.ToVector2(), z);
	}

	void DrawSoftShadows(Vector2D offset, float z) {
		float sunDirection = LightingManager2D.GetSunDirection ();
		
		// Day Soft Shadows
		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			if (id.dayHeight && id.height > 0) {
				Polygon2D poly = id.GetPolygon();
				poly = poly.ToWorldSpace (id.gameObject.transform);

				GL.PushMatrix();
				Max2D.defaultMaterial.SetPass(0);
				GL.Begin(GL.TRIANGLES);
				GL.Color(Color.black);
				
				foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
					Vector2D vA = p.A.Copy();
					Vector2D vB = p.B.Copy();

					vA.Push (sunDirection, id.height);
					vB.Push (sunDirection, id.height);

					Max2DMatrix.DrawTriangle(p.A, p.B, vA, offset, z);
					Max2DMatrix.DrawTriangle(vA, vB, p.B, offset, z);
				}
				GL.End();
				GL.PopMatrix();
				
				Polygon2D convexHull = id.GenerateShadow(new Polygon2D(poly.pointsList), sunDirection);

				Max2D.SetColor (Color.white);
				
				GL.PushMatrix ();
				LightingManager2D.Get().shadowBlurMaterial.SetPass (0);
				GL.Begin (GL.TRIANGLES);
				
				foreach (DoublePair2D p in DoublePair2D.GetList(convexHull.pointsList)) {
					Vector2D zA = new Vector2D (p.A + offset);
					Vector2D zB = new Vector2D (p.B + offset);
					Vector2D zC = new Vector2D (p.B + offset);

					Vector2D pA = zA.Copy();
					Vector2D pB = zB.Copy();

					zA.Push (Vector2D.Atan2 (p.A, p.B) + pi2, .5f);
					zB.Push (Vector2D.Atan2 (p.A, p.B) + pi2, .5f);
					zC.Push (Vector2D.Atan2 (p.B, p.C) + pi2, .5f);
					
					GL.TexCoord2 (uv0, uv0);
					Max2D.Vertex3 (pB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pA, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (zA, z);
				
					GL.TexCoord2 (uv0, uv1);
					Max2D.Vertex3 (zA, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (zB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pB, z);
					
					GL.TexCoord2 (uv0, uv1);
					Max2D.Vertex3 (zB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pB, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (zC, z);
				}

				GL.End();
				GL.PopMatrix();
			}
		}

		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			Max2D.SetColor (Color.white);
			Max2D.iDrawMesh (id.GetMesh(), id.transform, offset, z);
		}

		Vector2 size = new Vector2(bufferCamera.orthographicSize * ((float)Screen.width / Screen.height), bufferCamera.orthographicSize);
		Vector3 pos = Camera.main.transform.position;

		Max2D.iDrawImage(LightingManager2D.Get().additiveMaterial, new Vector2D(pos), new Vector2D(size), pos.z);
	}
	
	void DrawRooms(Vector2D offset, float z) {
		// Room Mask
		foreach (LightingRoom2D id in LightingRoom2D.GetList()) {
			Max2D.SetColor (id.color);
			Max2D.iDrawMesh (id.GetMesh(), id.transform, offset, z);
		}
	}

	void DrawLightingBuffers(float z) {
		// Lighting Buffers
		foreach (LightingBuffer2D id in LightingBuffer2D.GetList()) {
			if (id.lightSource != null) {
				if (id.lightSource.isActiveAndEnabled) {
					Vector3 pos = id.lightSource.transform.position - Camera.main.transform.position;
					float size = id.bufferCamera.orthographicSize;
					Max2D.iDrawImage (id.material, new Vector2D (pos), new Vector2D (size, size), z);
				}
			}
		}
	}

	void DrawOcclussion(Vector2 offset, float z) {
		// Collider Ambient Occlusion
		GL.PushMatrix ();
		LightingManager2D.Get().occlusionEdgeMaterial.SetPass (0);
		GL.Begin (GL.TRIANGLES);
		
		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			if (id.ambientOcclusion == true) {
				// Do not call Create From Collider
				Polygon2D poly = Polygon2DList.CreateFromGameObject (id.gameObject)[0];
				poly = poly.ToWorldSpace (id.gameObject.transform);

				foreach (DoublePair2D p in DoublePair2D.GetList(poly.pointsList)) {
					Vector2D vA = new Vector2D (p.A.ToVector2() + offset);
					Vector2D vB = new Vector2D (p.B.ToVector2()  + offset);
					Vector2D vC = new Vector2D (p.B.ToVector2()  + offset);

					Vector2D pA = new Vector2D (p.A.ToVector2()  + offset);
					Vector2D pB = new Vector2D (p.B.ToVector2()  + offset);

					vA.Push (Vector2D.Atan2 (p.A, p.B) + Mathf.PI / 2, 1);
					vB.Push (Vector2D.Atan2 (p.A, p.B) + Mathf.PI / 2, 1);
					vC.Push (Vector2D.Atan2 (p.B, p.C) + Mathf.PI / 2, 1);

					GL.TexCoord2 (uv0, uv0);
					Max2D.Vertex3 (pB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pA, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (vA, z);

					GL.TexCoord2 (uv0, uv1);
					Max2D.Vertex3 (vA, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (vB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pB, z);
		
					GL.TexCoord2 (uv1, uv0);
					Max2D.Vertex3 (vB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pB, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (vC, z);
				}
			}
		}
		GL.End ();
		GL.PopMatrix ();
	}
}