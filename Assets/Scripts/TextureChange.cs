using UnityEngine;
using System.Collections;

public class TextureChange : MonoBehaviour {

	public Texture m_happy;
	public Texture m_collision;

	public float xLeft;
	public float xRight;

	private Renderer m_renderer;
	private Transform m_transform;

	private bool bounding;

	// Use this for initialization
	void Start () {
		m_renderer = GetComponent<Renderer>();
		m_transform = transform;
		bounding = false;
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

		m_renderer.material.mainTexture = m_happy;
		bounding = false;
	}
	public void setBounding() {
		m_renderer.material.mainTexture = m_happy;
		bounding = true;
	}
}
