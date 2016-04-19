using UnityEngine;
using System.Collections;
using System;

public class RobotTower : BaseTower
{
	public bool             leftMuzzle;
	public GameObject       bulletPrefab;


	protected override void Start()
	{
		base.Start();
	}

	void Update()
	{

	}

	public override void Fire()
	{
		GameObject bullet = GlobalObjectPoolManager.Instance.GetGameObject(bulletPrefab);
		
	}

	private void __SetBulletProperties()
	{

	}
}
