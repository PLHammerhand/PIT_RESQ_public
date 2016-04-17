using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour
{
	public GameObject self;

	public void DamageShield(float dmg)
	{
		self.SendMessage("DamageShield", dmg, SendMessageOptions.DontRequireReceiver);
	}
}
