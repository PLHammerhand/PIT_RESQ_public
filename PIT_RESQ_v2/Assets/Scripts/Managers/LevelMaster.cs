using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMaster : Singleton<LevelMaster>
{
	public Transform            candyshopPosition;
	public List<Wave>           waves;
	public GameObject           gemPrefab;

	private Candyshop			__candyshop;
	
	//	Properties
	private int                 __gems					= 5;
	private int					__score;
	private int					__wave;
	
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

			//	TODO:	check if score counting works
			//GUIManager.Instance.Score = __score;
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


	void Update()
	{
		if(!ready)
			Initialize();
	}

	public void Initialize()
	{
		if(GUIManager.Instance.ready)
		{
			__CalculateMaxEnemyTypes();

			gemPrefab = Resources.Load("Gem") as GameObject;
			GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(gemPrefab, Gems);

			__candyshop = GameObject.FindObjectOfType<Candyshop>();
			__candyshop.gems = __gems;
			candyshopPosition = __candyshop.gameObject.transform;

			ready = true;
		}
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

		foreach(Wave w in waves.enemies)
		{
			int outInt;

			if(maxEnemies.TryGetValue(w.enemy, out outInt))
			{
				if(outInt < w.enemyCount)
					outInt = w.enemyCount;
			}
			else
			{
				maxEnemies.Add(w.enemy, w.enemyCount);
			}
		}

		foreach(GameObject key in maxEnemies.Keys)
			GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(key, maxEnemies[key]);
	}

	private void __EndGame(bool victory = false)
	{
		Debug.Log("Game over, status: " + (victory ? "victory" : "defeat"));
	}
}
