using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	public float            northBorder                 = 0f;
	public float            eastBorder                  = -5f;
	public float            westBorder                  = 6f;
	public float            southBorder                 = -16f;

	public bool             enableInput                 = true;
	public float            rayRange                    = 30f;
	public float            cameraMovingSpeed           = 7.5f;

	private Camera          __mainCamera;
	private RaycastHit      __hit;


	void Awake()
	{
		__mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void Update()
	{
		if(enableInput)
		{
			if(Input.GetButtonDown("Click") && TimeManager.Instance.gameplayState == true)
				__CheckClick();
		}
	}

	void LateUpdate()
	{
		Vector3 camPos = __mainCamera.transform.position;

		if(Input.GetAxis("Horizontal") > 0 && camPos.x < westBorder)
			__mainCamera.transform.Translate(Vector3.right * Time.deltaTime * cameraMovingSpeed, Space.World);
		else if(Input.GetAxis("Horizontal") < 0 && camPos.x > eastBorder)
			__mainCamera.transform.Translate(Vector3.left * Time.deltaTime * cameraMovingSpeed, Space.World);

		if(Input.GetAxis("Vertical") > 0 && camPos.z < northBorder)
			__mainCamera.transform.Translate(Vector3.forward * Time.deltaTime * cameraMovingSpeed, Space.World);
		else if(Input.GetAxis("Vertical") < 0 && camPos.z > southBorder)
			__mainCamera.transform.Translate(Vector3.back * Time.deltaTime * cameraMovingSpeed, Space.World);
	}

	private void __CheckClick()
	{
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out __hit, rayRange))
		{
			LayerMask layer = __hit.collider.gameObject.layer;

			if(layer == LayerMask.NameToLayer("Construction"))
				__ConstructionClick();
			else if(layer == LayerMask.NameToLayer("Gems"))
				LevelMaster.Instance.GemClick(__hit);
			else
			{
				GUIManager.Instance.CloseUpgradePanels();
				LevelMaster.Instance.GemClick(null);
			}
		}
		else
		{
			GUIManager.Instance.CloseUpgradePanels();
			LevelMaster.Instance.GemClick(null);
		}
	}

	public override void Initialize()
	{
		ready = true;
	}

	private void __ConstructionClick()
	{
		if(BuildingManager.Instance.BuildingState == BuildingState.CONSTRUCTION)
			BuildingManager.Instance.ConstructionClick(__hit);
		else if(BuildingManager.Instance.BuildingState != BuildingState.STOP)
			BuildingManager.Instance.CheckPositionStatus(__hit);

		LevelMaster.Instance.GemClick(null);
	}
}
