using UnityEngine;
using System.Collections;

public class RoboticsBulletScript : MonoBehaviour
{
	public float bulletSpeed = 80f;
	public float bulletRange = 25f;

	private float distance;

	void Start()
	{

	}

	void Update()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
		distance += Time.deltaTime * bulletSpeed;
		if(distance >= bulletRange) Destroy(gameObject);
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWithGem") Destroy(gameObject);
	}
}
