using UnityEngine;
using System.Collections;

public class AliensTowerScript : MonoBehaviour
{
	//public GameObject target;
	public GameObject laser;
	public Transform prism;
	public float firerate = 0.2f;
	public Material mat;
	public float towerDamage = 5f;
	public UIPanel upgradePanel;
	public Light light;
	public ArrayList targets;
	public LayerMask enemyLayer;

	public int publicTotalLasers;
	public int publicFreeLasers;
	
	private float damage;
	private float nextDamageTime;
	private Color laserColor = Color.red;
	private int laserCount = 1;
	private int damageLevel = 1;
	private int rangeLevel = 1;
	private int freeLasers;
	
	void Start()
	{
		damage = towerDamage;
		freeLasers = laserCount;
		targets = new ArrayList(laserCount);

		publicTotalLasers = laserCount;
		publicFreeLasers = freeLasers;
	}
	
	void Update()
	{
		if(targets.Count > 0)
		{
			foreach(GameObject t in targets)
			{
				if(t)
				{
					Attack(t);
					RaycastHit hit;
					Physics.Raycast(prism.position, (t.transform.position - prism.position), out hit, 25f, enemyLayer);
					
					if(Time.time >= nextDamageTime)
					{
						if(hit.collider.tag == "Shield") hit.collider.gameObject.SendMessage("DamageShield", damage, SendMessageOptions.DontRequireReceiver);
						else t.SendMessage("GetDamage", damage, SendMessageOptions.DontRequireReceiver);
						
						CalculateDamageTime();
					}
				}
			}
		}
	}

	void LateUpdate()
	{
		ArrayList tmp = new ArrayList(targets);
		freeLasers = laserCount;
		targets.Clear();
		foreach(GameObject o in tmp)
		{
			if(o)
			{
				targets.Add(o);
				if(freeLasers > 0) freeLasers--;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(freeLasers > 0 && (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWithGem"))
		{
			if(!targets.Contains(other.gameObject))
			{
				freeLasers--;
				publicFreeLasers = freeLasers;
				targets.Add(other.gameObject);
				CalculateDamageTime();
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(freeLasers > 0 && (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWithGem"))
		{
			if(!targets.Contains(other.gameObject))
			{
				freeLasers--;
				targets.Add(other.gameObject);
				CalculateDamageTime();
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		targets.Remove(other.gameObject);
		if(freeLasers < laserCount) freeLasers++;
		publicFreeLasers = freeLasers;
	}
	
	void SetDamage(float dmg)
	{
		damage = dmg;
	}
	
	void Attack(GameObject target)
	{
		GameObject clone;
		clone = Instantiate(laser, prism.position, prism.rotation) as GameObject;
		clone.GetComponent<LineRenderer>().material = mat;
		clone.GetComponent<LineRenderer>().SetVertexCount(2);
		clone.GetComponent<LineRenderer>().SetWidth(0.05f, 0.05f);
		clone.GetComponent<LineRenderer>().SetColors(laserColor, laserColor);
		clone.GetComponent<LineRenderer>().SetPosition(0,prism.transform.position);
		clone.GetComponent<LineRenderer>().SetPosition(1,target.transform.position);
	}
	
	void CalculateDamageTime()
	{
		nextDamageTime = Time.time + firerate;
	}
	
	public void DisableLight()
	{
		light.enabled = false;
	}
	
	public void SetUpgradePanel(UIPanel pan)
	{
		upgradePanel = pan;
	}
	
	public void UpgradeLaser()
	{
		laserCount++;
		publicTotalLasers = laserCount;
		targets.Capacity = laserCount;
	}
	
	public void UpgradeDamage()
	{
		damageLevel++;
		damage *= 1.1f;
	}
	
	public void UpgradeRange()
	{
		rangeLevel++;
		GetComponent<CapsuleCollider>().radius *= 1.1f;
	}
	
	public void ChangeValues(PositionInfo pi)
	{
		damage *= pi.boostDamage;
		GetComponent<CapsuleCollider>().radius *= pi.boostRange;
	}
	
	public AlienUpgradeLevels GetTowerStatus()
	{
		AlienUpgradeLevels towerInfo = new AlienUpgradeLevels(laserCount, damageLevel, rangeLevel);
		return towerInfo;
	}
	
}


public class AlienUpgradeLevels
{
	public int laserLvl;
	public int damageLvl;
	public int rangeLvl;
	
	public AlienUpgradeLevels(int laser, int damage, int range)
	{
		laserLvl = laser;
		damageLvl = damage;
		rangeLvl = range;
	}
}
