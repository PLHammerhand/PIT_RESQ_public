using UnityEngine;
using System.Collections;

public class BuildingScript : MonoBehaviour
{
	//		=========	INFOPANEL	=========

	public UIPanel infoPanel;

	public UILabel towerNameLabel;
	public UILabel towerSummaryLabel;
	public UILabel towerDamageLabel;
	public UILabel towerRangeLabel;
	public UILabel towerFirerateLabel;

	//		=========	OTHER	=========

	public UILabel moniesLabel;
	public UILabel RobotCostLabel;
	public UILabel MageCostLabel;
	public UILabel AlienCostLabel;
	
	public int money;
	public CostsScript costs;

	public UIPanel alienUpgradePanel;
	public UILabel alienLaserUpgradeLabel;
	public UILabel alienDamageUpgradeLabel;
	public UILabel alienRangeUpgradeLabel;

	public UIPanel mageUpgradePanel;
	public UILabel mageLeftUpgradeLabel;
	public UILabel mageRightUpgradeLabel;

	public UIPanel robotUpgradePanel;
	public UILabel robotUpgradeLabel;

	public GameObject MageTowerBuildParticles;

	public UIButton alienDamageUpgradeButton;
	public UIButton alienLaserUpgradeButton;
	public UIButton alienRangeUpgradeButton;

	public UIButton robotUpgradeButton;
	public UIButton mageLeftUpgradeButton;
	public UIButton mageRightUpgradeButton;
	
	public int maxAlienLaserLevel;
	public int maxAlienDamageLevel;
	public int maxalienRangeLevel;
	
	public Material hover;
	public LayerMask placementLayer;
	public GameObject[] buildList;
	public GameObject[] robotTowerBuildList;
	public UISprite[] buttons;

	public float fallingSpeed = 50f;

	// private
	private Material original;
	private GameObject lastObject;
	private int index = 0;
	private GameObject[] buildPlanes;
	private bool buildMode = false;
	private GameObject buildModeButton;
	private int cost;
	private float startTime = 0;
	private bool startTimeSet = false;
	private bool placeAlienTower = false;
	private GameObject clone;
	private bool enableUpgradeMode = true;
	private bool robotUpgraded = false;

	private PositionScript upgradePosScript;
	private GameObject upgradeClone;
	private bool alienUpgradePanelOpen = false;
	private bool mageUpgradePanelOpen = false;
	private bool robotUpgradePanelOpen = false;
	private AlienUpgradeLevels aul;
	private int upgradeCloneLevel;

	private ArrayList robotTowers;
	private int robotTowerLevel = 0;
	private int robotTowerCost;

	private TowerInfo alienInfo;
	private TowerInfo robotInfo;
	private TowerInfo mageInfo;

