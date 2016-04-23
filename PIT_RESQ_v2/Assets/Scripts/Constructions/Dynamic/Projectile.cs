using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	[HideInInspector]
	public int					damage;
	[HideInInspector]
	public float				projectileSpeed;
	[HideInInspector]
	public GameObject			hitParticles;


	protected virtual void Update()
	{
		gameObject.transform.Translate(gameObject.transform.forward * projectileSpeed * Time.deltaTime, Space.World);
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			_DealDamage(other.gameObject.GetComponent<Enemy>());
			_SelfDestroy();
		}
	}

	protected virtual void _DealDamage(Enemy enemy)
	{
		enemy.DealDamage(damage);
	}

	protected virtual void _SelfDestroy(bool hit = false)
	{
		if(hit)
		{
			GameObject particles = GlobalObjectPoolManager.Instance.GetGameObject(hitParticles);
			particles.transform.position = gameObject.transform.position;
			particles.SetActive(true);
		}

		gameObject.SetActive(false);
	}
}
