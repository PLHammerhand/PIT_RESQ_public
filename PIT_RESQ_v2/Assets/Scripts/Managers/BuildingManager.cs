using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : Singleton<BuildingManager>
{
	public GameObject						firerateBoostParticles;
	public GameObject						damageBoostParticles;
	public GameObject						rangeBoostParticles;

	public List<ConstructionPosition>       constructionPositions;


	void Awake()
	{
		constructionPositions = new List<ConstructionPosition>();
	}

	void Start()
	{
		constructionPositions.AddRange(GameObject.FindObjectsOfType<ConstructionPosition>());
	}

	void Update()
	{

	}
}
