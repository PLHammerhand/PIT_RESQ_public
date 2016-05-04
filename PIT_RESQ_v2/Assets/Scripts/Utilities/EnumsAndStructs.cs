using UnityEngine;
using System.Collections;


[System.Serializable]
public struct Wave
{
	public GameObject[]				enemies;
	public int[]					enemiesCount;
}

public enum BuildingState
{
	GAMEPLAY,
	CONSTRUCTION,
	UPGRADE,
	STOP
}

public enum TowerAttribute
{
	DAMAGE,
	FIRERATE,
	RANGE,
	LASER,
	PROJECTILE,
	POSITION,
	SPEED
}

public enum Tower
{
	ALIEN,
	MAGE,
	ROBOT
}
