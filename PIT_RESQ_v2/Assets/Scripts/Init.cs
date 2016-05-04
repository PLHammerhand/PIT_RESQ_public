using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour
{
	public LayerMask            constructionLayer;

	void Start()
	{
		TimeManager.Instance.Init();
		CostsManager.Instance.Init();
		InputManager.Instance.Init();

		LevelMaster.Instance.Init();

		BuildingManager.Instance.Init();
		BuildingManager.Instance.constructionLayer = constructionLayer;

		GUIManager.Instance.Init();
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
