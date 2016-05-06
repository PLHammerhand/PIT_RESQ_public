using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AlienTower : BaseTower
{
	public int DamageLevel
	{
		get
		{
			return __damageLevel;
		}
		set
		{
			__damage = damage + (5 * __damageLevel);

			__damageLevel = value;
		}
	}

	public int RangeLevel
	{
		get
		{
			return __rangeLevel;
		}
		set
		{
			_capsuleCollider.radius = range + (0.5f * __rangeLevel);

			__rangeLevel = value;
		}
	}

	public int LaserCount
	{
		get
		{
			return __lasers.Count;
		}
	}

	private int						__damageLevel				= 1;
	private int						__rangeLevel				= 1;
	private int                     __damage;

	private List<GameObject>        __targets;
	private List<LineRenderer>		__lasers;


	void Awake()
	{
		__damage = damage;
		__lasers = new List<LineRenderer>();
		__targets = new List<GameObject>();
	}

	protected override void Start()
	{
		base.Start();

		AddLaser();
	}

	//	TODO	repair whole Alien Tower system

	protected override void Update()
	{
		if(_targetsList.Count > 0)
		{
			Debug.Log("> Got targets!");
			if(__targets.Count < __lasers.Count)
			{
				Debug.Log(">> Laser's ready!");
				GameObject go = _NextTarget();

				Debug.Log(">> go: " + go.name);

				if(go != null)
				{
					__targets.Add(go);
					_targetsList.RemoveAt(0);
				}
				else
					_targetsList = new List<GameObject>();
            }
		}

		if(__targets.Count > 0)
			Fire();
		else
			__HoldFire();
	}

	//void LateUpdate()
	//{
	//	List<GameObject> newTargets = new List<GameObject>();

	//	foreach(GameObject go in __targets)
	//	{
	//		if(go.activeInHierarchy)
	//			newTargets.Add(go);
	//	}

	//	__targets = newTargets;
	//}

	//	TODO	change system to be more efective

	public override void Fire()
	{
		int i = 0;
		bool stop = false;

		do
		{
			__lasers[i].SetVertexCount(2);
			__lasers[i].SetPosition(0, muzzle[0].position);
			__lasers[i].SetPosition(1, __targets[i].transform.position);

			i++;

			if(i == __lasers.Count || i == __targets.Count)
				stop = true;
			
			Debug.Log(">>> Firing!\t" + stop);
		} while(!stop);
	}

	private void __HoldFire()
	{
		foreach(LineRenderer lr in __lasers)
			lr.enabled = false;
	}

	public void AddLaser()
	{
		GameObject go = new GameObject();
		LineRenderer line = go.AddComponent<LineRenderer>();
        __lasers.Add(line);
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetColors(Color.red, Color.red);
		line.SetWidth(0.25f, 0.25f);
		line.enabled = false;

		go.transform.position = muzzle[0].position;
		go.transform.SetParent(muzzle[0]);
		go.name = "Laser " + LaserCount;
	}
}
