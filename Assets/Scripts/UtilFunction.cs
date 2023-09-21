using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UtilFunction
{
	public static T AddAndGetComponent<T>(Transform parent) where T : MonoBehaviour
	{
		if (parent == null)
		{
			Debug.LogError("Cannot add component because argument parent is NULL.");
			return null;
		}
		return parent.AddComponent<T>();
	}
	public static T AddAndGetComponent<T>(bool dontDestroyOnLoad = false) where T : MonoBehaviour
	{
		T result = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
		if (dontDestroyOnLoad == true)
		{
			Object.DontDestroyOnLoad(result.gameObject);
		}
		return result;
	}
	public static void DestroyIfNotNull(GameObject variable)
	{
		if (variable != null)
		{
			Object.Destroy(variable);
		}
	}
	public static void DestroyIfNotNull<T>(T variable) where T : MonoBehaviour
	{
        if (variable != null)
        {
            Object.Destroy(variable.gameObject);
        }
    }
	public static void PauseTime()
	{
		SetTimeScale(0);
	}
	public static void PlayTime()
	{
		SetTimeScale(1);
	}
	public static void SetTimeScale(float value)
	{
		Time.timeScale = value;
	}
}
