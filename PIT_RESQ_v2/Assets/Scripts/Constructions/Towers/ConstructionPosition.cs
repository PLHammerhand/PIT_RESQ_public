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
	public BaseTower ConstructedTower
	{
		get
		{
			return __constructedTower;
		}
		set
		{
			if(value != null)
			{
				if(__constructedTower != null)
					__constructedTower.towerPosition = null;

				__constructedTower = value;
				__constructedTower.towerPosition = this;

				if(boostFirerate)
					__constructedTower.firerate *= multiplayerFirerate;
				if(boostDamage)
					__constructedTower.damage = (int)(__constructedTower.damage * multiplayerDamage);
				if(boostRange)
					__constructedTower.range *= multiplayerRange;
			}
        }
	}

	[HideInInspector]
	public bool Renderer
	{
		get
		{
			return __renderer.enabled;
		}
		set
		{
			__renderer.enabled = value;
		}
	}

	private BaseTower       __constructedTower;
	private MeshRenderer    __renderer;


	void Start()
	{
		if(boostFirerate)
		{
			GameObject go = Resources.Load("Particles/Boosts/Firerate") as GameObject;
			go.transform.position = gameObject.transform.position;
			go.transform.rotation = gameObject.transform.rotation;
        }

		if(boostDamage)
		{
			GameObject go = Resources.Load("Particles/Boosts/Damage") as GameObject;
			go.transform.position = gameObject.transform.position;
			go.transform.rotation = gameObject.transform.rotation;
		}

		if(boostRange)
		{
			GameObject go = Resources.Load("Particles/Boosts/Range") as GameObject;
			go.transform.position = gameObject.transform.position;
			go.transform.rotation = gameObject.transform.rotation;
		}

		__renderer = gameObject.GetComponent<MeshRenderer>();
	}
}
