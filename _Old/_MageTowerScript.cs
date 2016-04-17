using UnityEngine;
using System.Collections;

public class MageTowerScript : MonoBehaviour
{
	public GameObject missile;
	public Transform muzzlePosition;
	public float reloadTime = 1f;	//	Shorter = faster;
	public Transform target;
	public float towerDamage = 25f;
	public float missileSpeed = 5f;
	public GameObject leftUpgradeTowerPrefab;
	public GameObject rightUpgradeTowerPrefab;
	public int upgradeCost;
	public UIPanel upgradePanel;
	public int towerLevel = 1;

	private float nextFireTime;
	private float damage;

	void Start()
	{
		damage = towerDamage;
	}
	
	void Update()
	{
		if(target)
		{
			if(Time.time >= nextFireTime) fireMissile();
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(!target)
		{
			if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWithGem")
			{
				target = other.gameObject.transform;
				nextFireTime = Time.time + reloadTime;
			}
			else if (other.gameObject.tag == "Shield")
			{
				target = other.gameObject.transform;
				nextFireTime = Time.time + reloadTime;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		target = null;
	}

	void fireMissile()
	{
		nextFireTime = Time.time + reloadTime;
		GameObject clone;
		clone = Instantiate(missile, muzzlePosition.position, muzzlePosition.rotation) as GameObject;
		clone.GetComponent<MageMissileScript>().target = target;
		clone.GetComponent<MageMissileScript>().SetValues(damage, missileSpeed);
		clone.GetComponent<MageMissileScript>().maxRange = GetComponent<CapsuleCollider>().radius;
	}

	public void UpgradeToLeft(GameObject pos)
	{
		GameObject clone = Instantiate(leftUpgradeTowerPrefab, transform.position, Quaternion.identity) as GameObject;
		clone.SendMessage("SetUpgradePanel", upgradePanel, SendMessageOptions.DontRequireReceiver);
		pos.SendMessage("BuildTower", clone, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}

	public void UpgradeToRight(GameObject pos)
	{
		GameObject clone = Instantiate(rightUpgradeTowerPrefab, transform.position, Quaternion.identity) as GameObject;
		clone.SendMessage("SetUpgradePanel", upgradePanel, SendMessageOptions.DontRequireReceiver);
		pos.SendMessage("BuildTower", clone, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}

	public void SetUpgradePanel(UIPanel pan)
	{
		upgradePanel = pan;
	}

	public void ChangeValues(PositionInfo pi)
	{
		reloadTime *= pi.boostFirerate;
		damage *= pi.boostDamage;
		GetComponent<CapsuleCollider>().radius *= pi.boostRange;
	}
}
