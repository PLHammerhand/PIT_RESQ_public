using UnityEngine;
using System.Collections;

public class Candyshop : MonoBehaviour
{
	public int						gems;
	[HideInInspector]
	public MeshRenderer[]			gemsInCandyshop;


	void Start()
	{
		gemsInCandyshop = gameObject.transform.GetComponentsInChildren<MeshRenderer>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			Enemy enemy = other.gameObject.GetComponent<Enemy>();

			if(gems > 0)
				enemy.GemPicked();

			enemy.Return();
		}
	}
}
