using UnityEngine;
using System.Collections;

public class AstarOffScript : MonoBehaviour
{

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWithGem")
		{
			other.gameObject.SendMessage("disableAstar", null, SendMessageOptions.DontRequireReceiver);
		}
	}
}
