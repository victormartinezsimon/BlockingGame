using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
	public int addPlayer = 1;
	public int addBlock = 1;

	[Header("UI")]
	public Text puntuationText;
	public EasyTween m_gameOverScreen;
	public Text m_bestPuntuation;
	public Text m_actualPuntuation;

	private float m_velocityPress;

	private bool gameEnded;
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
		m_velocityPress = blockInitialVelocity * 2;
		actualVelocityPlayer = playerInitialVelocity;
		actualBlockVelocity = blockInitialVelocity;
		fillWorld();
		m_blocksEnabled = new List<GameObject>();
		timeAcum = 0;
		puntuationText.text = m_score.ToString();
		gameEnded = false;
	}
	
	// Update is called once per frame
	void Update () {
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
	}

	public void addScore() {
		++m_score;

		if(m_score % levelIncrease == 0) {
			actualBlockVelocity = Mathf.Min(actualBlockVelocity + addBlock, maxVelocityBlock);
			actualVelocityPlayer = Mathf.Min(actualVelocityPlayer + addPlayer, maxVelocityPlayer);

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
		float initY = Screen.height * 1.5f;

		//left part
		Vector3 leftInfo = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (borderSize + separationBorder),initY, 0));
		float positionXLeft = leftInfo.x;

		float originYLeft = leftInfo.y;
		for(int i = 0; i < m_poolSize/2; ++i) {
			m_blocks[i].transform.position = new Vector3(positionXLeft, originYLeft, 0);
			m_blocks[i].gameObject.name = i.ToString();
			originYLeft -= heightSquare;
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

	public void endGame ()
	{
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
}
