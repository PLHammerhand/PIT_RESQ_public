using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : Singleton<GUIManager>
{
	//	TODO:	make it more readable

	//	--	Panels
	//	--	--	Info
	private UIPanel				__infoPanel;
	//	--	--	Superpowers
	private UIPanel				__superpowersPanel;
	//	--	--	General
	private UIPanel				__generalPanel;
	//	--	--	Upgrades
	private UIPanel				__alienUpgradePanel;
	private UIPanel				__mageUpgradePanel;
	private UIPanel				__robotUpgradePanel;

	//	--	Buttons
	//	--	--	Superpowers
	private UIButton			__courierButton;
	//	--	--	Upgrades
	private UIButton			__alienLaserUpgradeButton;
	private UIButton			__alienDamageUpgradeButton;
	private UIButton			__alienRangeUpgradeButton;
	private UIButton			__robotUpgradeButton;
	private UIButton			__mageRightUpgradeButton;
	private UIButton			__mageLeftUpgradeButton;

	//	--	Labels
	//	--	--	Info
	private UILabel				__scoreLabel;
	private UILabel				__waveLabel;
	private UILabel				__nextWaveLabel;
	private UILabel				__generalLabel;
	private UILabel				__courierTimer;
	//	--	--	Tower stats
	private UILabel				__towerNameLabel;
	private UILabel				__towerSummaryLabel;
	private UILabel				__towerDamageLabel;
	private UILabel				__towerRangeLabel;
	private UILabel				__towerFirerateLabel;
	//	--	--	Money
	private UILabel				__moniesLabel;
	//	--	--	Towers' costs
	private UILabel				__robotCostLabel;
	private UILabel				__mageCostLabel;
	private UILabel				__alienCostLabel;
	//	--	--	Upgrades
	private UILabel				__alienLaserUpgradeLabel;
	private UILabel				__alienDamageUpgradeLabel;
	private UILabel				__alienRangeUpgradeLabel;
	private UILabel				__mageRightUpgradeLabel;
	private UILabel				__mageLeftUpgradeLabel;
	private UILabel				__robotUpgradeLabel;

	//	--	Other
	//private 
	private UICamera			__uiCamera;
	private int					__money;
	private int                 __score;
	private int                 __wave;

	public int Wave
	{
		get
		{
			return __wave;
		}
		set
		{
			__wave = value;
			__waveLabel.text = "" + __wave;
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
			__scoreLabel.text = "" + __score;
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
			__moniesLabel.text = "" + __money;
		}
	}


	void Awake()
	{
		Application.LoadLevelAdditive("InGameUI");
	}

	void Update()
	{
		if(__uiCamera == null)
			Initialize();
	}

	public void BuildTower(GameObject button)
	{
		if(BuildingManager.Instance.BuildingState != BuildingState.CONSTRUCTION)
		{
			switch(button.name)
			{
				case "AlienButton":
					BuildingManager.Instance.tower = Tower.ALIEN;
					break;
				case "MageButton":
					BuildingManager.Instance.tower = Tower.MAGE;
					break;
				case "RobotButton":
					BuildingManager.Instance.tower = Tower.ROBOT;
					break;
			}

			BuildingManager.Instance.BuildingState = BuildingState.CONSTRUCTION;
		}
		else
			BuildingManager.Instance.BuildingState = BuildingState.GAMEPLAY;
	}

	public void Initialize()
	{
		__uiCamera = GameObject.FindObjectOfType<UICamera>();

		if(__uiCamera != null)
		{
			//	Constructions
			__moniesLabel = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/Money").gameObject.GetComponent<UILabel>();
			__alienCostLabel = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/AlienButton/Label").GetComponent<UILabel>();
			__mageCostLabel = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/MageButton/Label").GetComponent<UILabel>();
			__robotCostLabel = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/RobotButton/Label").GetComponent<UILabel>();

			__infoPanel = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/Info").GetComponent<UIPanel>();
			__towerNameLabel = __infoPanel.transform.FindChild("TowerName").GetComponent<UILabel>();
			__towerSummaryLabel = __infoPanel.transform.FindChild("Summary").GetComponent<UILabel>();
			__towerDamageLabel = __infoPanel.transform.FindChild("Damage").GetComponent<UILabel>();
			__towerRangeLabel = __infoPanel.transform.FindChild("Range").GetComponent<UILabel>();
			__towerFirerateLabel = __infoPanel.transform.FindChild("Firerate").GetComponent<UILabel>();

			//	Superpowers
			__superpowersPanel = __uiCamera.transform.FindChild("Superpowers/SuperpowerPanel").GetComponent<UIPanel>();
			__courierButton = __superpowersPanel.transform.FindChild("Courier").GetComponent<UIButton>();
			__courierTimer = __superpowersPanel.transform.FindChild("Courier/Timer").GetComponent<UILabel>();

			//	General
			__generalPanel = __uiCamera.transform.FindChild("General/GeneralPanel").GetComponent<UIPanel>();
			__generalLabel = __generalPanel.transform.FindChild("Label").GetComponent<UILabel>();
			//	Info
			__scoreLabel = __uiCamera.transform.FindChild("GameplayInfo/InfoPanel/Score").GetComponent<UILabel>();
			__waveLabel = __uiCamera.transform.FindChild("GameplayInfo/InfoPanel/Wave").GetComponent<UILabel>();
			//	Menu
			__nextWaveLabel = __uiCamera.transform.FindChild("Menu/MenuPanel/NextWave/Label").GetComponent<UILabel>();

			//	Upgrades
			//	--	Alien
			__alienUpgradePanel = __uiCamera.transform.FindChild("Upgrades/AlienUpgrade").GetComponent<UIPanel>();
			__alienLaserUpgradeButton = __alienUpgradePanel.transform.FindChild("Laser").GetComponent<UIButton>();
			__alienLaserUpgradeLabel = __alienLaserUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
			__alienDamageUpgradeButton = __alienUpgradePanel.transform.FindChild("Damage").GetComponent<UIButton>();
			__alienDamageUpgradeLabel = __alienDamageUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
            __alienRangeUpgradeButton = __alienUpgradePanel.transform.FindChild("Range").GetComponent<UIButton>();
			__alienRangeUpgradeLabel = __alienRangeUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
			//	--	Mage
			__mageUpgradePanel = __uiCamera.transform.FindChild("Upgrades/MageUpgrade").GetComponent<UIPanel>();
			__mageLeftUpgradeButton = __mageUpgradePanel.transform.FindChild("Left").GetComponent<UIButton>();
			__mageLeftUpgradeLabel = __mageLeftUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
			__mageRightUpgradeButton = __mageUpgradePanel.transform.FindChild("Right").GetComponent<UIButton>();
			__mageRightUpgradeLabel = __mageRightUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
			//	-- Robot
			__robotUpgradePanel = __uiCamera.transform.FindChild("Upgrades/RobotUpgrade").GetComponent<UIPanel>();
			__robotUpgradeButton = __robotUpgradePanel.transform.FindChild("Upgrade").GetComponent<UIButton>();
			__robotUpgradeLabel = __robotUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();

			ready = true;

			UIButtonMessage[] btnsMsgs = __uiCamera.GetComponentsInChildren<UIButtonMessage>();

			foreach(UIButtonMessage bm in btnsMsgs)
				bm.target = Instance.gameObject;
		}
	}

	public void EndGame(bool victory = false)
	{
		__generalLabel.text = (victory ? "Victory!" : "Defeat");

		UIPanel[] allPanels = GameObject.FindObjectsOfType<UIPanel>();

		foreach(UIPanel panel in allPanels)
			panel.gameObject.SetActive(false);

		__generalPanel.gameObject.SetActive(true);
	}
}
