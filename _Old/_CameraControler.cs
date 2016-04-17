using UnityEngine;
using System.Collections;

public class CameraControler : MonoBehaviour
{
	public float moveSpeed = 20f;
	public float zoomSpeed = 10f;

	public float westBorder = -4f;
	public float eastBorder = 4f;
	public float northBorder = 10f;
	public float southBorder = -14f;
	public bool enableZoom = false;
	public float topBorder = 8f;
	public float bottomBorder = 14f;

	public float mouseMovement = 0.05f;

	public GameObject spawn;
	public GameObject leftSpawnPosition;
	public GameObject rightSpawnPosition;
	public GameObject arrow;

	private float xMoveSpeed;
	private float yMoveSpeed;
	private float zMoveSpeed;

	void Start()
	{
		arrow.GetComponent<Renderer>().enabled = false;
		xMoveSpeed = moveSpeed;
		zMoveSpeed = moveSpeed;
		yMoveSpeed = zoomSpeed;
	}

	void Update()
	{
		//		========	MOVEMENT	========

		// Z axis - camera forward/backward
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if(transform.position.z < northBorder)
				transform.Translate (Vector3.forward * zMoveSpeed * Time.deltaTime, Space.World);

			/*if(transform.position.x >= westBorder && transform.position.x <= eastBorder)
				transform.Translate(Vector3.right * xMoveSpeed * Time.deltaTime, Space.World);
			if(transform.position.x <= eastBorder && transform.position.x >= westBorder)
				transform.Translate(Vector3.left * xMoveSpeed * Time.deltaTime, Space.World);*/
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if(transform.position.z > southBorder)
				transform.Translate (Vector3.back * zMoveSpeed * Time.deltaTime, Space.World);

			/*if(transform.position.x >= westBorder && transform.position.x <= eastBorder)
				transform.Translate(Vector3.right * xMoveSpeed * Time.deltaTime, Space.World);
			if(transform.position.x <= eastBorder && transform.position.x >= westBorder)
				transform.Translate(Vector3.left * xMoveSpeed * Time.deltaTime, Space.World);*/
		}


		// X axis - camera left/right
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if(transform.position.x > westBorder)
				transform.Translate (Vector3.left * xMoveSpeed * Time.deltaTime, Space.World);

			/*if(transform.position.z >= northBorder && transform.position.z <= southBorder)
				transform.Translate(Vector3.forward * xMoveSpeed * Time.deltaTime, Space.World);
			if(transform.position.z <= northBorder && transform.position.z >= southBorder)
				transform.Translate(Vector3.back * xMoveSpeed * Time.deltaTime, Space.World);*/
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if(transform.position.x < eastBorder)
				transform.Translate (Vector3.right * xMoveSpeed * Time.deltaTime, Space.World);
		}

		if(Screen.fullScreen)
		{
			//	front-back movement
			if((Input.mousePosition.y / Screen.height) < mouseMovement)
			{
				if(transform.position.z > southBorder)
					transform.Translate (Vector3.back * zMoveSpeed * Time.deltaTime, Space.World);
			}
			if((Input.mousePosition.y / Screen.height) > (1 - mouseMovement))
			{
				if(transform.position.z < northBorder)
					transform.Translate (Vector3.forward * zMoveSpeed * Time.deltaTime, Space.World);
			}
			
			// left-right movement
			if((Input.mousePosition.x / Screen.width) < mouseMovement)
			{
				if(transform.position.x > westBorder)
					transform.Translate (Vector3.left * xMoveSpeed * Time.deltaTime, Space.World);
			}
			if((Input.mousePosition.x / Screen.width) > (1 - mouseMovement))
			{
				if(transform.position.x < eastBorder)
					transform.Translate (Vector3.right * xMoveSpeed * Time.deltaTime, Space.World);
			}
		}

		// Y axis - zooming
		if(enableZoom)
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				if(transform.position.y > bottomBorder)
					transform.Translate (Vector3.down * yMoveSpeed * Time.deltaTime, Space.World);
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				if(transform.position.y < topBorder)
					transform.Translate (Vector3.up * yMoveSpeed * Time.deltaTime, Space.World);
			}
		}
	//		========		OTHER		========

		arrow.transform.LookAt(spawn.transform.position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hit;

		if(leftSpawnPosition.GetComponent<Renderer>().isVisible || rightSpawnPosition.GetComponent<Renderer>().isVisible)
		{
			arrow.GetComponent<BoxCollider>().enabled = false;
			arrow.GetComponent<Renderer>().enabled = false;
		}
		else
		{
			arrow.GetComponent<BoxCollider>().enabled = true;
			arrow.GetComponent<Renderer>().enabled = true;
			if(Physics.Raycast(ray, out hit, 5f))
			{
				if(hit.collider.tag == "Spawn")
				{
					if(Input.GetMouseButton(0))
					{
						Vector3 spawnPos = new Vector3((spawn.transform.position.x - transform.position.x), 0f, (southBorder - transform.position.z));
						transform.Translate(spawnPos, Space.World);
					}
				}
			}
		}
	}
}
