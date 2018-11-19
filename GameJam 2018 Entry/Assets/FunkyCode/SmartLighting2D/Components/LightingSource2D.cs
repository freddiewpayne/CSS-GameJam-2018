using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingSource2D : MonoBehaviour {
	public enum TextureSize {px1024, px512, px256, px128};
	public enum LightSprite {Default, Custom};

	public Color color = new Color(.5f,.5f, .5f, .5f);
	public float lightSize = 5f;
	public TextureSize textureSize = TextureSize.px1024;
	public bool rotationEnabled = false;

	public LightSprite lightSprite = LightSprite.Default;

	public Sprite sprite;
	private Material material;

	private Vector3 updatePosition = Vector3.zero;
	private Color updateColor = Color.white;
	private float updateRotation = 0;
	private float updateSize = 0;
	
	public bool update = true;

	static public List<LightingSource2D> GetList() {
		List<LightingSource2D> result = new List<LightingSource2D>();
		foreach (LightingSource2D buffer in Object.FindObjectsOfType(typeof(LightingSource2D))) {
			result.Add(buffer);
		}
		return(result);
	}

	public int GetTextureSize() {
		switch(textureSize) {
			case TextureSize.px1024:
				return(1024);

			case TextureSize.px512:
				return(512);

			case TextureSize.px256:
				return(256);
			
			default:
				return(128);
		}
	}

	void Start () {
		SetMaterial ();
	}

	void SetMaterial() {
		material = new Material (Shader.Find ("Particles/Multiply"));
		material.mainTexture = GetSprite().texture;
	}

	public Sprite GetSprite() {
		if (sprite == null) {
			sprite = Resources.Load <Sprite> ("Sprites/gfx_light") ;
		}
		return(sprite);
	}
		
	public Material GetMaterial() {
		return(material);
	}

	void Update() {
		if (updatePosition != transform.position) {
			updatePosition = transform.position;

			update = true;
		}

		if (updateRotation != transform.rotation.eulerAngles.z) {
			updateRotation = transform.rotation.eulerAngles.z;

			update = true;
		}

		if (updateSize != lightSize) {
			updateSize = lightSize;

			update = true;
		}

		if (updateColor.Equals(color) == false) {
			updateColor = color;

			update = true;
		}

		if (update == true) {
			LightingBuffer2D buffer = LightingBuffer2D.GetBuffer (GetTextureSize(), this);
			buffer.lightSource = this;
			//buffer.bufferCamera.enabled=true;
		}
	}
}
