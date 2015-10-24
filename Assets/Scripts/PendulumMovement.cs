using UnityEngine;
using System.Collections;

public class PendulumMovement : MonoBehaviour {

	private Vector3 m_velocityVector;
	private bool m_right;
	public float m_velocityMagnitude;
	private TextureChange m_textureChange;

	public float VelocityMagnitude{
		get{ return m_velocityMagnitude;}
		set{ m_velocityMagnitude = value;}
	}

	private Transform m_transform;
	// Use this for initialization
	void Start () {
		m_velocityVector = new Vector3(-1,0,0);
		m_right = false;
		m_transform = this.transform;
		m_textureChange = GetComponent<TextureChange>();
	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.deltaTime;
		Vector3 deltaPosition = m_velocityVector * time * m_velocityMagnitude;
		this.m_transform.position += deltaPosition;
	}

	void OnCollisionEnter(Collision collision) {
		m_velocityVector *= -1;
		m_right = !m_right;
		m_textureChange.setBounding();
	}
}
