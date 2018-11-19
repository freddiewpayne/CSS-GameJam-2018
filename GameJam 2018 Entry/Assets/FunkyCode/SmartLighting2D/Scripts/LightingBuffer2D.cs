using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingBuffer2D : MonoBehaviour {
	public RenderTexture renderTexture;
	public LightingSource2D lightSource;
	public int textureSize = 0;

	public Material material;

	public Camera bufferCamera;

	static public int GetCount() {
		return(GetList().Count);
	}

	static public List<LightingBuffer2D> GetList() {
		List<LightingBuffer2D> result = new List<LightingBuffer2D>();
		foreach (LightingBuffer2D buffer in Object.FindObjectsOfType(typeof(LightingBuffer2D))) {
			result.Add(buffer);
		}
		return(result);
	}

	void LateUpdate() {
		if (lightSource != null) { // Check if size > 0
			if (lightSource.lightSize > 0) {
				float cameraZ = Camera.main.transform.position.z - 10 - GetCount();

				bufferCamera.transform.position = new Vector3(0, 0, cameraZ);
				bufferCamera.orthographicSize = lightSource.lightSize;
			}
		}
	}

	public void OnRenderObject() {
		if(Camera.current != bufferCamera) {
			return;
		}

		LightingManager2D.LightingDebug.LightBufferUpdates ++;

		if (lightSource == null) {
			return;
		}

		if (lightSource.update == false) {
			bufferCamera.enabled = false;
		}
	
		LateUpdate ();

		lightSource.update = false;
		
		material.SetColor ("_TintColor", lightSource.color);

		Vector2D vZero = new Vector2D (0, 0);
		float z = transform.position.z;

		Max2D.Check();

		GL.PushMatrix();
		Max2D.defaultMaterial.SetPass(0);

		GL.Begin(GL.TRIANGLES);
		GL.Color(Color.black);

		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			Polygon2D poly = id.GetPolygon();
			poly = poly.ToWorldSpace (id.gameObject.transform);
			poly = poly.ToOffset (new Vector2D (-lightSource.transform.position));

			if (poly.PointInPoly (vZero)) {
				continue;
			}

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
				Vector2D vA = p.A.Copy();
				Vector2D vB = p.B.Copy();
				
				vA.Push (Vector2D.Atan2 (vA, vZero), 15);
				vB.Push (Vector2D.Atan2 (vB, vZero), 15);
				
				Max2DMatrix.DrawTriangle(p.A ,p.B, vA, vZero, z);
				Max2DMatrix.DrawTriangle(vA, vB, p.B, vZero, z);
			}
		}

		GL.End();
		GL.PopMatrix();

		GL.PushMatrix();

		LightingManager2D.Get().penumbraMaterial.SetPass(0);

		GL.Begin(GL.TRIANGLES);
		GL.Color(Color.white);
		
		const float uv0 = 1f / 128f;
		const float uv1 = 1f - uv0;

		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			Polygon2D poly = id.GetPolygon();
			poly = poly.ToWorldSpace (id.gameObject.transform);
			poly = poly.ToOffset (new Vector2D (-lightSource.transform.position));

			if (poly.PointInPoly (vZero)) {
				continue;
			}

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
				Vector2D vA = p.A.Copy();
				Vector2D pA = p.A.Copy();

				Vector2D vB = p.B.Copy();
				Vector2D pB = p.B.Copy();

				float angleA = (float)Vector2D.Atan2 (vA, vZero);
				float angleB = (float)Vector2D.Atan2 (vB, vZero);

				vA.Push (angleA, lightSource.lightSize);
				pA.Push (angleA - Mathf.Deg2Rad * 25, lightSource.lightSize);

				vB.Push (angleB, lightSource.lightSize);
				pB.Push (angleB + Mathf.Deg2Rad * 25, lightSource.lightSize);

				GL.TexCoord2(uv0, uv0);
				GL.Vertex3((float)p.A.x,(float) p.A.y, z);
				GL.TexCoord2(uv1, uv0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);
				GL.TexCoord2((float)uv0, uv1);
				GL.Vertex3((float)pA.x,(float) pA.y, z);

				GL.TexCoord2(uv0, uv0);
				GL.Vertex3((float)p.B.x,(float) p.B.y, z);
				GL.TexCoord2(uv1, uv0);
				GL.Vertex3((float)vB.x, (float)vB.y, z);
				GL.TexCoord2(uv0, uv1);
				GL.Vertex3((float)pB.x, (float)pB.y, z);
			}
		}

		GL.End();
		GL.PopMatrix();

		Max2D.SetColor (Color.white);
		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			Max2D.iDrawMesh (id.GetMesh (), id.transform, new Vector2D (-lightSource.transform.position), z);
		}

		Vector2 size = new Vector2 (bufferCamera.orthographicSize, bufferCamera.orthographicSize);

		if (lightSource.rotationEnabled) {
			Max2D.DrawImage(lightSource.GetMaterial (), Vector2.zero, size, lightSource.transform.rotation.eulerAngles.z, z);
		} else {
			Max2D.DrawImage(lightSource.GetMaterial (), Vector2.zero, size, 0, z);
		}
		
		GL.PushMatrix();
		Max2D.SetColor (Color.black);
		Max2D.defaultMaterial.color = Color.black;
		Max2D.defaultMaterial.SetPass(0);

		float rotation = lightSource.transform.rotation.eulerAngles.z;
		float squaredSize = Mathf.Sqrt((size.x * size.x) + (size.y * size.y));
		
		Vector2 p0 = Vector2D.RotToVec((double)rotation).ToVector2() * squaredSize;
		Vector2 p1 = Vector2D.RotToVec((double)rotation + Mathf.PI / 4).ToVector2() * squaredSize;
		
		Max2DMatrix.DrawTriangle(Vector2.zero, p0, p1, Vector2.zero, z);
		Max2DMatrix.DrawTriangle(Vector2.zero, p1, p0, Vector2.zero, z);

		//Max2DMatrix.DrawTriangle(vA, vB, p.B, vZero, z);

		GL.End();
		GL.PopMatrix();

		Max2D.defaultMaterial.color = Color.white;
		
		//lightSource = null;
		//bufferCamera.enabled = false;
	}

	static public LightingBuffer2D AddBuffer(int textureSize) {
		GameObject buffer = new GameObject ();
		buffer.name = "Buffer " + GetCount();
		buffer.transform.parent = LightingManager2D.Get().mainBuffer.transform;
		buffer.layer = LightingManager2D.lightingLayer;

		LightingBuffer2D lightingBuffer = buffer.AddComponent<LightingBuffer2D> ();
		lightingBuffer.Initiate (textureSize);

		return(lightingBuffer);
	}

	static public LightingBuffer2D GetBuffer(int textureSize, LightingSource2D lightSource) {
		foreach (LightingBuffer2D id in LightingBuffer2D.GetList()) {
			if ((id.lightSource == lightSource || id.lightSource == null) && id.textureSize == textureSize) {
				id.lightSource = lightSource;
				lightSource.update = true;
				id.bufferCamera.enabled = true;
				//id.gameObject.SetActive (true);
				return(id);
			}
		}
			
		return(AddBuffer(textureSize));		
	}

	public void Initiate (int textureSize) {
		SetUpRenderTexture (textureSize);
		SetUpRenderMaterial();
		SetUpCamera ();
	}

	void SetUpRenderTexture(int _textureSize) {
		textureSize = _textureSize;

		renderTexture = new RenderTexture(textureSize, textureSize, 16, RenderTextureFormat.ARGB32);

		name = "Buffer " + GetCount() + " (size: " + textureSize + ")";
	}

	void SetUpRenderMaterial() {
		material = new Material (Shader.Find ("Particles/Additive"));
		material.mainTexture = renderTexture;
	}

	void SetUpCamera() {
		bufferCamera = gameObject.AddComponent<Camera> ();
		bufferCamera.clearFlags = CameraClearFlags.Color;
		bufferCamera.backgroundColor = Color.white;
		bufferCamera.cameraType = CameraType.Game;
		bufferCamera.orthographic = true;
		bufferCamera.targetTexture = renderTexture;
		bufferCamera.farClipPlane = 0.5f;
		bufferCamera.nearClipPlane = 0f;
		bufferCamera.allowHDR = false;
		bufferCamera.allowMSAA = false;
		bufferCamera.enabled = false;
	}
}

		//static private List<LightingBuffer2D> list = new List<LightingBuffer2D>();

	//void Awake() {
	//	list.Add (this);
	//}

	//void OnDisable() { list.Remove (this);} // OnDestroy?

		//List<LightingBuffer2D> result = new List<LightingBuffer2D>(list);
		//foreach (LightingBuffer2D buffer in result) {
		//	if (buffer ==null || buffer.gameObject == null) {
		//		list.Remove(buffer);
		//		continue;
		//	}
		//}
		//return(new List<LightingBuffer2D>(list));
