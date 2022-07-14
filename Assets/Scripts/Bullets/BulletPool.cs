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
    public void Init()
    {
        _bulletPrefab = Resources.Load("Bullet/PlayerBullet") as GameObject;
    }
}