	void Start()
	{
		robotTowers = new ArrayList();

		alienUpgradePanel.GetComponent<TweenPosition>().Play(false);
		mageUpgradePanel.GetComponent<TweenPosition>().Play(false);
		robotUpgradePanel.GetComponent<TweenPosition>().Play(false);

		alienInfo = new TowerInfo(buildList[2].GetComponent<AliensTowerScript>().towerDamage, buildList[2].GetComponent<CapsuleCollider>().radius, -1f);
		mageInfo = new TowerInfo(buildList[1].GetComponent<MageTowerScript>().towerDamage, buildList[1].GetComponent<CapsuleCollider>().radius, buildList[1].GetComponent<MageTowerScript>().reloadTime);
		robotInfo = new TowerInfo(buildList[0].GetComponent<RoboticsTurretScript>().towerDamage, buildList[0].GetComponent<CapsuleCollider>().radius, buildList[0].GetComponent<RoboticsTurretScript>().reloadTime);

		robotTowerCost = costs.towerCosts;

		index = 0;
		buildPlanes = GameObject.FindGameObjectsWithTag("PositionOpen");
		showMoney();
		UpdateTowerCost();
	}
	
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(buildMode)
		{
			if(Physics.Raycast(ray, out hit, 30f, placementLayer))
			{
				if(lastObject) lastObject.GetComponent<Renderer>().material = original;

				lastObject = hit.collider.gameObject;
				original = lastObject.GetComponent<Renderer>().material;
				lastObject.GetComponent<Renderer>().material = hover;
			}
			else
			{
				if(lastObject)
				{
					lastObject.GetComponent<Renderer>().material = original;
					lastObject = null;
				}
			}

			if(Input.GetMouseButtonDown(0) && lastObject)
			{
				if(lastObject.tag == "PositionOpen")
				{
					if(money >= cost)
					{
						Vector3 buildPos = lastObject.transform.position;

						switch(index)
						{
						case 0:		//	Robot Tower
							clone = Instantiate(robotTowerBuildList[robotTowerLevel],lastObject.transform.position, Quaternion.identity) as GameObject;
							robotTowers.Add(clone);
							clone.SendMessage("SetUpgradePanel", robotUpgradePanel, SendMessageOptions.DontRequireReceiver);
							break;
						case 1:		//	Mage Tower
							buildPos.y += 1.5f;
							Instantiate(MageTowerBuildParticles, buildPos, Quaternion.identity);
							clone = Instantiate(buildList[index],lastObject.transform.position, Quaternion.identity) as GameObject;
							clone.SendMessage("SetUpgradePanel", mageUpgradePanel, SendMessageOptions.DontRequireReceiver);
							break;
						case 2:		// Alien Tower
							buildPos.y = 100;
							clone = Instantiate(buildList[index], buildPos, Quaternion.identity) as GameObject;
							clone.SendMessage("SetUpgradePanel", alienUpgradePanel, SendMessageOptions.DontRequireReceiver);
							placeAlienTower = true;
							break;
						}

						lastObject.SendMessage("BuildTower", clone, SendMessageOptions.DontRequireReceiver);

						money -= cost;
						showMoney();
						lastObject.tag = "PositionTaken";
						foreach(GameObject myPlane in buildPlanes) myPlane.GetComponent<Renderer>().enabled = false;
						buildMode = false;
					}
					else Debug.Log("No monies!");
				}
			}
		}
		else
		{
			if(Physics.Raycast(ray, out hit, 30f, placementLayer) && enableUpgradeMode && Input.GetMouseButtonDown(0))
			{
				if(hit.collider.gameObject != null)
				{
					lastObject = hit.collider.gameObject;
					upgradePosScript = lastObject.GetComponent<PositionScript>();
					if(upgradePosScript.IsTowerBuild())
					{
						upgradeClone = upgradePosScript.GetTower();

						if(upgradePosScript.GetTowerType() == 0 && !robotUpgradePanelOpen)
						{
							if(upgradeClone.GetComponent<RoboticsTurretScript>().upgradeTowerPrefab != null)
							{
								upgradeCloneLevel = upgradeClone.GetComponent<RoboticsTurretScript>().towerLevel;
								robotUpgradePanelOpen = true;
							}
							else robotUpgradePanelOpen = false;
							mageUpgradePanelOpen = false;
							alienUpgradePanelOpen = false;
						}
						else if(upgradePosScript.GetTowerType() == 1 && !mageUpgradePanelOpen)
						{
							if(upgradeClone.GetComponent<MageTowerScript>().leftUpgradeTowerPrefab != null || upgradeClone.GetComponent<MageTowerScript>().rightUpgradeTowerPrefab != null)
							{
								upgradeCloneLevel = upgradeClone.GetComponent<MageTowerScript>().towerLevel;
								mageUpgradePanelOpen = true;
							}
							else mageUpgradePanelOpen = false;
							robotUpgradePanelOpen = false;
							alienUpgradePanelOpen = false;
						}
						else if(upgradePosScript.GetTowerType() == 2 && !alienUpgradePanelOpen)
						{
							aul = upgradeClone.GetComponent<AliensTowerScript>().GetTowerStatus();

							alienUpgradePanelOpen = true;
							robotUpgradePanelOpen = false;
							mageUpgradePanelOpen = false;
						}
						else
						{
							CloseAllPanels();
							upgradeClone = null;
						}
					}
					else
					{
						CloseAllPanels();
						upgradeClone = null;
					}
				}
			}
		}

		if(placeAlienTower) BuildAlienTower();
		else clone = null;

