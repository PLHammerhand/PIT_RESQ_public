using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : Singleton<BuildingManager>
{
	public GameObject						firerateBoostParticles;
	public GameObject						damageBoostParticles;
	public GameObject						rangeBoostParticles;

	public GameObject                       alienTowerPrefab;
	public GameObject                       mageTowerPrefab;
	public GameObject                       robotTowerPrefab;

	public Tower                            tower;

	public List<ConstructionPosition>       constructionPositions;

	private BuildingState					__buildingState					= BuildingState.GAMEPLAY;

	public BuildingState					BuildingState
	{
		get
		{
			return __buildingState;
		}
		set
		{
			__buildingState = value;
			
			if(__buildingState == BuildingState.CONSTRUCTION)
			{
				foreach(ConstructionPosition position in constructionPositions)
					position.Renderer = true;
			}
			else
			{
				foreach(ConstructionPosition position in constructionPositions)
					position.Renderer = false;

				if(__buildingState == BuildingState.UPGRADE)
				{
					//	TODO
				}
			}
		}
	}
		
	void Awake()
	{
		constructionPositions = new List<ConstructionPosition>();
	}

	void Start()
	{
		constructionPositions.AddRange(GameObject.FindObjectsOfType<ConstructionPosition>());

		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(alienTowerPrefab, 5);
		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(mageTowerPrefab, 5);
		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(robotTowerPrefab, 5);
	}
	
	void Update()
	{
		if(BuildingState == BuildingState.CONSTRUCTION)
		{

		}
	}
}
