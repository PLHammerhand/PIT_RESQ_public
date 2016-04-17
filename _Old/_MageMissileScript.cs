using UnityEngine;
using System.Collections;

public class MageMissileScript : MonoBehaviour
{
	public Transform target;
	public float maxRange = 30f;
	public GameObject hit;

	private float damage = 25f;	
	private float speed = 5f;
	private float range;

	void Update()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
		range += Time.deltaTime * speed;
		if(range >= maxRange) Explode();
		
		if(target) transform.LookAt(target);
		else Explode();
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Shield")
		{
			other.gameObject.SendMessage("DamageShield", damage, SendMessageOptions.DontRequireReceiver);
			Explode();
		}
		else if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWithGem")
		{
			other.gameObject.SendMessage("GetDamage", damage, SendMessageOptions.DontRequireReceiver);
			Explode();
		}
	}
	
	void Explode()
	{
		Instantiate(hit, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	public void SetValues(float dmg, float spd)
	{
		damage = dmg;
		speed = spd;
	}
}
