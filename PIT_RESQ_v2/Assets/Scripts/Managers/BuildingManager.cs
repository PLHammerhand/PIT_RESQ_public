using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : Singleton<BuildingManager>
{
	public int                              maxRobotTowerLevel					= 3;

	public GameObject                       alienTowerPrefab;
	public GameObject                       mageTowerPrefab;
	public List<GameObject>                 robotTowerPrefabs;

	public LayerMask                        constructionLayer;
	public Tower                            tower;
	[Range(30f, 100f)]
	public float                            alienHeight							= 30f;

	public List<ConstructionPosition>       constructionPositions;

	private BuildingState                   __buildingState						= BuildingState.GAMEPLAY;
	private ConstructionPosition            __currentConstructionPosition;

	private int                             __money								= 500;
	private GameObject                      __alienBuildParticles;
	private GameObject                      __mageBuildParticles;
	private GameObject						__alienTowerUpgradeVisuals;
	private GameObject						__mageTowerUpgradeVisuals;
	private GameObject						__robotTowerUpgradeVisuals;
	private Dictionary<Tower, int>          __towersCount;

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

	public int Money
	{
		get
		{
			return __money;
		}
		set
		{
			__money = value;

			GUIManager.Instance.Money = __money;
		}
	}

	public ConstructionPosition ClickedPosition
	{
		get
		{
			return __currentConstructionPosition;
		}
	}

	public Dictionary<Tower, int> TowersCount
	{
		get
		{
			return __towersCount;
		}
	}


	void Awake()
	{
		constructionPositions = new List<ConstructionPosition>();
	}

	public override void Initialize()
	{
		robotTowerPrefabs = new List<GameObject>();
		__towersCount = new Dictionary<Tower, int>();
		__towersCount.Add(Tower.ALIEN, 0);
		__towersCount.Add(Tower.MAGE, 0);
		__towersCount.Add(Tower.ROBOT, 0);

		constructionPositions.AddRange(GameObject.FindObjectsOfType<ConstructionPosition>());

		//__alienBuildParticles = Resources.Load("Particles/Build/Alien") as GameObject;
		__mageBuildParticles = Resources.Load("Particles/Build/Mage") as GameObject;

		alienTowerPrefab = Resources.Load("Towers/AlienTower") as GameObject;
		mageTowerPrefab = Resources.Load("Towers/MageTower") as GameObject;
		robotTowerPrefabs.Add(Resources.Load("Towers/RobotTower") as GameObject);

		int i = 1;

		while(i < maxRobotTowerLevel)
		{
			i++;
			robotTowerPrefabs.Add(Resources.Load("Towers/Upgrades/Robot/RobotTower_" + i) as GameObject);
		}

		//__alienTowerUpgradeVisuals = Instantiate(Resources.Load("Towers/Upgrades/Visuals/AlienUpgrade") as GameObject);
		//__mageTowerUpgradeVisuals = Instantiate(Resources.Load("Towers/Upgrades/Visuals/MageUpgrade") as GameObject);
		//__robotTowerUpgradeVisuals = Instantiate(Resources.Load("Towers/Upgrades/Visuals/RobotUpgrade") as GameObject);

		//__alienTowerUpgradeVisuals.SetActive(false);
		//__mageTowerUpgradeVisuals.SetActive(false);
		//__robotTowerUpgradeVisuals.SetActive(false);

		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(alienTowerPrefab, 2);
		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(mageTowerPrefab, 2);

		foreach(GameObject go in robotTowerPrefabs)
			GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(go, 2);

		//GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(__alienBuildParticles, 2);
		GlobalObjectPoolManager.Instance.CreateMultipleObjectsInPool(__mageBuildParticles, 2);
	}

	public void ShowTowerBuildParticles(GameObject go = null)
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

	public void ConstructionClick(RaycastHit hit)
	{
		__currentConstructionPosition = hit.transform.gameObject.GetComponent<ConstructionPosition>();

		if(__currentConstructionPosition.ConstructedTower == null)
		{
			GameObject go = GlobalObjectPoolManager.Instance.GetGameObject(__GetTower());
			__currentConstructionPosition.ConstructedTower = go.GetComponent<BaseTower>();
			__BuildTower(go);
		}
	}

	public void CheckPositionStatus(RaycastHit hit)
	{
		ConstructionPosition constPos = hit.collider.GetComponent<ConstructionPosition>();

		if(BuildingState == BuildingState.UPGRADE && constPos == __currentConstructionPosition)
		{
			BuildingState = BuildingState.GAMEPLAY;
			GUIManager.Instance.CloseUpgradePanels();
		}
		else if(constPos.ConstructedTower != null)
		{
			//GameObject visuals;
			__currentConstructionPosition = constPos;
			__currentConstructionPosition.Renderer = true;
			BuildingState = BuildingState.UPGRADE;

			if(constPos.ConstructedTower is AlienTower)
			{
				//visuals = __alienTowerUpgradeVisuals;
				GUIManager.Instance.OpenUpgradePanel(Tower.ALIEN);
			}
			else if(constPos.ConstructedTower is MageTower)
			{
				//visuals = __mageTowerUpgradeVisuals;
				GUIManager.Instance.OpenUpgradePanel(Tower.MAGE);
			}
			else
			{
				//visuals = __robotTowerUpgradeVisuals;
				GUIManager.Instance.OpenUpgradePanel(Tower.ROBOT);
			}

			//visuals.transform.position = constPos.transform.position;
			//visuals.SetActive(true);
		}
	}

	public void UpgradeAlienTower(TowerAttribute arg)
	{
		AlienTower clickedTower = BuildingManager.Instance.ClickedPosition.ConstructedTower as AlienTower;
		int cost = 0;

		switch(arg)
		{
			case TowerAttribute.DAMAGE:
				cost = (clickedTower.DamageLevel) * CostsManager.Instance.alienDamageUpgrade;
				(__currentConstructionPosition.ConstructedTower as AlienTower).DamageLevel++;
				break;
			case TowerAttribute.LASER:
				cost = (clickedTower.LaserCount) * CostsManager.Instance.alienLaserUpgrade;
				(__currentConstructionPosition.ConstructedTower as AlienTower).AddLaser();
				break;
			case TowerAttribute.RANGE:
				cost = (clickedTower.RangeLevel) * CostsManager.Instance.alienRangeUpgrade;
				(__currentConstructionPosition.ConstructedTower as AlienTower).RangeLevel++;
				break;
		}

		Money -= cost;
	}

	public void UpgradeMageTower(bool upgradeToLeft)
	{
		GameObject go;
		GameObject prefab;

		if(upgradeToLeft)
			prefab = (__currentConstructionPosition.ConstructedTower as MageTower).leftUpgrade;
		else
			prefab = (__currentConstructionPosition.ConstructedTower as MageTower).rightUpgrade;

        go = Instantiate(prefab) as GameObject;
		go.transform.position = __currentConstructionPosition.ConstructedTower.transform.position;

		__currentConstructionPosition.ConstructedTower.gameObject.SetActive(false);
		__currentConstructionPosition.ConstructedTower = go.GetComponent<BaseTower>();

		GlobalObjectPoolManager.Instance.AddGameObject(prefab, go);

		go.SetActive(true);

		Money -= ((BuildingManager.Instance.ClickedPosition.ConstructedTower as MageTower).level + 1) * CostsManager.Instance.mageUpgrade;
	}

	public void UpgradeRobotTower()
	{
		if(RobotTower.Level < maxRobotTowerLevel)
		{
			RobotTower[] allRobotTowers = GameObject.FindObjectsOfType<RobotTower>();

			foreach(RobotTower rt in allRobotTowers)
			{
				GameObject go = GlobalObjectPoolManager.Instance.GetGameObject(robotTowerPrefabs[RobotTower.Level]);
				go.transform.position = rt.transform.position;
				rt.towerPosition.ConstructedTower = go.GetComponent<BaseTower>();
				rt.gameObject.SetActive(false);
				go.SetActive(true);

				if(!GlobalObjectPoolManager.Instance.CheckIfPooled(robotTowerPrefabs[RobotTower.Level], go))
					GlobalObjectPoolManager.Instance.AddGameObject(robotTowerPrefabs[RobotTower.Level], go);
			}

			RobotTower.Level++;
		}

		Money -= CostsManager.Instance.robotUpgrade * RobotTower.Level;
	}

	private void __BuildTower(GameObject go)
	{
		Tower towerToBuild = Tower.ALIEN;

		switch(tower)
		{
			case Tower.ALIEN:
				towerToBuild = Tower.ALIEN;
				Money -= __towersCount[towerToBuild] * CostsManager.Instance.towerCountCost + CostsManager.Instance.baseTower;
				go.transform.position = __currentConstructionPosition.transform.position + new Vector3(0f, alienHeight, 0f);
				go.SetActive(true);
				__BuildAlienTower(go);
				break;
			case Tower.MAGE:
				towerToBuild = Tower.MAGE;
				Money -= __towersCount[towerToBuild] * CostsManager.Instance.towerCountCost + CostsManager.Instance.baseTower;
				go.transform.position = __currentConstructionPosition.transform.position;
				go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				go.SetActive(true);
				__BuildMageTower(go);
				break;
			case Tower.ROBOT:
				towerToBuild = Tower.ROBOT;
				Money -= CostsManager.Instance.baseTower + (RobotTower.Level - 1) * CostsManager.Instance.robotUpgrade;
				go.transform.position = __currentConstructionPosition.transform.position;
				go.SetActive(true);
				__BuildRobotTower(go);
				break;
		}

		__towersCount[towerToBuild]++;
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
				return robotTowerPrefabs[RobotTower.Level - 1];
			default:
				return robotTowerPrefabs[RobotTower.Level - 1];
		}
	}

	private void __BuildAlienTower(GameObject go)
	{
		Hashtable args = new Hashtable();
		args.Add("position", __currentConstructionPosition.transform.position);
		args.Add("time", (alienHeight / 100f + 0.05f));
		args.Add("easetype", iTween.EaseType.linear);
		args.Add("oncomplete", "ShowTowerBuildParticles");
		args.Add("oncompletetarget", Instance.gameObject);
		args.Add("oncompleteparams", go);

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
