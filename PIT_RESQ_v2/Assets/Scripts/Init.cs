using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour
{
	public int                  courierCooldown				= 30;

	public LayerMask            constructionLayer;

	void Start()
	{
		TimeManager.Instance.Initialize();
		CostsManager.Instance.Initialize();
		InputManager.Instance.Initialize();

		LevelMaster.Instance.Initialize();

		BuildingManager.Instance.Initialize();

		GUIManager.Instance.Initialize();
	}

	void Update()
	{
		if(GUIManager.Instance.ready)
		{
			__StartGame();

			Destroy(gameObject);
		}
	}

	private void __StartGame()
	{
		BuildingManager.Instance.Money = 500;
		BuildingManager.Instance.constructionLayer = constructionLayer;

		GUIManager.Instance.courierCooldown = courierCooldown;
	}
}
