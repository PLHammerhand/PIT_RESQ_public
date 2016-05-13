using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class AlienTower : BaseTower
{
	public UnityEvent               targetCheck;

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
			return _projectiles.ObjectsInPool;
		}
	}

	private int                     __damageLevel               = 1;
	private int                     __rangeLevel                = 1;
	private int                     __damage;

	private List<GameObject>        __targets;

	void Awake()
	{
		projectilePrefab = Resources.Load("Towers/Projectiles/AlienLaser") as GameObject;
		__damage = damage;
		__targets = new List<GameObject>();
		targetCheck = new UnityEvent();
	}

	/*
			POTRZEBA EVENTÓW!!!
	*/

	protected override void Start()
	{
		base.Start();

		_projectiles = gameObject.AddComponent<ObjectPool>();
		_projectiles.Initialize(false);
		_projectiles.autoincrease = false;

		AddLaser();
	}

	protected override void Update()
	{
		if(_targetsList.Count > 0)
			__ManageTargets();

		if(__targets.Count > 0)
			Fire();
	}

	void FixedUpdate()
	{
		if(__targets.Count > 0)
		{
			foreach(GameObject e in __targets)
				e.GetComponent<Enemy>().DealDamage(damage);
		}
	}

	private void __ManageTargets()
	{
		int i = 0;

		do
		{
			if(!_targetsList[i].activeInHierarchy)
			{
				if(__targets.Contains(_targetsList[i]))
					__targets.Remove(_targetsList[i]);

				_targetsList.Remove(_targetsList[i]);
			}
			else if(__targets.Count < LaserCount)
			{
				if(!__targets.Contains(_targetsList[i]))
				{
					Debug.Log("Firing at target");
					__targets.Add(_targetsList[i]);
				}

				i++;
			}
			else
				break;
		} while(i < _targetsList.Count);
	}

	public override void Fire()
	{
		foreach(GameObject e in __targets)
		{
			if(!_projectiles.GetObject())
				continue;

			Laser laser = _projectiles.GetObject().GetComponent<Laser>();
			laser.Target = e;
			laser.gameObject.SetActive(true);
		}
	}

	public bool IsTarget(GameObject enemy)
	{
		return __targets.Contains(enemy);
	}

	protected override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);

		if(__targets.Contains(other.gameObject))
		{
			__targets.Remove(other.gameObject);
			targetCheck.Invoke();
		}
	}

	public void AddLaser()
	{
		GameObject go = Instantiate(projectilePrefab) as GameObject;
		_projectiles.AddGameObject(go);

		go.GetComponent<Laser>().parentTower = this;
		go.transform.position = muzzle[0].position;
		go.transform.SetParent(muzzle[0]);
		go.name = "Laser " + LaserCount;
		go.SetActive(false);
	}
}
