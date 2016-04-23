using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour
{
	public LayerMask            constructionLayer;

	void Start()
	{
		LevelMaster.Instance.Initialize();

		BuildingManager.Instance.Initialize();
		BuildingManager.Instance.constructionLayer = constructionLayer;

		GUIManager.Instance.Initialize();

	}
}
