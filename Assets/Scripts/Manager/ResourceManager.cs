using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceManager
{
	public static async Task<GameObject> LoadGameObject(string path)
	{
		try
		{ 
			var handle = Addressables.LoadAssetAsync<GameObject>(path).Task;
			await handle;
			if (handle.Result == null)
			{
				throw new System.Exception($"{path} is NULL");
			}
			return handle.Result;
		}
		catch (System.Exception e)
		{ 
			Debug.LogError(e.Message);
			return null; 
		}
	}
	public static async Task<T> LoadGameObjectAndGetComponent<T>(string path) where T : MonoBehaviour
	{
		try
		{
			var handle = Addressables.LoadAssetAsync<GameObject>(path).Task;
			await handle;
			if (handle.Result == null)
			{
				throw new System.Exception($"{path} is NULL");
			}
			return handle.Result.GetComponent<T>();
		}
		catch (System.Exception e)
		{
			Debug.LogError(e.Message);
			return null;
		}
	}
	public static GameObject InstantiateGameObject(string path)
	{
		try
		{
			return Addressables.InstantiateAsync(path).Result;
		}
		catch (System.Exception e)
		{
			Debug.LogError(e.Message);
			return null;
		}
	}
	public static async Task<T> InstantiateGameObjectAndGetComponent<T>(string path, Transform parent = null) where T : MonoBehaviour
	{
		var handle = Addressables.InstantiateAsync(path, parent).Task;
		await handle;
		handle.Result.gameObject.SetActive(false);
		return handle.Result.GetComponent<T>();
	}
	public static void ReleaseIfNotNull<T>(T resource)
	{
		if (resource == null) return;
		Addressables.Release(resource);
	}
}
