using UnityEngine;
using System.Collections;

public class PendulumMovement : MonoBehaviour {

	[HideInInspector]
	public float m_velocityMagnitude;

	private Vector3 m_velocityVector;
	private bool m_right;
	private TextureChange m_textureChange;
	private GameManager m_manager;

	public float timeBetweenCollision = 0.1f;
	private float timeAcum = 0;

	public float VelocityMagnitude{
		get{ return m_velocityMagnitude;}
		set{ m_velocityMagnitude = value;}
	}

	public GameManager GameManager {
		set{ m_manager = value;}
	}

	private Transform m_transform;
	// Use this for initialization
	void Start () {
		m_velocityVector = new Vector3(-1,0,0);
		m_right = false;
		m_transform = this.transform;
		m_textureChange = GetComponent<TextureChange>();
		timeAcum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.deltaTime;
		Vector3 deltaPosition = m_velocityVector * time * m_velocityMagnitude;
		this.m_transform.position += deltaPosition;

		timeAcum += time;
	}

	void OnCollisionEnter(Collision collision) {
		if(timeAcum >= timeBetweenCollision) {
			m_velocityVector *= -1;
			m_right = !m_right;
			m_textureChange.setBounding();
			m_manager.addScore();
			timeAcum = 0;
		}
	}
}
