using UnityEngine;
using System.Collections;

public class Kill : MonoBehaviour {

	public GameObject littleCubes;
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			Destroy(other.gameObject);
			GameObject go = Instantiate(littleCubes);
			go.transform.position = other.transform.position;

			FindObjectOfType<GameManager>().endGame();
		}
	}
}
