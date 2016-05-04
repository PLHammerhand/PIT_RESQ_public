using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager>
{
	public bool             enableInput                 = true;
	public float            rayRange                    = 30f;


	private RaycastHit      __hit;

	void Update()
	{
		if(enableInput)
		{
			if(Input.GetButtonDown("Click") && TimeManager.Instance.gameplayState == true)
			{
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out __hit, rayRange))
				{
					if(__hit.collider.gameObject.layer == LayerMask.NameToLayer("Construction"))
					{
						if(BuildingManager.Instance.BuildingState == BuildingState.CONSTRUCTION)
							BuildingManager.Instance.ConstructionClick(__hit);
						else if(BuildingManager.Instance.BuildingState != BuildingState.STOP)
							BuildingManager.Instance.CheckPositionStatus(__hit);
					}
				}
				else
					GUIManager.Instance.CloseUpgradePanels();
			}
		}
	}

	void LateUpdate()
	{

	}

	public void Init()
	{
		ready = true;
	}
}
