using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] // Only 1 Lighting Manager Allowed
public class LightingManager2D : MonoBehaviour {
	private static LightingManager2D instance;

	public Color darknessColor = Color.black;
	public float sunDirection = - Mathf.PI / 2;

	public LightingMainBuffer2D mainBuffer;

	public Material penumbraMaterial;
	public Material occlusionEdgeMaterial;
	public Material shadowBlurMaterial;
	public Material additiveMaterial;

	public bool enable = true;
	public bool debug = false;

	static public int lightingLayer = 8;

	static public LightingManager2D Get() {
		if (instance != null) {
			return(instance);
		}

		foreach(LightingManager2D manager in Object.FindObjectsOfType(typeof(LightingManager2D))) {
			instance = manager;
			return(instance);
		}

		// Create New Light Manager
		GameObject gameObject = new GameObject();
		gameObject.name = "Lighting Manager 2D";
		instance = gameObject.AddComponent<LightingManager2D>();
		instance.Start();

		return(instance);
	}

	void Start () {
		instance = this;

		// Lighting Materials
		additiveMaterial = new Material (Shader.Find ("Particles/Additive"));
		//additiveMaterial.mainTexture = Resources.Load ("textures/additive") as Texture;

		penumbraMaterial = new Material (Shader.Find ("Particles/Multiply"));
		penumbraMaterial.mainTexture = Resources.Load ("textures/penumbra") as Texture;

		occlusionEdgeMaterial = new Material (Shader.Find ("Particles/Multiply"));
		occlusionEdgeMaterial.mainTexture = Resources.Load ("textures/occlusionedge") as Texture;

		shadowBlurMaterial = new Material (Shader.Find ("Particles/Multiply"));
		shadowBlurMaterial.mainTexture = Resources.Load ("textures/shadowedge") as Texture;

		transform.position = Vector3.zero;

		mainBuffer = LightingMainBuffer2D.Get(); 
	}
	
	void Update() {
		if (mainBuffer == null) { //???
			Start();
		}

		mainBuffer.darknessColor = darknessColor;
	}
		
	public void OnRenderObject() {
		if (enable == false) {
			return;
		}
		
		if (Camera.current != Camera.main) {
			return;
		}

		if (mainBuffer == null) {
			mainBuffer = LightingMainBuffer2D.Get();
			return;
		}	

		if (mainBuffer.bufferCamera == null) {
			return;
		}

		float sizeX = mainBuffer.bufferCamera.orthographicSize * ((float)Screen.width / Screen.height);
		Vector2 size = new Vector2(sizeX, mainBuffer.bufferCamera.orthographicSize);
		
		size.x *=  (16f/9f) / (size.x/size.y);
		Vector3 pos = Camera.main.transform.position;
		pos.z += 1;

		Max2D.iDrawImage2(mainBuffer.material, pos, size, Camera.main.transform.eulerAngles.z, pos.z);
	}

	static public float GetSunDirection() {
		return(Get().sunDirection);
	}

	void OnGUI() {
		if (debug) {
			LightingDebug.OnGUI();
		}
	}

	public class LightingDebug {
		static public int LightBufferUpdates = 0;
		static public int ShowLightBufferUpdates = 0;
		
		static public TimerHelper timer;

		static public void OnGUI() {
			if (timer == null) {
				LightingDebug.timer = TimerHelper.Create();
			}
			if (timer.GetMillisecs() > 1000) {
				ShowLightBufferUpdates = LightBufferUpdates;

				LightBufferUpdates = 0;

				timer = TimerHelper.Create();
			}
			GUI.Label(new Rect(10, 10, 200, 20), "Light Buffer Updates: " + ShowLightBufferUpdates);
			GUI.Label(new Rect(10, 30, 200, 20), "Light Buffer Count: " + LightingBuffer2D.GetList().Count);
		}
	}
}
