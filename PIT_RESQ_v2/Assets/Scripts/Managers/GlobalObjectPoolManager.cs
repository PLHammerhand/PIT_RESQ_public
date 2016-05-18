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
			go.name = pooled.name + " " + go.GetInstanceID();
			outList.Add(go);

			return go;
		}
		else
			return __CreateKeyListPair(pooled);
	}

	public void AddGameObject(GameObject prefab, GameObject go)
	{
		List<GameObject> list;
		go.SetActive(false);

		if(__objects.TryGetValue(prefab, out list))
			list.Add(go);
		else
			__CreateKeyListPair(prefab, go);
	}

	public bool CheckIfPooled(GameObject prefab, GameObject go)
	{
		List<GameObject> list;

		if(__objects.TryGetValue(prefab, out list))
		{
			if(list != null && list.Contains(go))
				return true;
		}

		return false;
	}

	private GameObject __CreateKeyListPair(GameObject prefab, GameObject go = null)
	{
		List<GameObject> list = new List<GameObject>();

		if(go == null)
		{
			go = Instantiate(prefab) as GameObject;
			go.name = prefab.name + " " + go.GetInstanceID();
			list.Add(go);
		}
		else
			list.Add(go);

		__objects.Add(prefab, list);

		return go;
	}

	public void CreateMultipleObjectsInPool(GameObject go, int number, bool createActive = false)
	{
		List<GameObject> outList;

		if(!__objects.TryGetValue(go, out outList))
		{
			__CreateKeyListPair(go).SetActive(createActive);
			__objects.TryGetValue(go, out outList);
			number--;
        }

		if(outList == null)
		{
			Debug.LogError("No object pool for " + go.name + " was created!");
			return;
		}

		for(int i = 0; i < number; i++)
		{
			GameObject tmpGO = Instantiate(go) as GameObject;
			tmpGO.name = go.name + " " + tmpGO.GetInstanceID();
			outList.Add(tmpGO);
			tmpGO.SetActive(createActive);
		}
	}
}
