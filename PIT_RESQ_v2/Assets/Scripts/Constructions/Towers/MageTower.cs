using UnityEngine;
using System.Collections;
using System;

public class MageTower : ProjectileTower
{
	void Awake()
	{
		projectilePrefab = Resources.Load("Towers/Projectiles/MageMissile") as GameObject;

		Init();
	}

	protected override void Start()
	{
		base.Start();
	}

	public override void Fire()
	{
		GameObject go = GlobalObjectPoolManager.Instance.GetGameObject(projectilePrefab);
		go.transform.position = muzzle[0].transform.position;
	}

	private void __SetMissleProperties(MageMissle missle)
	{
		_SetProjectileProperties(missle);
	}
}
