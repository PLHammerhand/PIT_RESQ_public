using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseTower : MonoBehaviour
{
	public int						damage;
	public float					range;
	public float					firerate;
	public Transform                muzzle;

	private GameObject				__target;
	private Queue<GameObject>		__targetsQueue;


	protected virtual void Start()
	{
		__targetsQueue = new Queue<GameObject>();
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			if(__target == null)
				__target = other.gameObject;
			else
				__targetsQueue.Enqueue(other.gameObject);
		}
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			if(__target == other.gameObject)
			{
				if(__targetsQueue.Count > 0)
					__target = __targetsQueue.Dequeue();
				else
					__target = null;
			}
		}
	}

	public abstract void Fire();
}
