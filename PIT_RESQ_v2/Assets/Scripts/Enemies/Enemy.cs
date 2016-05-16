using UnityEngine;
using Pathfinding;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public int                  value					= 10;
	public int                  health;
	public float                speed;
	public Transform            target;

	private int                 __currentWaypoint;
	private int                 __currentHealth;
	private float               __waypointDistance      = 0.2f;
	private Path                __path;
	private Seeker              __seeker;

	private bool                __returning;
	private GameObject          __gem;
	private GameObject          __smoke;

	public bool GotGem
	{
		get
		{
			return __gem.activeInHierarchy;
		}
	}

	public bool Returning
	{
		get
		{
			return __returning;
		}
	}


	void Awake()
	{
		__currentHealth = health;
		__gem = transform.FindChild("Gem").gameObject;
		__smoke = transform.FindChild("Smoke").gameObject;
		__seeker = gameObject.GetComponent<Seeker>();
	}

	void OnEnable()
	{
		__currentHealth = health;
		__returning = false;
		__CalculatePath();
	}

	void Start()
	{
		__gem.SetActive(false);
	}

	void Update()
	{
		if(__path == null)
			return;

		if(__currentWaypoint >= __path.vectorPath.Count)
		{
			if(__returning)
				__seeker.StartPath(gameObject.transform.position, LevelMaster.Instance.enemySpawn.transform.position, OnPathComplete);
			else
				return;
		}

		if(__currentWaypoint < __path.vectorPath.Count)
		{
			Vector3 dir = (__path.vectorPath[__currentWaypoint] - transform.position).normalized * speed * Time.deltaTime;
			transform.Translate(dir);

			if(Vector3.Distance(transform.position, __path.vectorPath[__currentWaypoint]) < __waypointDistance)
			{
				__currentWaypoint++;
				//gameObject.transform.LookAt(__path.vectorPath[__currentWaypoint]);
			}
		}
	}

	private void __CalculatePath()
	{
		if(LevelMaster.Instance.Gameplay)
		{
			if(__returning)
				__seeker.StartPath(transform.position, LevelMaster.Instance.enemySpawn.transform.position, OnPathComplete);
			else
				__seeker.StartPath(transform.position, LevelMaster.Instance.candyshopPosition.position, OnPathComplete);
		}
	}

	public void OnPathComplete(Path p)
	{
		__path = p;
		__currentWaypoint = 0;
	}

	public void Return(bool pickedGem = false)
	{
		__returning = true;
		__CalculatePath();

		if(pickedGem)
			__gem.SetActive(true);
	}

	public void FindPathTo(Vector3 pos)
	{
		__seeker.StartPath(transform.position, pos, OnPathComplete);
	}

	public void DealDamage(int amount)
	{
		__currentHealth -= amount;

		if(__currentHealth <= 0)
			__SelfDestroy();
	}

	private void __SelfDestroy()
	{
		LevelMaster.Instance.EnemyDestroyed(this, GotGem);
		LevelMaster.Instance.Score += health / 10;
		gameObject.SetActive(false);
	}

	public void Escape()
	{
		if(GotGem)
		{
			LevelMaster.Instance.Gems--;
			__gem.SetActive(false);
		}

		gameObject.SetActive(false);
	}

	void OnDisable()
	{
		__gem.SetActive(false);
		__seeker.pathCallback -= OnPathComplete;
		gameObject.transform.position = new Vector3(0f, -100f, 0f);
	}
}
