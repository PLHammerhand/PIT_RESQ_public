using UnityEngine;
using System.Collections;

public class MageMissle : Projectile
{
	private GameObject              __target;

	[HideInInspector]
	public GameObject               Target
	{
		get
		{
			return __target;
		}
		set
		{
			__target = value;

			if(value == null)
				_SelfDestroy();
		}
	}


	protected override void Update()
	{
		if(__target == null)
			_SelfDestroy();

		if(!__target.activeInHierarchy)
			_SelfDestroy();

		gameObject.transform.LookAt(__target.transform);

		base.Update();
	}
}
