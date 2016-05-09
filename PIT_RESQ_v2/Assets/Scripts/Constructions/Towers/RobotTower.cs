using UnityEngine;
using System.Collections;
using System;

public class RobotTower : ProjectileTower
{
	public static int           Level                   = 1;

	public float                fireAngle               = 4f;
	[Range(20f, 50f)]
	public float                rotationSpeed           = 20f;

	private int					__currentMuzzleNo		= 0;
	private Transform           __turret;
	private Transform           __rotator;
	private Transform           __weapons;
	private Transform           __aimer;

	void Awake()
	{
		__turret = gameObject.transform.Find("Turret");
		__weapons = __turret.FindChild("Weapons");

		__rotator = gameObject.transform.FindChild("Rotator");
		__aimer = __rotator.FindChild("Aimer");

		projectilePrefab = Resources.Load("Towers/Projectiles/RobotBullet") as GameObject;

		if(gameObject.GetComponent<ObjectPool>() == null)
			Init();
	}

	protected override void Start()
	{
		base.Start();

		muzzle = gameObject.transform.FindChild("Turret/Weapons").GetComponentsInChildren<Transform>();
	}

	protected override void Update()
	{
		if(_nextFireTime <= 0f)
		{
			if(_target != null)
			{
				Debug.Log("Target: " + _target.GetInstanceID());

				if(_target.activeInHierarchy)
				{
					__rotator.LookAt(_target.transform);
					__rotator.eulerAngles = new Vector3(0, __rotator.eulerAngles.y, 0);
					__aimer.LookAt(_target.transform);

					if(Vector3.Angle(transform.position, _target.transform.position) < fireAngle)
					{
						__AimAtTarget();
						Fire();
					}
					else
						__Rotate();
				}
				else
				{
					_targetsList.Remove(_target);
					_target = _NextTarget();
				}
			}
		}
		else
			_nextFireTime -= Time.deltaTime;
	}

	public override void Fire()
	{
		GameObject bullet = _projectiles.GetObject();
		bullet.transform.position = muzzle[__currentMuzzleNo].transform.position;
		bullet.transform.rotation = muzzle[__currentMuzzleNo].transform.rotation;
		__SetBulletProperties(bullet.GetComponent<RobotBullet>());
		bullet.SetActive(true);

		__NextMuzzleIndex();
		_nextFireTime = _refireTime;
	}

	private void __SetBulletProperties(RobotBullet bullet)
	{
		_SetProjectileProperties(bullet);
		bullet.lifetime = (range / projectileSpeed) * 1.5f;
	}

	private void __Rotate()
	{
		__turret.rotation = Quaternion.Lerp(__turret.rotation, __rotator.rotation, Time.deltaTime * rotationSpeed);
		__weapons.rotation = Quaternion.Lerp(__weapons.rotation, __aimer.rotation, Time.deltaTime * rotationSpeed);
		__weapons.eulerAngles = new Vector3(__weapons.eulerAngles.x, __turret.eulerAngles.y, __weapons.eulerAngles.z);
	}

	private void __AimAtTarget()
	{
		__turret.rotation = __rotator.rotation;
		__weapons.rotation = __aimer.rotation;
	}

	private void __NextMuzzleIndex()
	{
		__currentMuzzleNo++;

		if(__currentMuzzleNo >= muzzle.Length)
			__currentMuzzleNo = 0;
	}
}
