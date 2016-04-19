using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public bool                         ready						= false;

    private static T					__Instance;

    private static bool					__applicationIsQuitting		= false;
    private static object				__Lock						= new object();

    public static T Instance
    {
        get
        {
            if(__applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (__Lock)
            {
                if(__Instance == null)
                {
                    __Instance = (T)FindObjectOfType(typeof(T));

                    if(FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " + " - there should never be more than 1 singleton!" +
                            " Reopenning the scene might fix it.");
                        return __Instance;
                    }

                    if(__Instance == null)
					{
                        GameObject singleton = new GameObject();
                        __Instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " + __Instance.gameObject.name);
                    }
                }

                return __Instance;
            }
        }
    }

    
    public void OnDestroy()
    {
        __applicationIsQuitting = true;
    }

	/// <summary>
	/// NOT SAFE SINGLETON METHOD
	/// Use only for reseting singletons or for OnDestroy() case
	/// </summary>
	/// <param name="confirmation"></param>
	public virtual void SelfDestroy(bool confirmation = false)
	{
		if(confirmation)
		{
			//	TODO	Singleton destruction
		}
	}
}