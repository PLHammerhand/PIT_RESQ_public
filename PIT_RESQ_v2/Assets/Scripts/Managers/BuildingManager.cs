using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : Singleton<BuildingManager>
{
	public GameObject                       alienTowerPrefab;
	public GameObject                       mageTowerPrefab;
	public GameObject                       robotTowerPrefab;

	public LayerMask                        constructionLayer;
	public Tower                            tower;
	[Range(30f, 100f)]
	public float                            rayRange                        = 30f;
	public float                            alienHeight                     = 30f;

	public List<ConstructionPosition>       constructionPositions;

	private BuildingState                   __buildingState                 = BuildingState.GAMEPLAY;
	private Ray                             __ray;
	private RaycastHit                      __hit;
	private ConstructionPosition            __positionToBuild;

	private GameObject                      __alienBuildParticles;
	private GameObject                      __mageBuildParticles;

	public BuildingState BuildingState
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

	public void Initialize()
	{
		constructionPositions.AddRange(GameObject.FindObjectsOfType<ConstructionPosition>());

		//__alienBuildParticles = Resources.Load("Particles/Build/Alien") as GameObject;
		__mageBuildParticles = Resources.Load("Particles/Build/Mage") as GameObject;

		alienTowerPrefab = Resources.Load("Towers/AlienTower") as GameObject;
		mageTowerPrefab = Resources.Load("Towers/MageTower") as GameObject;
		robotTowerPrefab = Resources.Load("Towers/RobotTower") as GameObject;

		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(alienTowerPrefab, 7);
		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(mageTowerPrefab, 7);
		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(robotTowerPrefab, 7);

		//GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(__alienBuildParticles, 2);
		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(__mageBuildParticles, 2);
	}

	public void ShowTowerBuildParticles(GameObject go)
	{
		if(go.GetComponent<BaseTower>() is AlienTower)
		{
			Debug.Log("Zium");
		}
		else if(go.GetComponent<BaseTower>() is MageTower)
		{
			Debug.Log("Whom");
		}
		else if(go.GetComponent<BaseTower>() is RobotTower)
		{
			Debug.Log("Bzyt");
		}
	}

	void Update()
	{
		if(BuildingState == BuildingState.CONSTRUCTION)
		{
			if(Input.GetButtonDown("Click"))
			{
				__ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if(Physics.Raycast(__ray, out __hit, rayRange, constructionLayer))
				{
					GameObject go = GlobalObjectPoolManager.Instance.GetGameObject(__GetTower());
					__positionToBuild = __hit.transform.gameObject.GetComponent<ConstructionPosition>();
					__positionToBuild.ConstructedTower = go.GetComponent<BaseTower>();
					__BuildTower(go);
				}
			}
		}
	}

	private void __BuildTower(GameObject go)
	{
		switch(tower)
		{
			case Tower.ALIEN:
				go.transform.position = __positionToBuild.transform.position + new Vector3(0f, alienHeight, 0f);
				go.SetActive(true);
				__BuildAlienTower(go);
				break;
			case Tower.MAGE:
				go.transform.position = __positionToBuild.transform.position;
				go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				go.SetActive(true);
				__BuildMageTower(go);
				break;
			case Tower.ROBOT:
				go.transform.position = __positionToBuild.transform.position;
				go.SetActive(true);
				break;
		}

		BuildingState = BuildingState.GAMEPLAY;
	}

	private GameObject __GetTower()
	{
		switch(tower)
		{
			case Tower.ALIEN:
				return alienTowerPrefab;
			case Tower.MAGE:
				return mageTowerPrefab;
			case Tower.ROBOT:
				return robotTowerPrefab;
			default:
				return robotTowerPrefab;
		}
	}

	private void __BuildAlienTower(GameObject go)
	{
		Hashtable args = new Hashtable();
		args.Add("position", __positionToBuild.transform.position);
		args.Add("time", (alienHeight / 100f + 0.05f));
		args.Add("easetype", iTween.EaseType.linear);
		args.Add("oncomplete", "ShowTowerBuildParticles");
		args.Add("oncompletetarget", Instance);
		args.Add("oncompleteparams", go.gameObject);

		iTween.MoveTo(go, args);
	}

	private void __BuildMageTower(GameObject go)
	{
		iTween.ScaleTo(go, new Vector3(1f, 1f, 1f), 0.75f);

		ShowTowerBuildParticles(go);
	}

	private void __BuildRobotTower(GameObject go)
	{
		ShowTowerBuildParticles(go);
	}
}
