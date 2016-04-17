using UnityEngine;
using System.Collections;

public class RotatingScript : MonoBehaviour
{
	public float horizontalRotationSpeed = 0f;
	public float verticalRotationSpeed = 0f;
	public float hooverSpeed = 0f;
	public float hooverHeight = 0f;

	private float hoover;
	private Vector3 startingPos;
	private Vector3 maxPos;
	private Vector3 targetPos;
	private Vector3 lastTargetPos;

	void Start()
	{
		hoover = hooverSpeed / 10f;
		startingPos = transform.position;
		maxPos = transform.position;
		maxPos.y += hooverHeight / 10f;
		targetPos = maxPos;
		lastTargetPos = startingPos;
	}
	
	void Update()
	{
		transform.Rotate(Vector3.up, horizontalRotationSpeed * Time.deltaTime, Space.World);
		transform.Rotate(Vector3.up, verticalRotationSpeed * Time.deltaTime, Space.World);
		transform.position = Vector3.Lerp(transform.position, targetPos, hoover * Time.deltaTime);

		if(Vector3.Distance(transform.position, targetPos) <= hoover / 10f)
		{
			Vector3 tmp = targetPos;
			targetPos = lastTargetPos;
			lastTargetPos = tmp;
		}
	}
}