		if(robotUpgradePanelOpen)
		{
			robotUpgradeLabel.text = "Cost: " + ((int)(costs.robotTowerUpgradeCost * upgradeClone.GetComponent<RoboticsTurretScript>().towerLevel * costs.robotTowerUpgradeMultiplayer));
			if(money < costs.robotTowerUpgradeCost * upgradeClone.GetComponent<RoboticsTurretScript>().towerLevel * costs.robotTowerUpgradeMultiplayer)
				robotUpgradeButton.isEnabled = false;
			else robotUpgradeButton.isEnabled = true;

			robotUpgradePanel.GetComponent<TweenPosition>().Play(true);
		}
		else robotUpgradePanel.GetComponent<TweenPosition>().Play(false);

		if(mageUpgradePanelOpen)
		{
			mageLeftUpgradeLabel.text = "Cost: " + ((int)(upgradeClone.GetComponent<MageTowerScript>().towerLevel * costs.mageTowerUpgradeLeftCost * costs.mageTowerUpgradeMultiplayer));
			if(money < upgradeClone.GetComponent<MageTowerScript>().towerLevel * costs.mageTowerUpgradeLeftCost * costs.mageTowerUpgradeMultiplayer)
				mageLeftUpgradeButton.isEnabled = false;
			else mageLeftUpgradeButton.isEnabled = true;

			mageRightUpgradeLabel.text = "Cost: " + ((int)(upgradeClone.GetComponent<MageTowerScript>().towerLevel * costs.mageTowerUpgradeRightCost * costs.mageTowerUpgradeMultiplayer));
			if(money < upgradeClone.GetComponent<MageTowerScript>().towerLevel * costs.mageTowerUpgradeRightCost * costs.mageTowerUpgradeMultiplayer)
				mageRightUpgradeButton.isEnabled = false;
			else mageRightUpgradeButton.isEnabled = true;
			mageUpgradePanel.GetComponent<TweenPosition>().Play(true);
		}
		else mageUpgradePanel.GetComponent<TweenPosition>().Play(false);

