using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject m_playerPrefab;
	private GameObject m_player;
	public GameObject m_blockPrefab;
	public int m_poolSize = 20;

	private GameObject[] m_blocks;

	private int m_score;

	[Header("World Colocation")]
	public float separationBorder = 0.1f;
	public float borderSize = 0.05f;
	public float changeTexture = 0.2f;

	private Camera m_camera;

	[Header("Velocities")]
	public float maxVelocityPlayer;
	public float playerInitialVelocity;
	public float maxVelocityBlock;
	public float blockInitialVelocity;

	private float m_velocityPress;
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
		fillWorld();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addScore() {
		++m_score;
	}

	private void fillWorld() {

		instantiatePlayer();
		instantiateBlocks();
		colocateBlocks();
	}

	private void instantiatePlayer() {
		Vector3 posInstantiatePlayer = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f , 0));
		m_player = Instantiate(m_playerPrefab);
		m_player.transform.position = new Vector3(posInstantiatePlayer.x, posInstantiatePlayer.y, 0);
		PendulumMovement pendulum = m_player.GetComponent<PendulumMovement>();
		pendulum.GameManager = this;
		pendulum.m_velocityMagnitude = playerInitialVelocity;
		
		float posChangeTextureLeft = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * changeTexture, 0, 0)).x;
		float posChangeTextureRight = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (1 - changeTexture), 0, 0)).x;

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
		}
	}

	private void colocateBlocks() {

		float heightSquare = m_blockPrefab.GetComponent<Renderer>().bounds.size.y;

		//left part
		Vector3 leftInfo = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (borderSize + separationBorder), 0, 0));
		float positionXLeft = leftInfo.x;

		float originYLeft = leftInfo.y;
		for(int i = 0; i < m_poolSize/2; ++i) {
			m_blocks[i].transform.position = new Vector3(positionXLeft, originYLeft, 0);
			m_blocks[i].gameObject.name = i.ToString();
			originYLeft += heightSquare;
		}

		//right part
		Vector3 rightInfo = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * (1 - (borderSize + separationBorder)), 0, 0));
		float positionXRight = rightInfo.x;
		
		float originYRight = rightInfo.y;
		for(int i = m_poolSize/2; i <m_poolSize ; ++i) {
			m_blocks[i].transform.position = new Vector3(positionXRight, originYRight, 0);
			m_blocks[i].gameObject.name = i.ToString();
			originYRight += heightSquare;
		}

	}
}
