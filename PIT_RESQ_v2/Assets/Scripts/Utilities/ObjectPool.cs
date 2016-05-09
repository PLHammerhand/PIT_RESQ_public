using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	public int                          count				= 10;
	public bool                         autoincrease        = true;
	public GameObject                   objectPrefab;

	private List<GameObject>            __objects;

	public int ObjectsInPool
	{
		get
		{
			return __objects.Count;
		}
	}

	public void Initialize(bool createProjectiles = true)
	{
		__objects = new List<GameObject>();

		if(createProjectiles)
		{
			for(int i = 0; i < count; i++)
			{
				GameObject go = Instantiate(objectPrefab) as GameObject;
				__objects.Add(go);
				go.SetActive(false);
			}
		}
	}

	public GameObject GetObject()
	{
		for(int i = 0; i < __objects.Count; i++)
		{
			if(!__objects[i].activeInHierarchy)
				return __objects[i];
		}

		if(autoincrease)
		{
			GameObject go = Instantiate(objectPrefab) as GameObject;
			__objects.Add(go);
			return go;
		}

		return null;
	}

	public GameObject GetObject(GameObject go)
	{
		for(int i = 0; i < __objects.Count; i++)
		{
			if(__objects[i] == go && !__objects[i].activeInHierarchy)
				return __objects[i];
		}

		if(autoincrease)
		{
			GameObject newGo = Instantiate(objectPrefab) as GameObject;
			__objects.Add(newGo);
			return newGo;
		}

		return null;
	}

	public void AddGameObject(GameObject go)
	{
		__objects.Add(go);

		if(__objects.Count > count)
			count++;
	}
}
