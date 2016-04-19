using UnityEngine;
using System.Collections;

public class RobotBullet : Projectile
{
	[HideInInspector]
	public float                lifetime;


	protected override void Update()
	{
		base.Update();

		lifetime -= Time.deltaTime;

		if(lifetime <= 0f)
			_SelfDestroy();
	}
}
