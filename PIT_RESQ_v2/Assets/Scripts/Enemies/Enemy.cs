using UnityEngine;
using Pathfinding;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public static EnemySpawn   EnemySpawn;

	public bool					gotGem					= false;

	public int					health;
	public float				speed;
	public Transform			target;
	//	??

	private int                 __currentWaypoint;
	private bool                __returning;
	private float               __waypointDistance		= 0.2f;
	private Path                __path;
	private Seeker              __seeker;
	private GameObject			__gem;

	void Start()
	{
		if(EnemySpawn == null)
			EnemySpawn = GameObject.FindObjectOfType<EnemySpawn>();

		__gem = transform.FindChild("Gem").gameObject;
		__gem.SetActive(false);
		__seeker = gameObject.GetComponent<Seeker>();
	}

	void Update()
	{
		if(__path == null)
			return;

		if(__currentWaypoint >= __path.vectorPath.Count)
		{
			if(__returning)
				__seeker.StartPath(gameObject.transform.position, EnemySpawn.gameObject.transform.position, OnPathComplete);
			else
				return;
		}

		Vector3 dir = (__path.vectorPath[__currentWaypoint] - transform.position).normalized * speed * Time.deltaTime;
		transform.Translate(dir);

		if(Vector3.Distance(transform.position, __path.vectorPath[__currentWaypoint]) < __waypointDistance)
		{
			__currentWaypoint++;
			gameObject.transform.LookAt(__path.vectorPath[__currentWaypoint]);
		}

		//if(Vector3.Distance(transform.position, __path.vectorPath[__currentWaypoint]) < __waypointDistance)
		//{
		//	if(__currentWaypoint >= __path.vectorPath.Count - 1 && __returning)
		//	{
		//		__seeker.StartPath(transform.position, LevelMaster.Instance.candyshopPosition.position, OnPathComplete);
		//		return;
		//	}
		//	else
		//	{
		//		__currentWaypoint++;
		//		return;
		//	}
		//}
	}

	private void __CalculatePath()
	{
		if(__returning)
			__seeker.StartPath(transform.position, EnemySpawn.transform.position, OnPathComplete);
		else
			__seeker.StartPath(transform.position, LevelMaster.Instance.candyshopPosition.position, OnPathComplete);
    }

	public void OnPathComplete(Path p)
	{
		__path = p;
		__currentWaypoint = 0;
	}

	public void GemPicked()
	{
		gotGem = true;
		__gem.SetActive(true);
	}

	public void Return()
	{
		__returning = true;
	}

	public void DealDamage(int amount)
	{
		health -= amount;

		if(health <= 0)
			__SelfDestroy();
	}

	private void __SelfDestroy()
	{
		LevelMaster.Instance.EnemyDestroyed(gameObject, gotGem);
		gameObject.SetActive(false);
	}

	public void Escape()
	{
		if(gotGem)
			LevelMaster.Instance.Gems--;
	}

	void OnDisable()
	{
		__seeker.pathCallback -= OnPathComplete;
	}
}
