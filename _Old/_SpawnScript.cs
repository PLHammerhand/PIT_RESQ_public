using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour
{
	public Transform leftBorder;
	public Transform rightBorder;
	public float speed = 10f;
	public GameObject spaceshipModel;
	public float rotationSpeed = 10f;

	private LevelScript levelScript;
	private Transform targetPos;
	private Transform otherPos;
	private float speedScaled;
	private bool moving = true;
	private float rotation = 0f;

	void Start()
	{
		levelScript = GameObject.FindGameObjectWithTag("LevelMaster").GetComponent<LevelScript>();
		targetPos = rightBorder;
		otherPos = leftBorder;
		speedScaled = speed / 50f;
	}
	
	void Update()
	{
		if(moving)
		{
			Vector3 spaceshipRotation = new Vector3(targetPos.position.x, spaceshipModel.transform.position.y, targetPos.position.z);
			spaceshipModel.transform.rotation.SetLookRotation(spaceshipRotation);
			if(Vector3.Distance(transform.position, targetPos.position) > 1.5f) transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime * speedScaled);
			else moving = false;
		}
		else
		{
			rotation += Time.deltaTime * rotationSpeed;
			spaceshipModel.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.Self);

			if(rotation >= 180f)
			{
				moving = true;
				rotation = 0f;
				Transform tmp = targetPos;
				targetPos = otherPos;
				otherPos = tmp;
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWithGem")
		{
			if(other.GetComponent<EnemyScript>().GetReturn())
			{
				levelScript.DestroyEnemy();
				other.gameObject.SendMessage("Escape", null, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
