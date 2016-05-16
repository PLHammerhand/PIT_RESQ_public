using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
	[Range(1f, 5f)]
	public float                speed                   = 2f;
	public bool                 circularRoute           = true;
	public GameObject[]         patrolPath;

	private int                 __waypointNumber        = 0;
	private bool                __returning             = false;


	void Start()
	{
		if(patrolPath.Length > 1)
		{
			Hashtable args = new Hashtable();
			args.Add("position", patrolPath[__waypointNumber].transform);
			args.Add("speed", speed);
			args.Add("easetype", iTween.EaseType.easeOutSine);
			args.Add("orienttopath", true);
			args.Add("oncomplete", "GoToNextWaypoint");
			args.Add("oncompletetarget", gameObject);

			iTween.MoveTo(gameObject, args);
		}
	}

	public void GoToNextWaypoint()
	{
		if(!circularRoute && __returning)
			__waypointNumber--;
		else
			__waypointNumber++;

		if(__waypointNumber == patrolPath.Length)
		{
			if(!circularRoute)
				__returning = true;
			else
				__waypointNumber = 0;
		}
		else if(__returning && __waypointNumber == 0)
			__returning = false;

		Hashtable args = new Hashtable();
		args.Add("position", patrolPath[__waypointNumber].transform);
		args.Add("speed", speed);
		args.Add("easetype", iTween.EaseType.easeInOutSine);
		args.Add("orienttopath", true);
		args.Add("oncomplete", "GoToNextWaypoint");
		args.Add("oncompletetarget", gameObject);

		iTween.MoveTo(gameObject, args);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy")
		{
			Enemy enemy = other.gameObject.GetComponent<Enemy>();

			if(enemy.Returning)
				enemy.Escape();
		}
	}
}
