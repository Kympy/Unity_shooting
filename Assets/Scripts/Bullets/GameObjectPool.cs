using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class GameObjectPool : MonoBehaviour, IInitialize
{
	private GameObject _bulletPrefab;                   // ===== 총알 프리팹
	private Queue<Bullet> poolingQueue = new Queue<Bullet>(); // ==== 총알 담을 큐
	private int i = 0;

	private Dictionary<PoolObjectKey, Queue<GameObject>> gameObjectPool = new Dictionary<PoolObjectKey, Queue<GameObject>>();
	private const int InitCreationCount = 10;
	public async Task Initialize()
	{
		_bulletPrefab = await ResourceManager.LoadGameObject("Bullet/PlayerBullet");
	}
	public GameObject GetObject(PoolObjectKey resourceName)
	{
		if (gameObjectPool.ContainsKey(resourceName) == false)
		{
			Debug.LogError($"Pool : {resourceName} is not in dictionary.");
			return null;
		}
		GameObject obj = null;
		//if (gameObjectPool.ContainsKey(resourceName) == false)
		//{
		//	await CreateNewQueue(resourceName);
		//	obj = gameObjectPool[resourceName].Dequeue();
		//}
		// 1개 이하로 남았을 때, 미리 생성 요청
		if (gameObjectPool[resourceName].Count <= 1)
		{
			// 대기하지 않는다.
			CreateObject(resourceName, InitCreationCount);
		}

		obj = gameObjectPool[resourceName].Dequeue();
		obj.transform.SetParent(null);
		obj.SetActive(true);
		return obj;
	}
	public async Task CreateNewQueue(PoolObjectKey resourceName)
	{
		GameObject obj = await ResourceManager.LoadGameObject(PoolObject.GetPath(resourceName));
		if (obj == null)
		{
			Debug.LogError($"Pool : Object creation is failed. {resourceName}");
			return;
		}
		gameObjectPool.Add(resourceName, new Queue<GameObject>());

		GameObject cachedCreated = null;
		for (int i = 0; i < InitCreationCount; i++)
		{
			cachedCreated = Instantiate(obj);
			cachedCreated.transform.SetParent(this.transform);
			cachedCreated.gameObject.SetActive(false);
			gameObjectPool[resourceName].Enqueue(cachedCreated);
		}
		cachedCreated = null;
	}
	public async Task CreateObject(PoolObjectKey resourceName, int creationCount = 1)
	{
		GameObject obj = await ResourceManager.LoadGameObject(PoolObject.GetPath(resourceName));
		if (obj == null)
		{
			Debug.LogError($"Pool : Object creation is failed. {resourceName}");
			return;
		}
		GameObject cached = null;
		for (int i = 0; i < creationCount; i++)
		{
			cached = Instantiate(obj);
			cached.transform.SetParent(this.transform);
			cached.SetActive(false);
			gameObjectPool[resourceName].Enqueue(cached);
		}
	}
	public void ReturnObject(GameObject obj, PoolObjectKey resourceName)
	{
		if (gameObjectPool.ContainsKey(resourceName) == false)
		{
			Debug.LogError($"Pool : No key {resourceName} but try to enqueue object");
			return;
		}
		obj.transform.SetParent(this.transform);
		obj.gameObject.SetActive(false);
		gameObjectPool[resourceName].Enqueue(obj);
	}
	private void Start()
	{
		//if (_bulletPrefab == null) Debug.Log("NULL OBJECT");
		//InitQueue(20);
	}
	private void InitQueue(int QueueSize) // 큐 초기화
	{
		//Debug.Log("## BulletPool InitQueue start");
		for (int i = 0; i < QueueSize; i++)
		{
			Bullet obj = CreateBullet();
			obj.name = "originBullet_" + i;
			poolingQueue.Enqueue(obj);
		}
	}
	private Bullet CreateBullet() // 총알 프리팹 생성
	{
		Bullet obj = Instantiate(_bulletPrefab).GetComponent<Bullet>();
		obj.name = "newCreatedBullet_" + i++;
		obj.gameObject.SetActive(false);
		obj.transform.SetParent(transform);
		return obj;
	}
	public Bullet GetBullet() // 큐에서 총알 꺼내기
	{
		if (poolingQueue.Count > 0)
		{
			Bullet obj = poolingQueue.Dequeue();
			//Debug.Log("Dequeue End");
			obj.transform.SetParent(null);
			obj.gameObject.SetActive(true);
			return obj;
		}
		else // 큐에 총알이 없으면 생성해서 집어넣음
		{
			poolingQueue.Enqueue(CreateBullet());
			//Debug.Log("Enqueue New One");
			Bullet obj = poolingQueue.Dequeue();
			//Debug.Log("Dequeue New One");
			obj.transform.SetParent(null);
			obj.gameObject.SetActive(true);
			return obj;
		}
	}
	public void ReturnBullet(Bullet obj) // 큐에 총알 반환
	{
		obj.gameObject.SetActive(false);
		obj.transform.SetParent(transform);

		poolingQueue.Enqueue(obj);
		//Debug.Log("Return finish");
	}
}
