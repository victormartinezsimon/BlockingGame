using UnityEngine;
using System.Collections;

public class AutoKill : MonoBehaviour {

	public float time = 2;
	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
