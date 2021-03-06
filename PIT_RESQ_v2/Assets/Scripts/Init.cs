﻿using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour
{
	public int                  startingMoney               = 500;
	public int                  courierCooldown				= 30;

	public LayerMask            clickLayers;
	public LayerMask            constructionLayer;

	void Start()
	{
		TimeManager.Instance.Initialize();
		CostsManager.Instance.Initialize();
		InputManager.Instance.Initialize();

		LevelMaster.Instance.Initialize();

		BuildingManager.Instance.Initialize();

		GUIManager.Instance.Initialize();
	}

	void Update()
	{
		if(GUIManager.Instance.ready)
		{
			__StartGame();

			Destroy(gameObject);
		}
	}

	private void __StartGame()
	{
		BuildingManager.Instance.Money = startingMoney;
		BuildingManager.Instance.constructionLayer = constructionLayer;

		InputManager.Instance.clickLayers = clickLayers;

		GUIManager.Instance.courierCooldown = courierCooldown;
	}
}
