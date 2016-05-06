using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : Singleton<GUIManager>
{
	//	--	Panels
	//	--	--	Info
	private UIPanel				__infoPanel;
	//	--	--	Superpowers	(tweens only)
	private TweenPosition		__superpowersPanel;
	//	--	--	General
	private UIPanel				__generalPanel;
	//	--	--	Upgrades	(tweens only)
	private TweenPosition		__alienUpgradePanel;
	private TweenPosition       __mageUpgradePanel;
	private TweenPosition       __robotUpgradePanel;

	//	--	Buttons
	//	--	--	Menu
	private UIButton            __nextWaveButton;
	//	--	--	Constructions
	private UIButton            __alienButton;
	private UIButton            __mageButton;
	private UIButton            __robotButton;
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

			__nextWaveLabel.text = "Incoming!";
			__waveLabel.text = "" + __wave;
		}
	}

	public int NextWaveTime
	{
		set
		{
			__nextWaveLabel.text = "" + value;
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

			__CheckFunds();
		}
	}

	public bool Gameplay
	{
		set
		{
			__alienButton.isEnabled = value;
			__mageButton.isEnabled = value;
			__robotButton.isEnabled = value;

			__nextWaveButton.isEnabled = value;
		}
	}

	//	=====	METHODS

	void Awake()
	{
		Application.LoadLevelAdditive("InGameUI");
	}

	void Update()
	{
		if(__uiCamera == null)
			Initialize();
	}

	public void OpenUpgradePanel(Tower tower)
	{
		int upgrade;

		switch(tower)
		{
			case Tower.ALIEN:
				AlienTower clickedTower = BuildingManager.Instance.ClickedPosition.ConstructedTower as AlienTower;

				upgrade = (clickedTower.DamageLevel + 1) * CostsManager.Instance.alienDamageUpgrade;
				__alienDamageUpgradeLabel.text = "" + upgrade;

				if(__money < upgrade)
					__alienDamageUpgradeButton.isEnabled = false;
				else
					__alienDamageUpgradeButton.isEnabled = true;

				upgrade = (clickedTower.LaserCount + 1) * CostsManager.Instance.alienLaserUpgrade;
				__alienLaserUpgradeLabel.text = "" + upgrade;

				if(__money < upgrade)
					__alienLaserUpgradeButton.isEnabled = false;
				else
					__alienLaserUpgradeButton.isEnabled = true;

				upgrade = (clickedTower.RangeLevel + 1) * CostsManager.Instance.alienRangeUpgrade;
				__alienRangeUpgradeLabel.text = "" + upgrade;

				if(__money < upgrade)
					__alienRangeUpgradeButton.isEnabled = false;
				else
					__alienRangeUpgradeButton.isEnabled = true;

				__alienUpgradePanel.Play(true);
				__mageUpgradePanel.Play(false);
				__robotUpgradePanel.Play(false);
				break;
			case Tower.MAGE:
				upgrade = ((BuildingManager.Instance.ClickedPosition.ConstructedTower as MageTower).level + 1) * CostsManager.Instance.mageUpgrade;
				__mageLeftUpgradeLabel.text = "" + upgrade;
				__mageRightUpgradeLabel.text = "" + upgrade;

				if(__money < upgrade)
				{
					__mageLeftUpgradeButton.isEnabled = false;
					__mageRightUpgradeButton.isEnabled = false;
				}
				else
				{
					__mageLeftUpgradeButton.isEnabled = true;
					__mageRightUpgradeButton.isEnabled = true;
				}

				__mageUpgradePanel.Play(true);
				__alienUpgradePanel.Play(false);
				__robotUpgradePanel.Play(false);
				break;
			case Tower.ROBOT:
				upgrade = CostsManager.Instance.robotTowerUpgrade * RobotTower.Level;

				if(__money < CostsManager.Instance.robotTowerUpgrade * RobotTower.Level)
					__robotUpgradeButton.isEnabled = false;
				else
					__robotUpgradeButton.isEnabled = true;

				__robotUpgradePanel.Play(true);
				__alienUpgradePanel.Play(false);
				__mageUpgradePanel.Play(false);
				break;
		}
	}

	public void CloseUpgradePanels()
	{
		__alienUpgradePanel.Play(false);
		__mageUpgradePanel.Play(false);
		__robotUpgradePanel.Play(false);
	}

	public void SendNextWave()
	{
		LevelMaster.Instance.StartNextWave();
		Wave++;
	}

	public void DisableInputManager()
	{
		InputManager.Instance.enableInput = false;
	}

	public void EnableInputManager()
	{
		InputManager.Instance.enableInput = true;
	}

	public void UpgradeTower(GameObject button)
	{
		string name = button.name;

		switch(name)
		{
			case "Upgrade":
				BuildingManager.Instance.UpgradeRobotTower();
				break;
			case "Left":
				BuildingManager.Instance.UpgradeMageTower(true);
				break;
			case "Right":
				BuildingManager.Instance.UpgradeMageTower(false);
				break;
			case "Damage":
				BuildingManager.Instance.UpgradeAlienTower(TowerAttribute.DAMAGE);
				break;
			case "Laser":
				BuildingManager.Instance.UpgradeAlienTower(TowerAttribute.LASER);
				break;
			case "Range":
				BuildingManager.Instance.UpgradeAlienTower(TowerAttribute.RANGE);
				break;
		}

		CloseUpgradePanels();
	}

	public void BuildTower(GameObject button)
	{
		CloseUpgradePanels();

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

	public virtual void Initialize()
	{
		__uiCamera = GameObject.FindObjectOfType<UICamera>();

		if(__uiCamera != null)
		{
			//	Constructions
			__alienButton = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/AlienButton").GetComponent<UIButton>();
			__mageButton = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/MageButton").GetComponent<UIButton>();
			__robotButton = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/RobotButton").GetComponent<UIButton>();

			__alienCostLabel = __alienButton.transform.FindChild("Label").GetComponent<UILabel>();
			__mageCostLabel = __mageButton.transform.FindChild("Label").GetComponent<UILabel>();
			__robotCostLabel = __robotButton.transform.FindChild("Label").GetComponent<UILabel>();

			__moniesLabel = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/Money").gameObject.GetComponent<UILabel>();

			__infoPanel = __uiCamera.transform.FindChild("Constructions/ConstructionsPanel/Info").GetComponent<UIPanel>();
			__towerNameLabel = __infoPanel.transform.FindChild("TowerName").GetComponent<UILabel>();
			__towerSummaryLabel = __infoPanel.transform.FindChild("Summary").GetComponent<UILabel>();
			__towerDamageLabel = __infoPanel.transform.FindChild("Damage").GetComponent<UILabel>();
			__towerRangeLabel = __infoPanel.transform.FindChild("Range").GetComponent<UILabel>();
			__towerFirerateLabel = __infoPanel.transform.FindChild("Firerate").GetComponent<UILabel>();

			//	Superpowers
			__superpowersPanel = __uiCamera.transform.FindChild("Superpowers/SuperpowerPanel").GetComponent<TweenPosition>();
			__courierButton = __superpowersPanel.transform.FindChild("Courier").GetComponent<UIButton>();
			__courierTimer = __superpowersPanel.transform.FindChild("Courier/Timer").GetComponent<UILabel>();

			//	General
			__generalPanel = __uiCamera.transform.FindChild("General/GeneralPanel").GetComponent<UIPanel>();
			__generalLabel = __generalPanel.transform.FindChild("Label").GetComponent<UILabel>();
			//	Info
			__scoreLabel = __uiCamera.transform.FindChild("GameplayInfo/InfoPanel/Score").GetComponent<UILabel>();
			__waveLabel = __uiCamera.transform.FindChild("GameplayInfo/InfoPanel/Wave").GetComponent<UILabel>();
			//	Menu
			__nextWaveButton = __uiCamera.transform.FindChild("Menu/MenuPanel/NextWave").GetComponent<UIButton>();
            __nextWaveLabel = __nextWaveButton.transform.FindChild("Label").GetComponent<UILabel>();

			//	Upgrades
			//	--	Alien
			__alienUpgradePanel = __uiCamera.transform.FindChild("Upgrades/AlienUpgrade").GetComponent<TweenPosition>();
			__alienLaserUpgradeButton = __alienUpgradePanel.transform.FindChild("Laser").GetComponent<UIButton>();
			__alienLaserUpgradeLabel = __alienLaserUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
			__alienDamageUpgradeButton = __alienUpgradePanel.transform.FindChild("Damage").GetComponent<UIButton>();
			__alienDamageUpgradeLabel = __alienDamageUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
            __alienRangeUpgradeButton = __alienUpgradePanel.transform.FindChild("Range").GetComponent<UIButton>();
			__alienRangeUpgradeLabel = __alienRangeUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
			//	--	Mage
			__mageUpgradePanel = __uiCamera.transform.FindChild("Upgrades/MageUpgrade").GetComponent<TweenPosition>();
			__mageLeftUpgradeButton = __mageUpgradePanel.transform.FindChild("Left").GetComponent<UIButton>();
			__mageLeftUpgradeLabel = __mageLeftUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();
			__mageRightUpgradeButton = __mageUpgradePanel.transform.FindChild("Right").GetComponent<UIButton>();
			__mageRightUpgradeLabel = __mageRightUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();

			//	-- Robot
			__robotUpgradePanel = __uiCamera.transform.FindChild("Upgrades/RobotUpgrade").GetComponent<TweenPosition>();
			__robotUpgradeButton = __robotUpgradePanel.transform.FindChild("Upgrade").GetComponent<UIButton>();
			__robotUpgradeLabel = __robotUpgradeButton.transform.FindChild("Label").GetComponent<UILabel>();

			__alienUpgradePanel.Play(false);
			__mageUpgradePanel.Play(false);
			__robotUpgradePanel.Play(false);
			__superpowersPanel.Play(false);

			UIButtonMessage[] btnsMsgs = __uiCamera.GetComponentsInChildren<UIButtonMessage>();

			foreach(UIButtonMessage bm in btnsMsgs)
				bm.target = Instance.gameObject;

			ready = true;
		}
	}

	private void __CheckFunds()
	{
		if(__money < CostsManager.Instance.baseTower)
		{
			__alienButton.isEnabled = false;
			__mageButton.isEnabled = false;
			__robotButton.isEnabled = false;
		}
		else
		{
			__alienButton.isEnabled = true;
			__mageButton.isEnabled = true;

			if(__money < (CostsManager.Instance.baseTower + (RobotTower.Level - 1) * CostsManager.Instance.robotTowerUpgrade))
				__robotButton.isEnabled = false;
			else
				__robotButton.isEnabled = true;
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
