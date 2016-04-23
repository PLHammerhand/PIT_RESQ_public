using UnityEngine;
using System.Collections;

public abstract class ProjectileTower : BaseTower
{
	public float                projectileSpeed;
	public GameObject           projectilePrefab;

	protected ObjectPool        projectiles;


	public void Init()
	{
		projectiles = gameObject.AddComponent<ObjectPool>();
		projectiles.objectPrefab = projectilePrefab;
		projectiles.Initialize();
		_ready = true;
	}

	protected void _SetProjectileProperties(Projectile p)
	{
		p.damage = damage;
		p.projectileSpeed = projectileSpeed;
	}
}
