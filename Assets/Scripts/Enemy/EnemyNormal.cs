using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : Enemy
{
    private float speed = 10f;
    private Rigidbody rigidBody;
    private Vector3 screenPoint;
    private bool isIn = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        //Invoke("DestroyEnemy", 1f);
    }
    private void FixedUpdate()
    {
        Move();
        CheckTargetInCamera();
    }
    public void CheckTargetInCamera()
    {
        // ========================= 카메라 시야에 들어오면 타겟 후보 목록에 올림 ===================== //
        screenPoint = GameManager.Instance.mainCamera.WorldToViewportPoint(transform.position);
        if (screenPoint.z > 0 &&
            screenPoint.x > 0 && screenPoint.x < 1 &&
            screenPoint.y > 0 && screenPoint.y < 1) // 시야에 들어오면
        {
            if(isIn == false) // 목록에 없다면
            {
                //Debug.Log("In");
                GameManager.Instance.Target.Add(this.gameObject); // 추가
                isIn = true;
            }
        }
        else // 시야를 벗어나면
        {
            //Debug.Log("Out");
            GameManager.Instance.Target.Remove(this.gameObject); // 목록에서 제거
            isIn = false;
        }
    }
    public override void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    public override void DestroyEnemy()
    {
        //GameManager.Instance._EnemyPool.ReturnEnemy(this);
        GameManager.Instance.Target.Remove(this.gameObject); // 목록에서 제거
        Destroy(this.gameObject);
        //rigidBody.useGravity = true;
        //rigidBody.drag = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            Debug.Log("## Enemy Collision with Bullet");
            //GameManager.Instance._BulletPool.ReturnBullet(other.GetComponent<Bullet>()); // 총알 삭제

            DestroyEnemy(); // 적 삭제
        }
        else if (other.gameObject.tag == "Missile")
        {
            Debug.Log("## Enemy Collision with Missile");
            //GameManager.Instance._BulletPool.ReturnBullet(other.GetComponent<Bullet>()); // 총알 삭제

            DestroyEnemy(); // 적 삭제
        }
    }
}
