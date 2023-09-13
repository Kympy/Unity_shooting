using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static volatile T instance = null;
    private static object lockObject = new object();
	private static bool isShuttingDown = false;
	public static T Instance
    {
		get
		{
			if (isShuttingDown == true) return null;

			if (instance == null)
			{
				instance = FindObjectOfType(typeof(T)) as T; // 이미 있으면 가져오기
				if (instance == null)
				{
					lock(lockObject)
                    {
						// Create
						GameObject obj = new GameObject(typeof(T).ToString(), typeof(T));
						instance = obj.GetComponent<T>();
						if (instance == null)
						{
							Debug.LogError($"Get singleton instance ERROR - {typeof(T)}");
						}
					}
				}
			}
			return instance;
		}
	}

	protected virtual void Awake()
	{
		instance = this as T;
		DontDestroyOnLoad(this);
	}
}
