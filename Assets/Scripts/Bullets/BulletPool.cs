using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private GameObject _bulletPrefab;                   // ===== 총알 프리팹
    private Queue<Bullet> poolingQueue = new Queue<Bullet>(); // ==== 총알 담을 큐
    private int i = 0;
    private void Awake()
    {
        _bulletPrefab = Resources.Load("Bullet/PlayerBullet") as GameObject;
    }
    private void Start()
    {
        if (_bulletPrefab == null) Debug.Log("NULL OBJECT");
        InitQueue(20);
    }
    private void InitQueue(int QueueSize)
    {
        //Debug.Log("## BulletPool InitQueue start");
        for (int i = 0; i < QueueSize; i++)
        {
            Bullet obj = CreateBullet();
            obj.name = "originBullet_" + i;
            poolingQueue.Enqueue(obj);
        }
    }
    private Bullet CreateBullet()
    {
        Bullet obj = Instantiate(_bulletPrefab).GetComponent<Bullet>();
        obj.name = "newCreatedBullet_" + i++;
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }
    public Bullet GetBullet()
    {
        if (poolingQueue.Count > 0)
        {
            Bullet obj = poolingQueue.Dequeue();
            //Debug.Log("Dequeue End");
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
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
    public void ReturnBullet(Bullet obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);

        poolingQueue.Enqueue(obj);
        //Debug.Log("Return finish");
    }
    public void Init()
    {
        _bulletPrefab = Resources.Load("Bullet/PlayerBullet") as GameObject;
    }
}
