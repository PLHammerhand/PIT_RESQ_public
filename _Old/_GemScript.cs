using UnityEngine;
using System.Collections;
using Pathfinding;

public class GemScript : MonoBehaviour
{
	private LevelScript levelMaster;
	private bool isTarget = false;
	private GameObject enemyTarget;
	
	void Start()
	{
		levelMaster = GameObject.FindGameObjectWithTag("LevelMaster").GetComponent<LevelScript>();
	}
	
	void LateUpdate()
	{
		if(!isTarget)
		{
			float maxDist = 9001f;
			GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
			
			
			if(en.Length == 1)
			{
				enemyTarget = en[0].gameObject;
				Debug.Log("Message!");
				enemyTarget.GetComponent<EnemyScript>().CalculateTarget(transform.position);
				isTarget = !isTarget;
			}
			
			if(en.Length > 1)
			{
				foreach(GameObject e in en)
				{
					float x = Vector3.Distance(transform.position, e.transform.position);
					if (x < maxDist)
					{
						Debug.Log("Counted!");
						enemyTarget = e;
						maxDist = x;
					}
				}
				enemyTarget.GetComponent<EnemyScript>().CalculateTarget(transform.position);
				isTarget = !isTarget;
			}
		}
		else
		{
			if(!enemyTarget)
			{
				isTarget = !isTarget;
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			levelMaster.gemPanel.GetComponent<TweenPosition>().Play(false);
			other.gameObject.SendMessage("SetGem", null, SendMessageOptions.DontRequireReceiver);
			Destroy(gameObject);
		}
	}
	
}
