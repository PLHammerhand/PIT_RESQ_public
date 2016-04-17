using UnityEngine;
using System.Collections;

public class PositionScript : MonoBehaviour
{
	public float boostDamage = 1f;		//	Higher values = greater damage
	public float boostFirerate = 1f;	//	Lower values = faster firerate
	public float boostRange = 1f;		//	Higher values = bigger range

	public GameObject tower;
	public GameObject boostDamageParticle;
	public GameObject boostFirerateParticle;
	public GameObject boostRangeParticle;

	private PositionInfo info;
	private int towerType;

	void Start()
	{
		if(boostDamage >= 1.1f) boostDamageParticle.SetActive(true);
		else boostDamageParticle.SetActive(false);
		if(boostFirerate >= 1.1f) boostFirerateParticle.SetActive(true);
		else boostFirerateParticle.SetActive(false);
		if(boostRange >= 1.1f) boostRangeParticle.SetActive(true);
		else boostRangeParticle.SetActive(false);

		info = new PositionInfo(boostFirerate, boostRange, boostDamage, this);
	}
	
	public void BuildTower(GameObject t)
	{
		tower = t;
		if(t.tag == "RobotTower") towerType = 0;
		else if(t.tag == "MageTower") towerType = 1;
		else if(t.tag == "AlienTower") towerType = 2;

		tower.SendMessage("ChangeValues", info, SendMessageOptions.DontRequireReceiver);
	}

	public void UpgradeTower(GameObject t)
	{
		tower = t;
	}

	public bool IsTowerBuild()
	{
		if(tower != null) return true;
		else return false;
	}

	public GameObject GetTower()
	{
		return tower;
	}

	public int GetTowerType()
	{
		return towerType;
	}
}

public class PositionInfo
{
	public float boostFirerate;
	public float boostRange;
	public float boostDamage;
	public PositionScript myPos;

	public PositionInfo(float firerate, float range, float dmg, PositionScript pos)
	{
		boostFirerate = firerate;
		boostRange = range;
		boostDamage = dmg;
		myPos = pos;
	}
}
