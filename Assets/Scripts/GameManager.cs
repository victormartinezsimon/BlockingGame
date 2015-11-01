using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour {

	public GameObject m_playerPrefab;
	private GameObject m_player;
	public GameObject m_blockPrefab;
	public int m_poolSize = 20;

	public GameObject m_spike;

	private GameObject[] m_blocks;
	private List<GameObject> m_blocksEnabled;

	private int m_score;

	[Header("World Colocation")]
	public float separationBorder = 0.1f;
	public float borderSize = 0.05f;
	public float changeTexture = 0.1f;

	private Camera m_camera;

	[Header("Velocities")]
	public float maxVelocityPlayer;
	public float playerInitialVelocity;
	private float actualVelocityPlayer;
	public float maxVelocityBlock;
	public float blockInitialVelocity;
	private float actualBlockVelocity;

	[Header("Game Level")]
	public int levelIncrease = 6;
	public float percentageCreation = 0.8f;
	public float percentageCreationAlpha = 0.05f;
	public float percentageCreationMin = 0.2f;
	public float timeBetweenTries = 0.5f;
	private float timeAcum;
	public int addPlayerVelocity = 1;
	public int addBlockVelocity = 1;

	[Header("UI")]
	public Text puntuationText;
	public EasyTween m_gameOverScreen;
	public Text m_bestPuntuation;
	public Text m_actualPuntuation;
	public SpriteRenderer m_clickAnimation;
	public EasyTween m_showVideoToContinue;

	private float m_velocityPress;

	private bool gameEnded;
	private bool gameStarted;
	private bool alreadyContinued;
	private bool tryToShowVideo;
	#region getters and setters
	public float blockVelocity {
		get{ return m_velocityPress;}
	}
	#endregion

	// Use this for initialization
	void Start () {
		m_score = 0;
		m_camera = Camera.main;
		Random.seed = 10;
		m_blocksEnabled = new List<GameObject>();
		timeAcum = 0;
		alreadyContinued = false;
		tryToShowVideo = false;
		puntuationText.text = m_score.ToString();
		continueGame();
	}

	private void continueGame(bool _continue = false) {
		Destroy(m_player);
		m_blocksEnabled.Clear();
		if(m_blocks != null) {
			for(int i = 0; i < m_blocks.Length; i++) {
				Destroy(m_blocks[i]);
			}
		}
		m_blocks = null;

		m_velocityPress = blockInitialVelocity * 2;
		actualVelocityPlayer = playerInitialVelocity;
		actualBlockVelocity = blockInitialVelocity;
		fillWorld();
		gameStarted = false;
		gameEnded = false;

		if(_continue) {
			m_clickAnimation.gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(!gameStarted) {

			if(Input.touchCount!= 0 || Input.GetMouseButtonDown(0)){
				gameStarted = true;
				enableAllBlocks();
				m_clickAnimation.gameObject.SetActive(false);
			}

			return;
		}

		timeAcum += Time.deltaTime;

		if(timeAcum >= timeBetweenTries) {
			timeAcum = 0;
			float initY = Screen.height * 1.5f;
			if(Random.value < percentageCreation) {
				Vector3 leftInfo = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (borderSize + separationBorder),initY, 0));
				if(m_blocksEnabled.Count != 0) {
					GameObject go = m_blocksEnabled[0];
					m_blocksEnabled.RemoveAt(0);
					go.transform.position = new Vector3(leftInfo.x, leftInfo.y, 0);
					go.GetComponent<BlockMovement>().enabled = true;
				}
			}

			if(Random.value < percentageCreation) {
				Vector3 rightInfo = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (1 - (borderSize + separationBorder)),initY, 0));
				if(m_blocksEnabled.Count != 0) {
					GameObject go = m_blocksEnabled[0];
					m_blocksEnabled.RemoveAt(0);
					go.transform.position = new Vector3(rightInfo.x, rightInfo.y, 0);
					go.GetComponent<BlockMovement>().enabled = true;
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape)) {
			backToMenu();
		}
	}

	private void enableAllBlocks() {
		for(int i = 0; i < m_blocks.Length; ++i) {
			m_blocks[i].GetComponent<BlockMovement>().enabled = true;
		}
	}

	public void addScore() {
		if(!gameStarted) {
			return;
		}
		++m_score;

		if(m_score % levelIncrease == 0) {
			actualBlockVelocity = Mathf.Min(actualBlockVelocity + addBlockVelocity, maxVelocityBlock);
			actualVelocityPlayer = Mathf.Min(actualVelocityPlayer + addPlayerVelocity, maxVelocityPlayer);

			updateVelocityPlayer();
			updateVelocityBlock();
			percentageCreation = Mathf.Max(percentageCreation - percentageCreationAlpha, percentageCreationMin);
		}

		puntuationText.text = m_score.ToString();
	}

	private void updateVelocityPlayer() {
		m_player.GetComponent<PendulumMovement>().m_velocityMagnitude = actualVelocityPlayer;
	}

	private void updateVelocityBlock() {
		for (int i = 0; i < m_poolSize; ++i) {
			m_blocks[i].GetComponent<BlockMovement>().m_velocityMagnitude = actualBlockVelocity;
		}
	}

	private void fillWorld() {
		instantiatePlayer();
		instantiateBlocks();
		colocateBlocks();
		instantiateAndColocateSpikes();
		colocateAnimationClick();
	}

	private void instantiatePlayer() {

		Vector3 posInstantiatePlayer = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f , 0));
		m_player = Instantiate(m_playerPrefab);
		m_player.transform.position = new Vector3(posInstantiatePlayer.x, posInstantiatePlayer.y, 0);
		PendulumMovement pendulum = m_player.GetComponent<PendulumMovement>();
		pendulum.GameManager = this;
		pendulum.m_velocityMagnitude = playerInitialVelocity;
		
		float posChangeTextureLeft = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (changeTexture +  borderSize +  separationBorder), 0, 0)).x;
		float posChangeTextureRight = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (1 -  (changeTexture +  borderSize +  separationBorder)), 0, 0)).x;

		TextureChange texture = m_player.GetComponent<TextureChange>();
		texture.xLeft = posChangeTextureLeft;
		texture.xRight = posChangeTextureRight;

	}

	private void instantiateBlocks() {
		m_blocks = new GameObject[m_poolSize];
		for (int i = 0; i < m_poolSize; ++i) {
			GameObject go = Instantiate(m_blockPrefab);
			m_blocks[i] = go;
			go.transform.position = new Vector3(2000,2000,2000);
			BlockMovement movement = go.GetComponent<BlockMovement>();
			movement.m_manager = this;
			movement.m_velocityMagnitude = blockInitialVelocity;
			movement.id = i;
		}
	}

	private void colocateBlocks() {

		float heightSquare = m_blockPrefab.GetComponent<Renderer>().bounds.size.y;
		float initY = Screen.height * 1.4f;

		//left part
		Vector3 leftInfo = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (borderSize + separationBorder),initY, 0));
		float positionXLeft = leftInfo.x;

		float originYLeft = leftInfo.y;
		for(int i = 0; i < m_poolSize/2; ++i) {
			m_blocks[i].transform.position = new Vector3(positionXLeft, originYLeft, 0);
			m_blocks[i].gameObject.name = i.ToString();
			originYLeft -= heightSquare;
			m_blocks[i].GetComponent<BlockMovement>().enabled = false;
		}

		//right part
		Vector3 rightInfo = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (1 - (borderSize + separationBorder)), initY, 0));
		float positionXRight = rightInfo.x;
		
		float originYRight = rightInfo.y;
		for(int i = m_poolSize/2; i <m_poolSize ; ++i) {
			m_blocks[i].transform.position = new Vector3(positionXRight, originYRight, 0);
			m_blocks[i].gameObject.name = i.ToString();
			originYRight -= heightSquare;
		}

	}

	public void blockEnable(int id) {
		m_blocksEnabled.Add(m_blocks[id]);
	}

	private void instantiateAndColocateSpikes() {

		Vector3 spikeSize = m_spike.GetComponent<Renderer>().bounds.size;
		float topSpikesInWorld = m_camera.ScreenToWorldPoint(new Vector3(0,Screen.height * 1.2f,0)).y;
		float bottomSpikesInWorld = m_camera.ScreenToWorldPoint(new Vector3(0, -Screen.height * 0.2f, 0)).y;

		float totalSpikes = Mathf.Abs(topSpikesInWorld - bottomSpikesInWorld) / spikeSize.y;

		float yAcum = topSpikesInWorld;
		float xLeft = m_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
		for(int i = 0; i < totalSpikes; i++) {
			GameObject go = Instantiate(m_spike);

			go.transform.position = new Vector3(xLeft + spikeSize.x/2, yAcum, 0);

			yAcum -=spikeSize.y;
		}
		yAcum = topSpikesInWorld;
		float xRight = m_camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
		for(int i = 0; i < totalSpikes; i++) {
			GameObject go = Instantiate(m_spike);
			go.transform.position = new Vector3(xRight - spikeSize.x/2, yAcum, 0);
			go.transform.Rotate(0,0,180);
			yAcum -=spikeSize.y;
		}
	}

	private void colocateAnimationClick() {

		float posy = Screen.height * (0.5f - 0.2f);
		Vector3 posInstantiateAnimation = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, posy , 0));
		posInstantiateAnimation.z = 0;
		m_clickAnimation.transform.position = posInstantiateAnimation;
	}

	public void endGame ()
	{
		if(!alreadyContinued) {
			if(!tryToShowVideo) {
				showOptionVideo();
			}
			tryToShowVideo = true;
			return;
		}

		if(!gameEnded) {
			m_gameOverScreen.OpenCloseObjectAnimation();
			m_actualPuntuation.text = m_score.ToString();


			if(PlayerPrefs.GetInt("Score") < m_score) {
				PlayerPrefs.SetInt("Score", m_score);
			}
			m_bestPuntuation.text = PlayerPrefs.GetInt("Score").ToString();
			gameEnded = true;
		}
	}

	public void restarGame() {
		Application.LoadLevel(Application.loadedLevel);
	}

	public void backToMenu() {
		Application.LoadLevel("Menu");
	}
	public void showAdd() {
		AdsManager.showVideoAdd(CallBackAds);
		m_showVideoToContinue.gameObject.SetActive(false);
	}
	public void dontShowAdd() {
		alreadyContinued = true;
		m_showVideoToContinue.gameObject.SetActive(false);
		endGame();
	}
	private void showOptionVideo() {
		#if UNITY_STANDALONE || UNITY_EDITOR
		alreadyContinued = true;
		endGame();
		#else
		m_showVideoToContinue.OpenCloseObjectAnimation();
		#endif
	}
	
	private void CallBackAds(bool result) {
		alreadyContinued = true;
		if(result) {
			continueGame(true);//reload scene with same score
		} else {
			endGame();
		}
	}
}
