using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DestroyBullet", 0.2f);
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
