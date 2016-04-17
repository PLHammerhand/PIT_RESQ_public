using UnityEngine;
using System.Collections;

public class CostsScript : MonoBehaviour
{
	//			========	 MAIN COSTS 	========
	public int towerCosts;			//	Main total cost

	private int alienTowerCost;		//	Main cost
	private int mageTowerCost;		//	Main cost
	private int robotTowerCost;		//	Main cost
	
	//			======	ALIEN TOWERS COSTS	======
	public int alienDamageUpgradeCost;
	public int alienDamageUpgradeAdditive;

	public int alienFirerateUpgradeCost;
	public int alienFirerateUpgradeAdditive;

	public int alienLaserUpgradeCost;
	public int alienLaserUpgradeAdditive;

	public int alienRangeUpgradeCost;
	public int alienRangeUpgradeAdditivie;

	//			======	MAGE TOWERS COSTS	======
	public int mageTowerUpgradeLeftCost;
	public int mageTowerUpgradeRightCost;
	public float mageTowerUpgradeMultiplayer;

	//			====== ROBOT TOWERS COSTS	======
	public int robotTowerUpgradeCost;
	public float robotTowerUpgradeMultiplayer;
	public int[] robotTowerSpecialAbilitiesCosts;

	//END

	void Awake()
	{
		alienTowerCost = towerCosts;
		mageTowerCost = towerCosts;
		robotTowerCost = towerCosts;
	}

	public int getAlienTowerCost()
	{
		return alienTowerCost;
	}

	public int getMageTowerCost()
	{
		return mageTowerCost;
	}

	public int getRobotTowerCost()
	{
		return robotTowerCost;
	}
}
