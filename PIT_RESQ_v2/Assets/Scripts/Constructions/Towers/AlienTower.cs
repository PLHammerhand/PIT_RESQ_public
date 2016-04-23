using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AlienTower : BaseTower
{
	public int						damageLevel		= 1;
	public int						rangeLevel		= 1;

	private List<GameObject>        __targets;

	public int LaserCount
	{
		get
		{
			return __lasers.Count;
		}
	}

	private List<LineRenderer>		__lasers;

	void Awake()
	{
		__lasers = new List<LineRenderer>();
	}

	protected override void Start()
	{
		base.Start();

		AddLaser();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Fire()
	{

	}

	public void AddLaser()
	{
		GameObject go = new GameObject();
		LineRenderer line = go.AddComponent<LineRenderer>();
        __lasers.Add(line);
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetColors(Color.red, Color.red);
		line.SetWidth(0.25f, 0.25f);
		line.enabled = false;
	}
}
