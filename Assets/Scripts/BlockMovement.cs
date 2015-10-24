using UnityEngine;
using System.Collections;

public class BlockMovement : MonoBehaviour {

	public Vector3 m_velocityDirector;
	public float m_velocityMagnitude;
	private Transform m_transform;
	public GameManager m_manager;

	private bool touchOption;

	// Use this for initialization
	void Start () {
		m_transform = transform;
		touchOption = false;
	}
	// Update is called once per frame
	void Update () {
		manageTouch();
		float time = getTime();
		m_transform.position += time * m_velocityDirector * m_velocityMagnitude;
	}
	private float getTime() {
		float time = Time.deltaTime;

		if(Input.GetMouseButton(0) || touchOption) {
			time *= m_manager.blockVelocity;
		}
		return time;
	}

	private void manageTouch() {
		if (Input.touchCount > 0 ) {
			if(!touchOption && Input.GetTouch(0).phase == TouchPhase.Began) {
				touchOption = true;
			}
			if(touchOption && Input.GetTouch(0).phase == TouchPhase.Ended) {
				touchOption = false;
			}
			if( Input.GetTouch(0).phase == TouchPhase.Canceled) {
				touchOption = false;
			}
		}
	}
}
