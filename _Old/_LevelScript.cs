using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour
{
	public bool wavesIncoming = false;
	public bool spawningEnemies = false;
	
	public UILabel scoreLabel;
	public UILabel waveLabel;
	public UILabel nextWaveLabel;
	public UILabel GameOverLabel;
	
	public UIPanel gemPanel;
	public UIButton courierButton;
	public UILabel courierCounter;
	public LayerMask gemLayer;
	
	public float difficulty = 2.7f;
	public float waveDelay = 5f;
	
	public float gemTakeBackDelay = 20f;
	
	public GameObject[] waves;
	public Camera GUICamera;
	public Transform spawnPoint;
	public float enemySpawnDelay = 1f;
	public GameObject gemPrefab;
	
	private bool gameIsOver = false;
	private bool victory = false;
	private int score = 0;
	private int health = 0;
	private int totalEnemyCounter = 0;
	private int enemyCounter = 0;
	private int waveNumber = 0;
	private float nextEnemySpawn = 0f;
	private float waveDelayTime = 0f;
	private int waveIndexer = 0;
	private int enemyCounterPerWave = 0;
	private WaveScript currentWave;
	private int enemySpawnCounter = 0;
	private int gemCounter = 0;
	private int gemsInGame = 0;
	private float timeToNextWave;
	private string timeToNextWaveText = "";
	private bool gemPanelLock = false;
	private bool superpowersPanelOpen = false;
	private float gtbd = 0;								//	Gem Take Back Delay
	private int clickedGemNo = 0;
	private CandyshopEnemyScript candyshop;
	private GameObject activeGem;
	private bool turnOffGUI = true;

	private bool waitForExit = false;
	
	void Awake()
	{
		candyshop = GameObject.FindGameObjectWithTag("Candyshop").GetComponent<CandyshopEnemyScript>();
		GUICamera.enabled = false;
		GUICamera.enabled = true;
	}
	
	void Start()
	{
		gemCounter = candyshop.gems.Length;
		gemsInGame = gemCounter;

		gemPanel.GetComponent<TweenPosition>().Play(false);

		Debug.Log("TotalWaves: " + (waves.Length - 1));
	}
	
	void Update()
	{
		Debug.Log("Wave: " + waveNumber + "\tenemySpawnCounter" + enemySpawnCounter + "\twaveIncoming? " + wavesIncoming + "\tgameIsOver? " + gameIsOver);

		if(wavesIncoming && !victory)
		{
			if(spawningEnemies)
			{
				ChangeNextWaveLabel("Incoming!");
				if(Time.time >= nextEnemySpawn)
				{
					spawnEnemies();
				}
			}
			else if(!spawningEnemies && Time.time >= waveDelayTime)
			{
				SetNextWave();
				GetNextWave();
				StartNextWave();
			}
			else
			{
				timeToNextWave = waveDelayTime - Time.time;
				timeToNextWaveText = "Next wave in: " + ((int)timeToNextWave).ToString();
				ChangeNextWaveLabel(timeToNextWaveText);
			}

		}
		
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, 50f, gemLayer))
		{
			if(Input.GetMouseButton(0) && !superpowersPanelOpen)
			{
				activeGem = hit.collider.gameObject;
				superpowersPanelOpen = true;
				if(Time.time < gtbd) courierButton.isEnabled = false;
				gemPanel.GetComponent<TweenPosition>().Play(true);
			}
		}
		else if(Input.GetMouseButton(0) && !gemPanelLock && superpowersPanelOpen)
		{
			superpowersPanelOpen = false;
			gemPanel.GetComponent<TweenPosition>().Play(false);
		}
	}
	
	void LateUpdate()
	{
		float timeToNextUse = ((int)((gtbd - Time.time) * 10f)) / 10f;
		if(!courierButton.isEnabled) courierCounter.text = "" + timeToNextUse;//((int)(gtbd - Time.time)).ToString();
		if(Time.time >= gtbd)
		{
			courierButton.isEnabled = true;
			courierCounter.text = "";
		}
		
		if(gemsInGame == 0 && !gameIsOver) GameOver();
		Debug.Log("\t\tvictory: " + victory);
		if(!wavesIncoming && victory) GameWin();

		if(waitForExit && Input.GetKey(KeyCode.Escape)) Application.Quit();
	}
	
	void spawnEnemies()
	{
		if(currentWave.waveEnemies[waveIndexer])
		{
			float randomX = Random.Range(-0.7f, 0.7f);
			float randomZ = Random.Range(-0.7f, 0.7f);
			Vector3 spawnPosition = new Vector3(spawnPoint.position.x + randomX, spawnPoint.position.y, spawnPoint.position.z + randomZ);
			
			GameObject enemyClone = Instantiate(currentWave.waveEnemies[waveIndexer], spawnPosition, spawnPoint.rotation) as GameObject;
			enemyClone.GetComponent<EnemyScript>().enemyHealth += (waveNumber * difficulty);
			enemySpawnCounter++;
			if(enemySpawnCounter == currentWave.enemyCounter[waveIndexer])
			{
				waveIndexer++;
				enemySpawnCounter = 0;
				CheckWaveEnd();
			}
			totalEnemyCounter++;
			enemyCounter++;
			CalculateNextSpawn();
		}
	}
	
	void CheckWaveEnd()
	{
		if(waveIndexer == currentWave.waveEnemies.Length)
		{
			spawningEnemies = false;
			waveIndexer = 0;
			CheckGameEnd();
			if(wavesIncoming) CalculateNextWave();
		}
	}
	
	void CheckGameEnd()
	{
		if(waveNumber == waves.Length - 1)
		{
			wavesIncoming = false;
		}
	}
	
	void GetNextWave()
	{
		if(waveNumber <= waves.Length - 1)
		{
			currentWave = waves[waveNumber].GetComponent<WaveScript>();
			enemySpawnCounter = 0;
		}
	}
	
	void CalculateNextWave()
	{
		waveDelayTime = Time.time + waveDelay;
	}
	
	void CalculateNextSpawn()
	{
		nextEnemySpawn = Time.time + enemySpawnDelay;
	}
	
	void SetNextWave()
	{
		wavesIncoming = true;
		spawningEnemies = true;
		waveNumber++;
	}

	void DisableActiveGem()
	{
		activeGem = null;
	}
	
	void StartNextWave()
	{
		UpdateHUD();
		CalculateNextSpawn();
	}
	
	void UpdateHUD()
	{
		scoreLabel.text = score.ToString();
		waveLabel.text = waveNumber.ToString();
	}
	
	void SendNextWave(GameObject btn)
	{
		if(!wavesIncoming) ChangeNextWaveLabel("Incoming!");
		
		if(!spawningEnemies)
		{
			spawningEnemies = true;
			SetNextWave();
			GetNextWave();
			StartNextWave();
		}
	}
	
	public void gemPanelLockToFalse()
	{
		gemPanelLock = false;
	}
	
	public void gemPanelLockToTrue()
	{
		gemPanelLock = true;
	}
	
	public void ChangeNextWaveLabel(string n)
	{
		nextWaveLabel.text = n;
	}
	
	public void resqGem()
	{
		gemCounter++;
		gemsInGame++;
		candyshop.returnGem();
		Destroy(activeGem);
		DisableActiveGem();
		gtbd = Time.time + gemTakeBackDelay;
	}
	
	public void DropGem(Transform pos)
	{
		Instantiate(gemPrefab, pos.position, pos.rotation);
	}
	
	public void StealGem()
	{
		gemsInGame--;
	}
	
	public void GameOver()
	{
		wavesIncoming = false;
		gameIsOver = true;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BuildingScript>().enabled = false;
		GameObject.FindGameObjectWithTag("CameraControler").GetComponent<CameraControler>().enabled = false;

		if(turnOffGUI)
		{
			turnOffGUI = false;
			GameObject[] gui = GameObject.FindGameObjectsWithTag("InGameGUI");
			foreach(GameObject g in gui) g.GetComponent<UIPanel>().enabled = !g.GetComponent<UIPanel>().enabled;
		}
		waitForExit = true;
	}
	
	public void GameWin()
	{
		gameIsOver = true;
		
		GameOverLabel.text = "VICTORY!";
		
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BuildingScript>().enabled = false;
		GameObject.FindGameObjectWithTag("CameraControler").GetComponent<CameraControler>().enabled = false;

		if(turnOffGUI)
		{
			turnOffGUI = false;
			GameObject[] gui = GameObject.FindGameObjectsWithTag("InGameGUI");
			foreach(GameObject g in gui) g.GetComponent<UIPanel>().enabled = !g.GetComponent<UIPanel>().enabled;
		}
		waitForExit = true;
	}
	
	public void DestroyEnemy()
	{
		totalEnemyCounter--;
		Debug.Log(">\ttotalEnemyCounter: " + totalEnemyCounter);
		if(totalEnemyCounter <= 0 && waveNumber >= waves.Length - 1 && !spawningEnemies) victory = true;
	}
}
