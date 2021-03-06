﻿using UnityEngine;
using System.Collections;

public class CostsManager : Singleton<CostsManager>
{
	public int					baseTower					= 100;
	public int                  towerCountCost				= 25;

	public int                  robotUpgrade				= 100;
	public int                  mageUpgrade					= 75;

	public int                  alienDamageUpgrade			= 10;
	public int                  alienLaserUpgrade			= 50;
	public int                  alienRangeUpgrade			= 20;

	public override void Initialize()
	{
		ready = true;
	}
}
