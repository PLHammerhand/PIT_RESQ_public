using UnityEngine;
using System.Collections;

public abstract class ProjectileTower : BaseTower
{
	public float                projectileSpeed;

	protected float             _refireTime;
	protected float             _nextFireTime;

	protected override void Start()
	{
		_refireTime = 60f / firerate;
		_nextFireTime = _refireTime;

		base.Start();
	}

	protected override void Update()
	{
		if(_nextFireTime <= 0f)
		{
			base.Update();
			_nextFireTime = _refireTime;
		}
		else
			_nextFireTime -= Time.deltaTime;
	}

	public void Init()
	{
		_projectiles = gameObject.AddComponent<ObjectPool>();
		_projectiles.objectPrefab = projectilePrefab;
		_projectiles.Initialize();
	}

	protected void _SetProjectileProperties(Projectile p)
	{
		p.damage = damage;
		p.projectileSpeed = projectileSpeed;
	}
}
