using UnityEngine;
using System.Collections;

public class CostsManager : Singleton<CostsManager>
{
	public int					baseTower					= 100;
	public int                  robotTowerUpgrade           = 50;

	public int                  robotUpgrade				= 100;
	public int                  mageUpgrade					= 75;

	public int                  alienDamageUpgrade			= 10;
	public int                  alienLaserUpgrade			= 50;
	public int                  alienRangeUpgrade			= 20;

	public void Init()
	{
		ready = true;
	}
}
