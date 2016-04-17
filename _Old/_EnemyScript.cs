using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
	public float enemyHealth;
	public float enemySpeed;
	public int enemyValue;
	public Path path;
	public float maxWaypointDistance = 0.2f;
	public GameObject gem;
	public GameObject smoke;
	public GameObject shield;
	public float shieldHealth;
	public GameObject enemyMesh;
	
	private float enemyHP;
	private float shieldHP;
	private float enemyActualSpeed;
	private bool waypointsExists = true;
	private int currentWaypoint;
	private GameObject mainCamera;
	private GameObject levelMaster;
	private Seeker seeker;
	private bool returning = false;
	private Transform mainSpawn;
	private GameObject targetGem;
	private GameObject candyshop;

	void Awake()
	{
		mainSpawn = GameObject.FindGameObjectWithTag("Spawn").transform;
		levelMaster = GameObject.FindGameObjectWithTag("LevelMaster");
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		candyshop = GameObject.FindGameObjectWithTag("Candyshop");
	}

	void Start()
	{
		enemyHP = enemyHealth;
		shieldHP = shieldHealth;
		gem.GetComponent<MeshRenderer>().enabled = false;
		enemyActualSpeed = enemySpeed / 10f;
		seeker = GetComponent<Seeker>();
		
		smoke.SetActive(false);
		
		CalculateTarget();
	}
	
	public void OnPathComplete(Path pat)
	{
		path = pat;
		currentWaypoint = 0;
	}
	
	void Update()
	{
		if(path == null) return;
		
		if(currentWaypoint >= path.vectorPath.Count)
		{
			if(returning) seeker.StartPath(transform.position, mainSpawn.transform.position, OnPathComplete);
			else return;
		}
		
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		dir *= enemyActualSpeed * Time.deltaTime;
		transform.Translate(dir);
		
		if(Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < maxWaypointDistance)
		{
			if(currentWaypoint == path.vectorPath.Count - 1 && returning)
			{
				seeker.StartPath(transform.position, mainSpawn.transform.position, OnPathComplete);
				return;
			}
			else
			{
				currentWaypoint++;
				enemyMesh.transform.LookAt(path.vectorPath[currentWaypoint]);
				return;
			}
		}
	}

	public void SetGem()
	{
		gem.GetComponent<MeshRenderer>().enabled = true;
		returning = true;
		tag = "EnemyWithGem";
		CalculateTarget();
	}
	
	public bool GetGem()
	{
		return gem.GetComponent<MeshRenderer>().enabled;
	}
	
	public bool GetReturn()
	{
		return returning;
	}
	
	public void ReturnToSpawn()
	{
		returning = true;
		CalculateTarget();
	}
	
	public void CalculateTarget()
	{
		if (!returning)	seeker.StartPath(transform.position, candyshop.transform.position, OnPathComplete);
		else seeker.StartPath(transform.position, mainSpawn.position, OnPathComplete);
	}
	
	public void CalculateTarget(Vector3 v)
	{
		seeker.StartPath(transform.position, v, OnPathComplete);
	}

	
	public void SelfDestruction()
	{
		levelMaster.SendMessage("DestroyEnemy", null, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
	
	public void GetDamage(float dmg)
	{
		enemyHP -= dmg;
		
		if(enemyHP < (enemyHealth * 0.75f)) smoke.SetActive(true);
		
		if(enemyHP <= 0f)
		{
			mainCamera.SendMessage("AddMoney", enemyValue, SendMessageOptions.DontRequireReceiver);
			mainCamera.SendMessage("showMoney", null, SendMessageOptions.DontRequireReceiver);

			if(gem.GetComponent<MeshRenderer>().enabled == true)
				levelMaster.SendMessage("DropGem", transform, SendMessageOptions.DontRequireReceiver);
			
			SelfDestruction();
		}
	}
	
	public void DamageShield(float dmg)
	{
		shieldHP -= dmg;
		if(shieldHP <= 0)
		{
			shield.GetComponent<SphereCollider>().enabled = false;
			shield.GetComponent<Renderer>().enabled = false;
		}
	}

	public void Escape()
	{
		if(tag == "EnemyWithGem") levelMaster.SendMessage("StealGem", true, SendMessageOptions.DontRequireReceiver);
		else levelMaster.SendMessage("StealGem", false, SendMessageOptions.DontRequireReceiver);

		Destroy(gameObject);
	}
}


public class EnemyInfo
{
	public Vector3 pos;
	public int gem;
	
	public EnemyInfo(Vector3 position, int gemNo)
	{
		pos = position;
		gem = gemNo;
	}
}
