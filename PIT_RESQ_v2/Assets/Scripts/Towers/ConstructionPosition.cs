using UnityEngine;
using System.Collections;

public class ConstructionPosition : MonoBehaviour
{
	public bool             boostFirerate;
	public bool             boostDamage;
	public bool             boostRange;

	public float            multiplayerFirerate;
	public float            multiplayerDamage;
	public float            multiplayerRange;

	[HideInInspector]
	public BaseTower        constructedTower
	{
		get
		{
			return __constructedTower;
		}
		set
		{
			__constructedTower = value;

			if(boostFirerate)
				__constructedTower.firerate *= multiplayerFirerate;
			if(boostDamage)
				__constructedTower.damage = (int)(__constructedTower.damage * multiplayerDamage);
			if(boostRange)
				__constructedTower.range *= multiplayerRange;
        }
	}

	private BaseTower       __constructedTower;


	void Start()
	{
		if(boostFirerate)
		{
			GameObject go = Instantiate(BuildingManager.Instance.firerateBoostParticles) as GameObject;
			go.transform.position = gameObject.transform.position;
			go.transform.rotation = gameObject.transform.rotation;
        }

		if(boostDamage)
		{
			GameObject go = Instantiate(BuildingManager.Instance.damageBoostParticles) as GameObject;
			go.transform.position = gameObject.transform.position;
			go.transform.rotation = gameObject.transform.rotation;
		}

		if(boostRange)
		{
			GameObject go = Instantiate(BuildingManager.Instance.rangeBoostParticles) as GameObject;
			go.transform.position = gameObject.transform.position;
			go.transform.rotation = gameObject.transform.rotation;
		}
	}
}
