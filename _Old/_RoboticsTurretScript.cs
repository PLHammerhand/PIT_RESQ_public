using UnityEngine;
using System.Collections;

public class RoboticsTurretScript : MonoBehaviour
{
	public GameObject bullet;
	public float reloadTime = 0.15f;
	public float turnSpeed = 5f;
	public Transform target;
	public Transform muzzlePosition;
	public Transform towerTurret;
	public Transform towerConnector;
	public Transform rotator;
	public Transform aimer;
	public float towerDamage = 5.5f;
	public LayerMask enemyLayer;
	public GameObject upgradeTowerPrefab;
	public int upgradeCost;
	public int towerLevel;
	public UIPanel upgradePanel;

	private float nextFireTime;
	private float nextMoveTime;
	private float damage;
	private float range;
	private PositionInfo posInf;

	void Start()
	{
		damage = towerDamage;
		range = GetComponent<CapsuleCollider>().radius;
	}
	
	void Update()
	{
		Debug.DrawRay(muzzlePosition.position, muzzlePosition.forward);

		if(target)
		{
			RaycastHit hit;
			if(Time.time >= nextMoveTime)
			{
				rotator.LookAt(target);
				rotator.eulerAngles = new Vector3(0, rotator.eulerAngles.y, 0);
				aimer.LookAt(target);

				towerTurret.rotation = Quaternion.Lerp(towerTurret.rotation, rotator.rotation, Time.deltaTime * turnSpeed);
				towerConnector.rotation = Quaternion.Lerp(towerConnector.rotation, aimer.rotation, Time.deltaTime * turnSpeed);
				towerConnector.eulerAngles = new Vector3(towerConnector.eulerAngles.x, towerTurret.eulerAngles.y, towerConnector.eulerAngles.z);
			}

			if(Time.time >= nextFireTime)
			{
				fireBullet();
				Physics.Raycast(muzzlePosition.position, muzzlePosition.forward, out hit, 25f, enemyLayer);
				if(hit.collider != null && hit.collider.tag == "Shield")
					hit.collider.gameObject.SendMessage("DamageShield", damage, SendMessageOptions.DontRequireReceiver);
				else target.SendMessage("GetDamage", damage, SendMessageOptions.DontRequireReceiver);

			}
		}

	}

	void OnTriggerStay(Collider other)
	{
		if(!target)
		{
			if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWithGem")
			{
				nextFireTime = Time.time + (reloadTime * 0.5f);
				target = other.gameObject.transform;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		target = null;
	}

	void fireBullet()
	{
		nextFireTime = Time.time + reloadTime;
		GameObject clone;
		clone = Instantiate(bullet, muzzlePosition.position, muzzlePosition.rotation) as GameObject;
		clone.GetComponent<RoboticsBulletScript>().bulletRange = GetComponent<CapsuleCollider>().radius;
	}

	public void Upgrade()
	{
		GameObject clone = Instantiate(upgradeTowerPrefab, transform.position, Quaternion.identity) as GameObject;
		clone.SendMessage("SetUpgradePanel", upgradePanel, SendMessageOptions.DontRequireReceiver);
		posInf.myPos.SendMessage("UpgradeTower", clone, SendMessageOptions.DontRequireReceiver);
		clone.GetComponent<RoboticsTurretScript>().ChangeValues(posInf);
		Destroy(gameObject);
	}

	public void SetUpgradePanel(UIPanel pan)
	{
		upgradePanel = pan;
	}

	public void ChangeValues(PositionInfo pi)
	{
		posInf = pi;
		reloadTime *= pi.boostFirerate;
		damage *= pi.boostDamage;
		GetComponent<CapsuleCollider>().radius *= pi.boostRange;
		range = GetComponent<CapsuleCollider>().radius;
	}
}
