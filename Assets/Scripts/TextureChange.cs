using UnityEngine;
using System.Collections;

public class TextureChange : MonoBehaviour {

	public Texture m_happy;
	public Texture m_collision;

	[HideInInspector]
	public float xLeft;
	[HideInInspector]
	public float xRight;

	private Renderer m_renderer;
	private Transform m_transform;

	private bool bounding;

	private PendulumMovement movement;

	// Use this for initialization
	void Start () {
		m_renderer = GetComponent<Renderer>();
		m_transform = transform;
		bounding = false;
		movement = GetComponent<PendulumMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(xLeft >= m_transform.position.x) {
			if(bounding) {
				return;
			}
			m_renderer.material.mainTexture = m_collision;
			return;
		}

		if(xRight <= m_transform.position.x) {
			if(bounding) {
				return;
			}
			m_renderer.material.mainTexture = m_collision;
			return;
		}

		if(bounding) {
			bounding = false;
			m_renderer.material.mainTexture = m_happy;
		}

	}
	public void setBounding() {
		m_renderer.material.mainTexture = m_happy;
		bounding = true;
	}
}
