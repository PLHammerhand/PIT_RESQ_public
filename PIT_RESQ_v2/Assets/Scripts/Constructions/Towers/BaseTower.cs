using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseTower : MonoBehaviour
{
	public int							damage;
	public float						range;
	public float						firerate;
	[HideInInspector]
	public Transform[]					muzzle;

	protected bool						_ready						= false;
	protected GameObject				_target;
	protected List<GameObject>			_targetsList;


	protected virtual void Start()
	{
		_targetsList = new List<GameObject>();

		if(gameObject.transform.FindChild("Muzzle") != null)
		{
			muzzle = new Transform[1];
			muzzle[0] = gameObject.transform.FindChild("Muzzle");
		}
	}

	protected virtual void Update()
	{
		if(_target != null)
		{
			if(_target.activeInHierarchy)
				Fire();
			else
			{
				_targetsList.Remove(_target);
				_target = _NextTarget();
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			_targetsList.Add(other.gameObject);

			if(_target == null && other.gameObject.activeInHierarchy)
				_target = other.gameObject;
		}
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			if(_targetsList.Contains(other.gameObject))
				_targetsList.Remove(other.gameObject);

			if(_target == other.gameObject)
			{
				if(_targetsList.Count > 0)
				{
					_target = _NextTarget();

					if(_target == null)
						_targetsList = new List<GameObject>();
				}
				else
					_target = null;
			}
		}
	}

	protected virtual void OnTriggerStay(Collider other)
	{

	}

	protected GameObject _NextTarget()
	{
		for(int i = 0; i < _targetsList.Count; i++)
		{
			if(_targetsList[i].activeInHierarchy)
				return _targetsList[i];
		}

		return null;
	}

	public abstract void Fire();
}
