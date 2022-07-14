using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DestroyBullet", 0.2f); // 총알 0.2초 후 큐에 반환
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * 800 * Time.deltaTime);
    }
    private void DestroyBullet()
    {
        GameManager.Instance._BulletPool.ReturnBullet(this);
    }
}
