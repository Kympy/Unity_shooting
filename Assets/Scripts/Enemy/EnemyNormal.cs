using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : Enemy
{
    public enum State
    {
        forward,
        right,
        left,
        attack,
    }
    private float HP = 100.0f;
    private float speed = 30f; // 속도

    private Rigidbody rigidBody;

    private Vector3 screenPoint; // 스크린 좌표
    private bool isIn = false; // 화면에 들어왔는지
    private float movementTimer = 0f; // 이동 타이머
    private Quaternion direction;

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
        CheckTargetInCamera(); // 타겟 대상 체크
        Move(); // 이동
        Dead(); // 사망 판정
        movementTimer += Time.deltaTime; // 이동 타이머
    }
    // ==================================================== 적 타겟 가능 여부 체크 함수 ================================================= //
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
                if(Vector3.Distance(transform.position, GameManager.Instance._Player.transform.position) <= 2000.0f) // 2000 거리 안쪽에 있다면
                {
                    //Debug.Log("In");
                    GameManager.Instance.Target.Add(this.gameObject); // 추가
                    isIn = true;
                }
            }
        }
        else // 시야를 벗어나면
        {
            //Debug.Log("Out");
            GameManager.Instance.Target.Remove(this.gameObject); // 목록에서 제거
            isIn = false;
        }
    }
    // ==================================================== 이동 함수 ================================================= //
    public override void Move()
    {
        if (Vector3.Distance(GameManager.Instance._Player.transform.position, transform.position) <= 200.0f)  // 사거리 이내라면 추격 시작
        {
            rigidBody.velocity = transform.forward * speed;
            direction = Quaternion.LookRotation(GameManager.Instance._Player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * 3f);
        }
        else
        {
            rigidBody.velocity = transform.forward * speed;
            if(movementTimer > 2f)
            {
                switch (Random.Range(0, 4))
                {
                    case (int)State.forward:
                        {
                            direction = Quaternion.LookRotation(transform.forward * 100f - transform.position);
                            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * 1f);
                            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
                            break;
                        }
                    case (int)State.left:
                        {
                            direction = Quaternion.LookRotation(-transform.right * 100f - transform.position);
                            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * 1f);
                            //transform.Translate((Vector3.forward + Vector3.left) * speed * Time.deltaTime);
                            break;
                        }
                    case (int)State.right:
                        {
                            direction = Quaternion.LookRotation(transform.right * 100f - transform.position);
                            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * 1f);
                            //transform.Translate((Vector3.forward + Vector3.right) * speed * Time.deltaTime);
                            break;
                        }
                    case (int)State.attack:
                        {
                            direction = Quaternion.LookRotation(transform.forward * 100f - transform.position);
                            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * 1f);
                            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
                            break;
                        }
                        
                }
                movementTimer = 0f;
            }
           
        }
        /*
        if (movementTimer > 3f)
        {
            switch (Random.Range(0, 4))
            {
                case (int)State.forward:
                    {
                        direction = Quaternion.LookRotation(GameManager.Instance._Player.transform.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * 1f);
                        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
                        break;
                    }
                case (int)State.left:
                    {
                        transform.Translate((Vector3.forward + Vector3.left) * speed * Time.deltaTime);
                        break;
                    }
                case (int)State.right:
                    {
                        transform.Translate((Vector3.forward + Vector3.right) * speed * Time.deltaTime);
                        break;
                    }
                case (int)State.attack:
                    {
                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                        break;
                    }
            }
            movementTimer = 0f;
        }
        */
    }
    // ==================================================== 적 파괴 함수 ================================================= //
    public override void DestroyEnemy()
    {
        //GameManager.Instance._EnemyPool.ReturnEnemy(this);
        GameManager.Instance.Target.Remove(this.gameObject); // 목록에서 제거
        GameManager.Instance._Player.ResetTargetIndex(); // 플레이어의 타겟 인덱스 0으로 초기화
        Instantiate(GameManager.Instance._Effect.GetExplosion(), transform.position, transform.rotation);
        GameManager.Instance.SetScore(100);
        Destroy(this.gameObject);
        //rigidBody.useGravity = true;
        //rigidBody.drag = 0;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet") // 총알의 경우
        {
            DecreaseHP(); // 체력 감소
        }
        else if (other.gameObject.tag == "Missile") // 유도 미사일의 경우
        {
            DestroyEnemy(); // 즉사
        }
        else if(other.gameObject.tag == "Player") // 플레이어 몸체와 충돌 시
        {
            DestroyEnemy(); // 즉사
        }
    }
    public void DecreaseHP() // 체력 감소
    {
        HP -= 5f;
        Debug.Log(HP);
    }
    private void Dead() // 사망 함수
    {
        if(HP <= 0)
        {
            DestroyEnemy();
        }
    }
}
