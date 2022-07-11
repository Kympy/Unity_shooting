using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    private GameObject _EnemyPrefab;
    private Queue<Enemy> poolingQueue = new Queue<Enemy>();
    private int i = 0;
    private void Start()
    {
        //Debug.Log("## EnemyPool Start");
        if (_EnemyPrefab == null) Debug.Log("NULL OBJECT");
        InitQueue(7);
    }
    private void InitQueue(int QueueSize)
    {
        //Debug.Log("## EnemyPool InitQueue start");
        for (int i = 0; i < QueueSize; i++)
        {
            Enemy obj = CreateEnemy();
            obj.name = "originEnemy_" + i;
            poolingQueue.Enqueue(obj);
        }
    }
    private Enemy CreateEnemy()
    {
        Enemy obj = Instantiate(_EnemyPrefab).GetComponent<Enemy>();
        obj.name = "newCreatedEnemy_" + i++;
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }
    public Enemy GetEnemy()
    {
        if (poolingQueue.Count > 0)
        {
            Enemy obj = poolingQueue.Dequeue();
            //Debug.Log("Dequeue End");
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            poolingQueue.Enqueue(CreateEnemy());
            //Debug.Log("Enqueue New One");
            Enemy obj = poolingQueue.Dequeue();
            //Debug.Log("Dequeue New One");
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
    }
    public void ReturnEnemy(Enemy obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);

        poolingQueue.Enqueue(obj);
        //Debug.Log("Return finish");
    }
    public void Init()
    {
        _EnemyPrefab = Resources.Load("Enemy/EnemyNormal") as GameObject;
    }
}
