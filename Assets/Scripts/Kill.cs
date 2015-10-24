using UnityEngine;
using System.Collections;

public class Kill : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		Destroy(other.gameObject);
		Debug.Log("Dead");
	}
}
