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

	private enum GameplayState
	{
		WAITING,
		IN_PROGRESS,
		LEVEL_END,
		END,
	}

	//	Properties
	private int											__gems					= 5;
	private int                                         __enemiesOnBoard        = 0;
	private int											__score;
	private int											__wave;
	private GameplayState								__gameplay				= GameplayState.WAITING;

	private int __EnemiesOnBoard
	{
		get
		{
			return __enemiesOnBoard;
		}
		set
		{
			__enemiesOnBoard = value;

			if(__enemiesOnBoard == 0 && (__gameplay == GameplayState.LEVEL_END || WaveNumber >= waves.levelWaves.Length))
				__EndGame(true);
		}
	}

	
	public int WaveNumber
	{
		get
		{
			return __wave;
		}
		set
		{
			__wave = value;

			GUIManager.Instance.Wave = __wave;
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
			__EnemiesOnBoard--;

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
			return (TimeManager.Instance.gameplayState && __gameplay == GameplayState.IN_PROGRESS);
		}
	}


	void Update()
	{
		if(!ready)
			Initialize();

		if(TimeManager.Instance.gameplayState && __gameplay == GameplayState.IN_PROGRESS)
		{
			if(__spawning)
			{
				if(__nextSpawnTime <= 0f)
					__SpawnEnemy();
				else
					__nextSpawnTime -= Time.deltaTime;
			}
			else
			{
				__nextWaveTime -= Time.deltaTime;

				if(__nextWaveTime <= 0f)
				{
					if(WaveNumber >= waves.levelWaves.Length)
					{
						__gameplay = GameplayState.LEVEL_END;
						return;
					}

					StartNextWave();
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
			__nextWaveTime = waveDelay;
			__nextSpawnTime = 0f;
			__spawning = true;
			__gameplay = GameplayState.IN_PROGRESS;

			WaveNumber++;
		}
		else
			__EndGame();
	}

	private void __SpawnEnemy()
	{
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
		__EnemiesOnBoard++;
	}

	public void EnemyDestroyed(GameObject enemy, bool gem)
	{
		if(gem)
		{
			GameObject gemGO = GlobalObjectPoolManager.Instance.GetGameObject(gemPrefab);
			gemGO.transform.position = enemy.transform.position;
			gemGO.SetActive(true);
		}

		__EnemiesOnBoard--;
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
		__gameplay = GameplayState.LEVEL_END;
		Debug.Log("Game over, status: " + (victory ? "victory" : "defeat"));
	}
}
