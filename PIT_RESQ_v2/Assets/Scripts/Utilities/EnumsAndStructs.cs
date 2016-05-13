using System;
using UnityEngine;

//	TODO	Zmiana Wave(GameObject[], int[]) na coś w stylu:
//				Wave(Enemies[]) ze strukturą:
//				Enemies(GameObject, int)
[Serializable]
public struct Wave
{
	public EnemyCount[]				enemies;
}

[Serializable]
public struct EnemyCount
{
	public GameObject               enemy;
	public int                      count;
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
