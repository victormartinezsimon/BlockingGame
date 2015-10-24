using UnityEngine;
using System.Collections;

public class ConstantMovement : MonoBehaviour {

	public Vector3 m_velocityDirector;
	public float m_velocityMagnitude;
	private Transform m_transform;

	// Use this for initialization
	void Start () {
		m_transform = transform;
	}
	// Update is called once per frame
	void Update () {

		m_transform.position += Time.deltaTime * m_velocityDirector * m_velocityMagnitude;
	}
}