		if(alienUpgradePanelOpen)
		{
			alienLaserUpgradeLabel.text = "Cost: " + ((int)(aul.laserLvl * costs.alienLaserUpgradeAdditive) + costs.alienLaserUpgradeCost);
			alienDamageUpgradeLabel.text = "Cost: " + ((int)(aul.damageLvl * costs.alienDamageUpgradeAdditive) + costs.alienDamageUpgradeCost);
			alienRangeUpgradeLabel.text = "Cost: " + ((int)(aul.rangeLvl * costs.alienRangeUpgradeAdditivie) + costs.alienRangeUpgradeCost);

			if(aul.laserLvl >= maxAlienLaserLevel || money < (costs.alienLaserUpgradeCost + costs.alienLaserUpgradeAdditive * aul.laserLvl))
				alienLaserUpgradeButton.isEnabled = false;
			else alienLaserUpgradeButton.isEnabled = true;
			if(aul.damageLvl >= maxAlienDamageLevel || money < costs.alienDamageUpgradeCost + costs.alienDamageUpgradeAdditive * aul.damageLvl)
				alienDamageUpgradeButton.isEnabled = false;
			else alienDamageUpgradeButton.isEnabled = true;
			if(aul.rangeLvl >= maxalienRangeLevel || money < costs.alienRangeUpgradeCost + costs.alienRangeUpgradeAdditivie * aul.rangeLvl)
				alienRangeUpgradeButton.isEnabled = false;
			else alienRangeUpgradeButton.isEnabled = true;

			alienUpgradePanel.GetComponent<TweenPosition>().Play(true);
		}
		else alienUpgradePanel.GetComponent<TweenPosition>().Play(false);
	}

	void LateUpdate()
	{
		if(robotUpgraded)
		{
			robotUpgraded = false;
			robotTowers.Clear();
			GameObject[] towers = GameObject.FindGameObjectsWithTag("RobotTower");
			ArrayList tmp = new ArrayList();
			foreach(GameObject t in towers) tmp.Add(t);
			robotTowers = tmp;
		}
	}

	void GetStartTime()
	{
		startTime = Time.time;
		startTimeSet = true;
	}

	void BuildAlienTower()
	{
		if(clone.transform.position.y > lastObject.transform.position.y) clone.transform.Translate(Vector3.down * Time.deltaTime * fallingSpeed);
		else 
		{
			clone.transform.Translate(0, (lastObject.transform.position.y - clone.transform.position.y), 0, Space.World);
			clone.SendMessage("DisableLight", null, SendMessageOptions.DontRequireReceiver);
			placeAlienTower = false;
			clone = null;
		}
	}

	void CloseAllPanels()
	{
		robotUpgradePanelOpen = false;
		mageUpgradePanelOpen = false;
		alienUpgradePanelOpen = false;
	}

	public void AlienTowerUpgrade(GameObject btn)
	{
		string btnName = btn.name;
		
		switch(btnName)
		{
		case "UpgradeLaser":
			ReduceMoney(costs.alienLaserUpgradeCost + costs.alienLaserUpgradeAdditive * aul.laserLvl);
			upgradeClone.SendMessage("UpgradeLaser", null, SendMessageOptions.DontRequireReceiver);
			break;
		case "UpgradeDamage":
			ReduceMoney(costs.alienDamageUpgradeCost + costs.alienDamageUpgradeAdditive * aul.damageLvl);
			upgradeClone.SendMessage("UpgradeDamage", null, SendMessageOptions.DontRequireReceiver);
			break;
		case "UpgradeRange":
			ReduceMoney(costs.alienRangeUpgradeCost + costs.alienRangeUpgradeAdditivie * aul.rangeLvl);
			upgradeClone.SendMessage("UpgradeRange", null, SendMessageOptions.DontRequireReceiver);
			break;
		}

		alienUpgradePanelOpen = false;
		upgradeClone = null;
	}

	public void MageTowerUpgrade(GameObject btn)
	{
		string btnName = btn.name;
		int upgradeCost;

		switch(btnName)
		{
		case "UpgradeLeft":
			upgradeCost = (int)(costs.mageTowerUpgradeLeftCost * (costs.mageTowerUpgradeMultiplayer * upgradeCloneLevel));
			ReduceMoney(upgradeCost);
			upgradeClone.SendMessage("UpgradeToLeft", lastObject, SendMessageOptions.DontRequireReceiver);
			upgradeClone = null;
			mageUpgradePanelOpen = false;
			break;
		case "UpgradeRight":
			upgradeCost = (int)(costs.mageTowerUpgradeRightCost * (costs.mageTowerUpgradeMultiplayer * upgradeCloneLevel));
			ReduceMoney(upgradeCost);
			upgradeClone.SendMessage("UpgradeToRight", lastObject, SendMessageOptions.DontRequireReceiver);
			upgradeClone = null;
			mageUpgradePanelOpen = false;
			break;
		}
	}

	public void RobotTowerUpgrade(GameObject btn)
	{
		string btnName = btn.name;

		switch(btnName)
		{
		case "Upgrade":
			int upgradeCost = (int)(costs.robotTowerUpgradeCost * (costs.robotTowerUpgradeMultiplayer * upgradeCloneLevel));
			ReduceMoney(upgradeCost);
			foreach(GameObject o in robotTowers)
				o.SendMessage("Upgrade", lastObject, SendMessageOptions.DontRequireReceiver);
			costs.robotTowerUpgradeCost = costs.robotTowerUpgradeCost * upgradeCloneLevel + costs.towerCosts;
			robotTowerCost = robotTowerCost * 2;
			RobotCostLabel.text = robotTowerCost.ToString();
			robotUpgraded = true;
			robotTowerLevel++;
			upgradeClone = null;
			robotUpgradePanelOpen = false;
			break;
		}
	}

	void showUpgradeInfo(GameObject btn)
	{
		string btnName = btn.name;

		switch(btnName)
		{
		case "Upgrade":
			towerNameLabel.text = "Robot machinegun";
			towerSummaryLabel.text = "Even faster shooting machinegun";
			towerDamageLabel.text = "" + robotInfo.getDamage();
			towerRangeLabel.text = "" + robotInfo.getRange();
			towerFirerateLabel.text = "" + robotInfo.getFirerate();
			infoPanel.enabled = true;
			break;
		case "UpgradeLeft":
			towerNameLabel.text = "Wizzard tower";
			towerSummaryLabel.text = "Upgrade to faster but weaker tower";
			towerDamageLabel.text = "" + robotInfo.getDamage();
			towerRangeLabel.text = "" + robotInfo.getRange();
			towerFirerateLabel.text = "" + robotInfo.getFirerate();
			infoPanel.enabled = true;
			break;
		case "UpgradeRight":
			towerNameLabel.text = "Sorcerer tower";
			towerSummaryLabel.text = "Upgrade to stronger but slower\ntower";
			towerDamageLabel.text = "" + robotInfo.getDamage();
			towerRangeLabel.text = "" + robotInfo.getRange();
			towerFirerateLabel.text = "" + robotInfo.getFirerate();
			infoPanel.enabled = true;
			break;
		case "UpgradeLaser":
			towerNameLabel.text = "Laser quantity";
			towerSummaryLabel.text = "Increase tower's laser quantity";
			towerDamageLabel.text = "" + robotInfo.getDamage();
			towerRangeLabel.text = "" + robotInfo.getRange();
			infoPanel.enabled = true;
			break;
		case "UpgradeDamage":
			towerNameLabel.text = "Damage upgrade";
			towerSummaryLabel.text = "Increase damage dealt";
			towerDamageLabel.text = "" + robotInfo.getDamage();
			towerRangeLabel.text = "" + robotInfo.getRange();
			infoPanel.enabled = true;
			break;
		case "UpgradeRange":
			towerNameLabel.text = "Range upgrade";
			towerSummaryLabel.text = "Extend tower's range";
			towerDamageLabel.text = "" + robotInfo.getDamage();
			towerRangeLabel.text = "" + robotInfo.getRange();
			infoPanel.enabled = true;
			break;
		}
	}

	void buildChoice(GameObject btn)
	{
		buildMode = !buildMode;
		if(buildModeButton)
		{
			if(buildModeButton == btn)
			{
				buildMode = false;
				buildModeButton = null;
			}
			else
			{
				buildMode = true;
				buildModeButton = btn;
			}
		}
		else
		{
			buildMode = true;
			buildModeButton = btn;
		}

		if(buildMode)
		{
			string btnName = btn.name;

			foreach(GameObject myPlane in buildPlanes) myPlane.GetComponent<Renderer>().enabled = true;

			switch(btnName)
			{
			case "RobotButton":
				CloseAllPanels();
				index = 0;
				cost = robotTowerCost;
				break;
			case "MageButton":
				CloseAllPanels();
				index = 1;
				cost = costs.getMageTowerCost();
				break;
			case "AlienButton":
				CloseAllPanels();
				index = 2;
				cost = costs.getAlienTowerCost();
				break;
			}
		}
		else foreach(GameObject myPlane in buildPlanes) myPlane.GetComponent<Renderer>().enabled = false;
		buildModeButton = null;
	}

	public void disableUM()
	{
		enableUpgradeMode = false;
	}

	public void enableUM()
	{
		enableUpgradeMode = true;
		infoPanel.enabled = false;
	}

	public void showAlienUpgradeInfo(GameObject btn)
	{
		towerNameLabel.text = "Alien laser";

		string btnName = btn.name;

		switch(btnName)
		{
		case "UpgradeDamage":
			towerSummaryLabel.text = "";
			towerDamageLabel.text = "" + (upgradeClone.GetComponent<AliensTowerScript>().towerDamage * 1.3f);
			towerRangeLabel.text = "" + upgradeClone.GetComponent<CapsuleCollider>().radius;
			break;
		case "UpgradeLaser":
			towerSummaryLabel.text = "Adds new laser";
			towerDamageLabel.text = "" + upgradeClone.GetComponent<AliensTowerScript>().towerDamage;
			towerRangeLabel.text = "" + upgradeClone.GetComponent<CapsuleCollider>().radius;
			break;
		case "UpgradeRange":
			towerSummaryLabel.text = "";
			towerDamageLabel.text = "" + upgradeClone.GetComponent<AliensTowerScript>().towerDamage;
			towerRangeLabel.text = "" + (upgradeClone.GetComponent<CapsuleCollider>().radius * 1.1f);
			break;
		}

		infoPanel.enabled = true;
	}

	public void showMageUpgradeInfo(GameObject btn)
	{
		towerNameLabel.text = "Mage Tower";
		
		string btnName = btn.name;

		MageTowerScript mts = upgradeClone.GetComponent<MageTowerScript>();

		switch(btnName)
		{
		case "UpgradeLeft":
			towerSummaryLabel.text = "Upgrade to faster but weaker tower";
			towerDamageLabel.text = "" + mts.leftUpgradeTowerPrefab.GetComponent<MageTowerScript>().towerDamage;
			towerRangeLabel.text = "" + mts.leftUpgradeTowerPrefab.GetComponent<CapsuleCollider>().radius;
			towerFirerateLabel.text = "" + mts.leftUpgradeTowerPrefab.GetComponent<MageTowerScript>().reloadTime;
			break;
		case "UpgradeRight":
			towerSummaryLabel.text = "Upgrade to stronger but slower tower";
			towerDamageLabel.text = "" + mts.rightUpgradeTowerPrefab.GetComponent<MageTowerScript>().towerDamage;
			towerRangeLabel.text = "" + mts.rightUpgradeTowerPrefab.GetComponent<CapsuleCollider>().radius;
			towerFirerateLabel.text = "" + mts.rightUpgradeTowerPrefab.GetComponent<MageTowerScript>().reloadTime;
			break;
		}
		
		infoPanel.enabled = true;
	}

	public void showRobotUpgradeInfo(GameObject btn)
	{
		towerNameLabel.text = "Robot machinegun";
		
		string btnName = btn.name;
	
		RoboticsTurretScript rts = upgradeClone.GetComponent<RoboticsTurretScript>();

		towerSummaryLabel.text = "";
		towerDamageLabel.text = "" + rts.towerDamage;
		towerRangeLabel.text = "" + upgradeClone.GetComponent<CapsuleCollider>().radius;
		towerFirerateLabel.text = "" + rts.reloadTime;

		infoPanel.enabled = true;
	}

	public void showAlienInfo()
	{
		towerNameLabel.text = "Alien tower";
		towerSummaryLabel.text = "Constantly hitting laser";
		towerDamageLabel.text = "" + alienInfo.getDamage();
		towerRangeLabel.text = "" + alienInfo.getRange();
		infoPanel.enabled = true;
	}
	
	public void showMageInfo()
	{
		towerNameLabel.text = "Mage tower";
		towerSummaryLabel.text = "Enemy seeking magic missle";
		towerDamageLabel.text = "" + mageInfo.getDamage();
		towerRangeLabel.text = "" + mageInfo.getRange();
		towerFirerateLabel.text = "" + mageInfo.getFirerate();
		infoPanel.enabled = true;
	}

	public void showRobotInfo()
	{
		towerNameLabel.text = "Robot tower";
		towerSummaryLabel.text = "Fast shooting machinegun";
		towerDamageLabel.text = "" + robotInfo.getDamage();
		towerRangeLabel.text = "" + robotInfo.getRange();
		towerFirerateLabel.text = "" + robotInfo.getFirerate();
		infoPanel.enabled = true;
	}

	public void hideInfoPanel()
	{
		infoPanel.enabled = false;
	}
	
	public void UpdateTowerCost()
	{
		AlienCostLabel.text = costs.getAlienTowerCost().ToString();
		MageCostLabel.text = costs.getMageTowerCost().ToString();
		RobotCostLabel.text = costs.getRobotTowerCost().ToString();
	}

	public void AddMoney(int mon)
	{
		money += mon;
		showMoney();
	}

	public void ReduceMoney(int mon)
	{
		money -= mon;
		showMoney();
	}

	public void showMoney()
	{
		moniesLabel.text = money.ToString();
	}
}


public class TowerInfo
{
	private float damage;
	private float range;
	private float firerate;

	public TowerInfo(float d, float r, float f)
	{
		damage = d;
		range = r;
		firerate = f;
	}

	public float getDamage()
	{
		return damage;
	}

	public float getRange()
	{
		return range;
	}

	public float getFirerate()
	{
		return firerate;
	}

	public void setDamage(float d)
	{
		damage = d;
	}

	public void setRange(float r)
	{
		range = r;
	}

	public void setFirerate(float f)
	{
		firerate = f;
	}
}
