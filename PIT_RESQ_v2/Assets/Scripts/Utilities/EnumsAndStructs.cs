using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Wave
{
	public GameObject           enemy;
	public int                  enemyCount;
}

public enum BuildingState
{
	GAMEPLAY,
	CONSTRUCTION,
	UPGRADE
}

public enum Tower
{
	ALIEN,
	MAGE,
	ROBOT
}
