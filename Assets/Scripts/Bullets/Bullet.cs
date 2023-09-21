using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public const float ReturnTime = 0.2f;

    public async void Fly()
    {
        float timer = 0f;
        while(true)
        {
            timer += Time.deltaTime;
            if (timer > ReturnTime)
            {
                DestroyBullet();
                return;
            }
			transform.Translate(Vector3.up * 800 * Time.deltaTime);
            await Task.Yield();
		}
    }
    
    private void DestroyBullet()
    {
		GameManager.Instance.GetCurrentSceneObject<IngameSceneObject>().GameObjectPool.ReturnObject(this.gameObject, PoolObjectKey.Bullet);
	}
}
