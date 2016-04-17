using UnityEngine;
using System.Collections;

public class EnemyMeshFacing : MonoBehaviour
{
	//public GameObject self;
	private Vector3 lastPosition;
	private Vector3 currentPosition;

	void Start()
	{
		currentPosition = transform.position;
	}
	
	void Update()
	{
		lastPosition = currentPosition;
		transform.LookAt(lastPosition);
		currentPosition = transform.position;
	}
}
