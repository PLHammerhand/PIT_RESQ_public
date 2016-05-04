using UnityEngine;
using System.Collections;
using System;

public class MageTower : ProjectileTower
{
	public int                  level;
	public GameObject           leftUpgrade;
	public GameObject           rightUpgrade;

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
		GameObject go = _projectiles.GetObject();
		go.transform.position = muzzle[0].transform.position;
		__SetMissleProperties(go.GetComponent<MageMissle>());
		go.SetActive(true);
	}

	private void __SetMissleProperties(MageMissle missle)
	{
		_SetProjectileProperties(missle);
		missle.Target = _target;
	}
}
