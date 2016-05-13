using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Laser : Projectile
{
	public AlienTower               parentTower;

	private UnityAction             __targetListener;
	private GameObject              __target;
	private LineRenderer            __laser;

	public GameObject Target
	{
		get
		{
			return __target;
		}
		set
		{
			__target = value;

			if(value == null)
				gameObject.SetActive(false);
		}
	}

	void Awake()
	{
		__laser = gameObject.AddComponent<LineRenderer>();
		__laser.material = new Material(Shader.Find("Particles/Additive"));
		__laser.SetColors(Color.red, Color.red);
		__laser.SetWidth(0.25f, 0.25f);

		__laser.SetVertexCount(2);

		__targetListener = new UnityAction(__CheckTarget);
	}

	void Start()
	{
		__laser.SetPosition(0, gameObject.transform.position);
	}

	protected override void Update()
	{
		if(__target == null || !__target.activeInHierarchy)
			__HoldFire();
		else
			__laser.SetPosition(1, __target.transform.position);
	}

	private void __HoldFire()
	{
		__target = null;
		gameObject.SetActive(false);
	}

	private void __CheckTarget()
	{
		if(!parentTower.IsTarget(Target))
			Target = null;
	}

	void OnEnable()
	{
		if(__target == null)
		{
			gameObject.SetActive(false);
			return;
		}

		if(parentTower != null)
			parentTower.targetCheck.AddListener(__targetListener);

		__laser.SetPosition(1, __target.transform.position);
	}

	void OnDisable()
	{
		if(parentTower != null)
			parentTower.targetCheck.RemoveListener(__targetListener);
	}
}
