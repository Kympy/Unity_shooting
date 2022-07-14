using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class, new()
{
    private static volatile T _instance = null;
    private static object _lock = new object();
	private static bool _isShuttingDown = false;
    public static T Instance
    {
		get
		{
			//Debug.Log($"##MonoBehaviourSingleton : " + typeof(T).ToString());
			if (_isShuttingDown) return null;
			if (_instance == null)
			{
				// find
				_instance = GameObject.FindObjectOfType(typeof(T)) as T; // 이미 있으면 가져오기
				if (_instance == null)
				{
					lock(_lock)
                    {
						// Create
						GameObject obj = new GameObject(typeof(T).ToString(), typeof(T));
						_instance = obj.GetComponent<T>();
						DontDestroyOnLoad(obj);
						//_instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
						if (_instance == null)
						{
							Debug.LogError("##[Error]MonoBehaviourSingleton Instance Init ERROR - " + typeof(T).ToString());
						}
					}
				}
			}
			return _instance;
		}
	}
}
