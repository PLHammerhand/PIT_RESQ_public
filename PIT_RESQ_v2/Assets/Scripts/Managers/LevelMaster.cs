using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMaster : Singleton<LevelMaster>
{
	public float										spawnDelay				= 2f;
	public float										waveDelay				= 20f;
	public Transform									candyshopPosition;
	public Waves										waves;
	public EnemySpawn                                   enemySpawn;
	public GameObject								    gemPrefab;

	private int                                         __enemyTypeNumber		= 0;
	private int											__enemyNumber			= 0;
	private float									    __nextSpawnTime			= 0;
	private float									    __nextWaveTime			= 0;
	private bool										__spawning              = false;
	private Wave									    __currentWave;
	private Candyshop									__candyshop;
	private GameObject								    __enemyToSpawn;
	
	//	Properties
	private int											__gems					= 5;
	private int											__score;
	private int											__wave;
	private bool									    __gameplay				= false;
	
	public int WaveNumber
	{
		get
		{
			return __wave;
		}
		set
		{
			__wave = value;
		}
	}

	public int Score
	{
		get
		{
			return __score;
		}
		set
		{
			__score = value;

			GUIManager.Instance.Score = __score;
		}
	}

	public int Gems
	{
		get
		{
			return __gems;
		}
		set
		{
			__gems = value;

			if(__gems == 0)
			{
				__EndGame();
				TimeManager.Instance.gameplayState = false;
			}
		}
	}

	public bool Gameplay
	{
		get
		{
			return (TimeManager.Instance.gameplayState && __gameplay);
		}

		private set
		{
			__gameplay = value;

			if(TimeManager.Instance.gameplayState && __gameplay)
				GUIManager.Instance.Gameplay = true;
			else
				GUIManager.Instance.Gameplay = false;
		}
	}


	void Update()
	{
		if(!ready)
			Initialize();

		if(TimeManager.Instance.gameplayState && __gameplay)
		{
			if(__spawning)
			{
				Debug.Log("> Spawning...");
				if(__nextSpawnTime <= 0f)
					__SpawnEnemy();
				else
				{
					__nextSpawnTime -= Time.deltaTime;
					Debug.Log(">> Next spawn time countdown");
				}
			}
			else
			{
				Debug.Log("> Waiting for wave...");
				__nextWaveTime -= Time.deltaTime;

				if(__nextWaveTime <= 0f)
				{
					Debug.Log(">> Wave going!...");
					if(WaveNumber >= waves.levelWaves.Length - 1)
					{
						__gameplay = false;
						Debug.Log("End game!");
						return;
					}

					__spawning = true;
					__nextWaveTime = waveDelay;
					GUIManager.Instance.Wave++;
				}
				else
					GUIManager.Instance.NextWaveTime = (int)__nextWaveTime;
			}
		}
	}

	public override void Initialize()
	{
		if(GUIManager.Instance.ready)
		{
			gemPrefab = Resources.Load("Gem") as GameObject;
			GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(gemPrefab, Gems);

			waves = GameObject.FindObjectOfType<Waves>();
			enemySpawn = GameObject.FindObjectOfType<EnemySpawn>();
			__candyshop = GameObject.FindObjectOfType<Candyshop>();
			__candyshop.gems = __gems;
			candyshopPosition = __candyshop.gameObject.transform;

			__nextSpawnTime = spawnDelay;
			__nextWaveTime = waveDelay;

			//__CalculateMaxEnemyTypes();

			ready = true;
		}
	}

	public void StartNextWave()
	{
		if(WaveNumber < waves.levelWaves.Length)
		{
			__currentWave = waves.levelWaves[WaveNumber];
			__nextSpawnTime = spawnDelay;
			__spawning = true;
			__gameplay = true;

			WaveNumber++;
		}
		else
			__EndGame();
	}

	private void __SpawnEnemy()
	{
		Debug.Log(">> Spawning enemy...");
		__nextSpawnTime = spawnDelay;

		__enemyToSpawn = GlobalObjectPoolManager.Instance.GetGameObject(__currentWave.enemies[__enemyTypeNumber]);
		__enemyToSpawn.transform.position = enemySpawn.transform.position;
		__enemyToSpawn.transform.rotation = enemySpawn.transform.rotation;

		if(__enemyNumber >= __currentWave.enemiesCount[__enemyTypeNumber] - 1)
		{
			if(__enemyTypeNumber >= __currentWave.enemies.Length - 1)
			{
				__enemyNumber = 0;
				__enemyTypeNumber = 0;
				__spawning = false;
			}
			else
				__enemyTypeNumber++;
		}
		else
			__enemyNumber++;

		__enemyToSpawn.SetActive(true);
	}

	public void EnemyDestroyed(GameObject enemy, bool gem)
	{
		if(gem)
		{
			GameObject gemGO = GlobalObjectPoolManager.Instance.GetGameObject(gemPrefab);
			gemGO.transform.position = enemy.transform.position;
			gemGO.SetActive(true);
		}
	}

	private void __CalculateMaxEnemyTypes()
	{
		Waves waves = GameObject.FindObjectOfType<Waves>();

		if(waves == null)
		{
			Debug.LogError("!!! NO WAVES INFO FOUND !!!");
			return;
		}

		Dictionary<GameObject, int> maxEnemies = new Dictionary<GameObject, int>();

		foreach(Wave w in waves.levelWaves)
		{
			int outInt;

			for(int i = 0; i < w.enemies.Length; i++)
			{
				if(maxEnemies.TryGetValue(w.enemies[i], out outInt))
				{
					if(outInt < w.enemiesCount[i])
						outInt = w.enemiesCount[i];
				}
				else
					maxEnemies.Add(w.enemies[i], w.enemiesCount[i]);
			}
		}

		foreach(GameObject key in maxEnemies.Keys)
			GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(key, maxEnemies[key]);
	}

	private void __EndGame(bool victory = false)
	{
		__gameplay = false;
		Debug.Log("Game over, status: " + (victory ? "victory" : "defeat"));
	}
}
