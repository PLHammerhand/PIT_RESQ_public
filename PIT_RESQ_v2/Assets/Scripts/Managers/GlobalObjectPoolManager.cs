using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalObjectPoolManager : Singleton<GlobalObjectPoolManager>
{
	private Dictionary<GameObject, List<GameObject>>			__objects				= new Dictionary<GameObject, List<GameObject>>();

	public GameObject GetGameObject(GameObject pooled)
	{
		List<GameObject> outList;

		if(__objects.TryGetValue(pooled, out outList))
		{
			GameObject go;
			for(int i = 0; i < outList.Count; i++)
			{
				if(!outList[i].activeInHierarchy)
					return outList[i];
			}

			go = Instantiate(pooled) as GameObject;
			outList.Add(go);

			return go;
		}
		else return __CreateKeyListPair(pooled);
	}

	public void AddGameObject(GameObject go)
	{
		List<GameObject> list;
		go.SetActive(false);

		if(__objects.TryGetValue(go, out list))
			list.Add(go);
		else
			__CreateKeyListPair(go);
	}

	private GameObject __CreateKeyListPair(GameObject go)
	{
		List<GameObject> list = new List<GameObject>();
		GameObject tmpGO = Instantiate(go) as GameObject;
		tmpGO.name = go.name + " " + tmpGO.GetInstanceID();
		list.Add(tmpGO);
		tmpGO.SetActive(false);
		__objects.Add(go, list);

		return go;
	}

	public void CreateMultipleObjectsInPool(GameObject go, int number)
	{
		List<GameObject> outList;

		if(!__objects.TryGetValue(go, out outList))
			__CreateKeyListPair(go);

		if(outList == null)
			outList = new List<GameObject>();

		for(int i = 0; i < number; i++)
		{
			GameObject tmpGO = Instantiate(go) as GameObject;
			tmpGO.name = go.name + " " + tmpGO.GetInstanceID();
			outList.Add(tmpGO);
			tmpGO.SetActive(false);
		}
	}
}
