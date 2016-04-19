using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : Singleton<GUIManager>
{
	//	Panels
	public UIPanel gemPanel;
	public UIPanel infoPanel;
	public UIPanel alienUpgradePanel;
	public UIPanel mageUpgradePanel;
	public UIPanel robotUpgradePanel;

	//	Buttons
	public UIButton courierButton;
	public UIButton alienLaserUpgradeButton;
	public UIButton alienDamageUpgradeButton;
	public UIButton alienRangeUpgradeButton;
	public UIButton robotUpgradeButton;
	public UIButton mageRightUpgradeButton;
	public UIButton mageLeftUpgradeButton;

	//	Labels
	public UILabel scoreLabel;
	public UILabel waveLabel;
	public UILabel nextWaveLabel;
	public UILabel generalLabel;
	public UILabel courierCounter;
	public UILabel towerNameLabel;
	public UILabel towerSummaryLabel;
	public UILabel towerDamageLabel;
	public UILabel towerRangeLabel;
	public UILabel towerFirerateLabel;
	public UILabel moniesLabel;
	public UILabel RobotCostLabel;
	public UILabel mageCostLabel;
	public UILabel alienCostLabel;
	public UILabel alienLaserUpgradeLabel;
	public UILabel alienDamageUpgradeLabel;
	public UILabel alienRangeUpgradeLabel;
	public UILabel mageRightUpgradeLabel;
	public UILabel mageLeftUpgradeLabel;
	public UILabel robotUpgradeLabel;

	//	Other
	public int money;

	private Transform           __uiCamera;
	private int                 __score;

	public int Score
	{
		get
		{
			return __score;
		}
		set
		{
			__score = value;
		}
	}


	void Awake()
	{
		Application.LoadLevelAdditive("InGameUI");
	}

	void Update()
	{
		//Initialize();
	}

	public void Initialize()
	{
		__uiCamera = GameObject.FindObjectOfType<UICamera>().transform;
	}

	public void EndGame(bool victory = false)
	{
		generalLabel.text = (victory ? "Victory!" : "Defeat");


		//__overlay.gameObject.SetActive(true);

		//Hashtable args = new Hashtable();
		//args.Add("from", __overlay.color.a);
		//args.Add("to", 255f);
		//args.Add("time", 2f);
		//args.Add("onupdate", "ChangeOverlayTransparency");
		//args.Add("onupdatetarget", gameObject);

		//iTween.ValueTo(gameObject, args);
	}
}
