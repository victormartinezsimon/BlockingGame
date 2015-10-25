using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public GameObject player;
	public GameObject blockLeft;
	public GameObject blockRight;

	[Header("World Colocation")]
	public float separationBorder = 0.2f;
	public float height = 0.3f;

	[Header("Easter egg")]
	public float timeBetweenEaster = 1;
	private float timeAcum;
	public GameObject dead;

	// Use this for initialization
	void Start () {
	
		colocateBlocks();
		timeAcum = 0;

	}
	private void colocateBlocks() {

		float myHeight = Screen.height * height;

		Vector3 leftPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * separationBorder, myHeight, 0));
		Vector3 rightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * (1 - separationBorder), myHeight, 0));
		Vector3 playerPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, myHeight, 0));

		player.transform.position = new Vector3(playerPos.x, playerPos.y, 0);
		blockLeft.transform.position = new Vector3(leftPos.x, leftPos.y, 0);
		blockRight.transform.position = new Vector3(rightPos.x, rightPos.y, 0);
	}

	public void startGame() {
		Application.LoadLevel("GameScene");
	}

	void Update() {
		timeAcum += Time.deltaTime;

		if (Input.touchCount > 0 ) {
			if(Input.GetTouch(0).phase == TouchPhase.Began) {
				timeAcum = 0;
				easterEgg(Input.GetTouch(0).position);
			}
		}

		if(Input.GetMouseButtonDown(0)) {
			easterEgg(Input.mousePosition);
			timeAcum = 0;
		}

		if(Input.GetKeyDown(KeyCode.Escape)) {
			exitGame();
		}
	}

	private void easterEgg(Vector3 pixelPos) {
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(pixelPos);
		worldPos.z = 10;
		Instantiate(dead, worldPos, Quaternion.identity);
	}

	public void exitGame() {
		Application.Quit();
	}
}
