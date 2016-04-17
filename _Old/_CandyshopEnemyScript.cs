using UnityEngine;
using System.Collections;

public class CandyshopEnemyScript : MonoBehaviour
{
	public GameObject[] gems;
	
	private LevelScript levelMaster;
	private int counter;
	
	void Start()
	{
		levelMaster = GameObject.FindGameObjectWithTag("LevelMaster").GetComponent<LevelScript>();
		counter = gems.Length-1;
	}
	
	void Update()
	{
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			if(counter >= 0)
			{
				StealGem();
				other.gameObject.SendMessage("SetGem", null, SendMessageOptions.DontRequireReceiver);
			}
			else other.gameObject.SendMessage("ReturnToSpawn", null, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void returnGem()
	{
		counter++;
		gems[counter].GetComponent<MeshRenderer>().enabled = true;
	}
	
	public void StealGem()
	{
		gems[counter].GetComponent<MeshRenderer>().enabled = false;
		counter--;
	}
}
