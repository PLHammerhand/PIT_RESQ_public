using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour
{
	public LayerMask            constructionLayer;

	void Start()
	{
		TimeManager.Instance.Initialize();
		CostsManager.Instance.Initialize();
		InputManager.Instance.Initialize();

		LevelMaster.Instance.Initialize();

		BuildingManager.Instance.Initialize();
		BuildingManager.Instance.constructionLayer = constructionLayer;

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
	}
}
