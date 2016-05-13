using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour
{
	private Enemy           __target;
	private Light           __light;

	public bool GemLight
	{
		get
		{
			return __light.enabled;
		}
		set
		{
			__light.enabled = value;
		}
	}

	public Enemy Target
	{
		get
		{
			return __target;
		}
		set
		{
			if(__target != null)
			{
				if(value == null)
					__target.FindPathTo(LevelMaster.Instance.candyshopPosition.position);
				else
					__target.FindPathTo(gameObject.transform.position);
			}

			__target = value;
		}
	}

	void Awake()
	{
		__light = gameObject.GetComponent<Light>();
		__light.enabled = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy")
		{
			Enemy enemy = other.GetComponent<Enemy>();

			if(!enemy.GotGem)
			{
				enemy.Return(true);
				gameObject.SetActive(false);
			}
		}
	}

	void OnEnable()
	{
		if(LevelMaster.Instance.Gameplay)
			Target = LevelMaster.Instance.FindClosestEnemy(gameObject);
	}

	void OnDisable()
	{
		Target = null;
	}
}
