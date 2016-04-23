using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
	[Range(0f, 90f)]
	public float            xRotation				= 45f;
	[Range(0f, 90f)]
	public float            yRotation               = 45f;
	[Range(0f, 90f)]
	public float            zRotation               = 45f;


	void Update()
	{
		gameObject.transform.Rotate(xRotation * Time.deltaTime, yRotation * Time.deltaTime, zRotation * Time.deltaTime, Space.World);
	}
}