/*
	foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
		Vector2D vA = new Vector2D (p.A);
		Vector2D vB = new Vector2D (p.B);
		vA.Push (Vector2D.Atan2 (vA, vZero), 15);
		vB.Push (Vector2D.Atan2 (vB, vZero), 15);

		//Penumbra
		Vector2D vA2 = new Vector2D (vA);
		Vector2D vB2 = new Vector2D (vB);
		vA2.Push (Vector2D.Atan2 (vA, vZero) - Mathf.PI / 4, 15);
		vB2.Push (Vector2D.Atan2 (vB, vZero) + Mathf.PI / 4, 15);

		float a = Vector2D.Atan2 (vA, vZero) * Mathf.Rad2Deg - Vector2D.Atan2 (vB, vZero) * Mathf.Rad2Deg;
		a = (a + 180) % 360 - 180;

		if (a < 0) {
			GL.PushMatrix ();
			SmartLighting2DManager.instance.penumbraMaterial.SetPass (0);

			GL.Begin (GL.TRIANGLES);
			Max2D.Vertex3 (p.A, z);
			GL.TexCoord2 (uv0, uv1);
			Max2D.Vertex3 (vA2, z);
			GL.TexCoord2 (uv1, uv1);
			Max2D.Vertex3 (vA, z);
			GL.TexCoord2 (uv1, uv0);
			
			Max2D.Vertex3 (p.B, z);
			GL.TexCoord2 (uv0, uv1);
			Max2D.Vertex3 (vB2, z);
			GL.TexCoord2 (uv1, uv1);
			Max2D.Vertex3 (vB, z);
			GL.TexCoord2 (uv1, uv0);
			GL.End ();
			GL.PopMatrix ();
		}
	}
		*/
