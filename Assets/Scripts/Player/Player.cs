using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ========================================= 정보 관련 ========================================= //

    private float HP; // =============== 내구도
    private bool isDead = false; // 사망 여부(폭발 이펙트 재생을 1번만 하기위해 필요함)

    // ========================================= 이동 관련 ========================================= //

    private Rigidbody rigidBody; // 리지드바디
    private Vector3 movement; // 이동 벡터
    private float speed; // 속도(이동을 위한)
    private Vector3 lastPosition; // 마지막 위치
    private float velocity; // 진짜 속도

    // ========================================= 미사일 관련 ========================================= //

    private bool missileMode = false; // 미사일 모드 on/off
    private float missileTimer = 5f; // 미사일 발사 간격 타이머
    private GameObject missile;     // 미사일 오브젝트
    private GameObject FirePoint; // 미사일 발사 지점
    private int targetIndex = 0;

    // ========================================= 머신건 관련 ========================================= //

    private float GunTimer = 0f; // 총 발사 간격 타이머
    private GameObject muzzleFlash; // 총 발사 이펙트 오브젝트
    private GameObject ShootPoint; // 머신건 발사 지점
    private const float rayDistance = 1500.0f; // 레이 사거리
    private RaycastHit hit; // 레이 맞은 곳

    // ========================================= 기타 ========================================= //

    private GameObject exhaustOutlet; // 배출구 오브젝트

    private void Awake()
    {
        HP = 100.0f;
        speed = 2000.0f;
        missile = Resources.Load("Bullet/Missile") as GameObject;
    }
    void Start()
    {
        lastPosition = transform.position;
        ShootPoint = GameObject.Find("ShootPoint").gameObject;
        FirePoint = GameObject.Find("FirePoint").gameObject;
        rigidBody = GetComponent<Rigidbody>();
        muzzleFlash = GameObject.FindGameObjectWithTag("Effect").gameObject;
        muzzleFlash.SetActive(false);
        exhaustOutlet = GameObject.FindGameObjectWithTag("Particle").gameObject;
        exhaustOutlet.SetActive(false);
        //GameManager.Instance._Input.KeyAction -= PlayerControl;
        //GameManager.Instance._Input.KeyAction += PlayerControl;
    }
    private void Update()
    {

        if (targetIndex > GameManager.Instance.Target.Count - 1)
        {
            targetIndex = GameManager.Instance.Target.Count - 1;
        }
        else if(targetIndex < 0)
        {
            targetIndex = 0;
        }
        //Debug.Log(targetIndex);
        LockTarget();

    }
    private void FixedUpdate()
    {
        CalculateVelocity();
        Dead();
        GunTimer += Time.deltaTime;
        missileTimer += Time.deltaTime;
    }
    // =========================================== 플레이어 이동 ============================================== //
    
    public void MoveRight() // 우측으로 회전
    {
        //Debug.Log("## Player Right");
        movement = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 10, 0), Time.deltaTime);
        transform.Rotate(movement);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, 45.0f, transform.rotation.z), Time.deltaTime);
    }
    public void MoveLeft() // 좌측으로 회전
    {
        //Debug.Log("## Player Left");
        movement = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -10, 0), Time.deltaTime);
        transform.Rotate(movement);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, -45.0f, transform.rotation.z), Time.deltaTime);
    }
    public void MoveFoward() // 전진
    {
        exhaustOutlet.SetActive(true);
        rigidBody.AddForce(transform.forward * speed * Time.deltaTime + transform.up, ForceMode.Acceleration);
        //transform.position += transform.forward * Time.deltaTime * speed;
    }
    public void TiltForward() // 앞으로 기울기
    {
        movement = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(20, 0, 0), Time.deltaTime);
        transform.Rotate(movement);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(180.0f, 0, 0), Time.deltaTime);
    }
    public void TiltBackward() // 뒤로 기울기
    {
        movement = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-20, 0, 0), Time.deltaTime);
        transform.Rotate(movement);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-180.0f, transform.rotation.y, transform.rotation.z), Time.deltaTime);
    }
    public void TiltLeft() // 왼쪽 기울기
    {
        movement = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 20), Time.deltaTime);
        transform.Rotate(movement);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180.0f), Time.deltaTime);
    }
    public void TiltRight() // 오른쪽 기울기
    {
        movement = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, -20), Time.deltaTime);
        transform.Rotate(movement);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, -180.0f), Time.deltaTime);
    }
    // ========================================== 사격 & 미사일 담당 ============================================ //
    public void Shoot()
    {
        // ============================ 사격 모드 =========================== //
        if (!missileMode)
        {

            // ============================ 사격 이펙트 활성화 & 총알 생성 =========================== //
            muzzleFlash.SetActive(true);
            if (GunTimer > 0.2f)
            {
                Bullet obj = GameManager.Instance._BulletPool.GetBullet();
                //obj.transform.position = transform.position + Vector3.forward * 5.0f + Vector3.left * 3.0f;
                obj.transform.position = ShootPoint.transform.position;
                obj.transform.rotation = transform.rotation;
                obj.transform.Rotate(Vector3.right * 90.0f); // x 축으로 +90 도 회전
                GunTimer = 0f;
            }
            // ============================ 레이캐스트 적 피격 판정 =========================== //
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.red, 0.1f);
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance))
            {
                if(hit.transform.tag == "Enemy")
                {
                    Debug.Log("## Enemy Damaged");
                    hit.transform.gameObject.GetComponent<EnemyNormal>().DecreaseHP();
                }
            }
        }// ============================ 미사일 모드 =========================== //
        else ShootMissile();
    }
    public void ShootMissile()
    {
        // ============================ 미사일 생성 & 미사일 타이머 초기화 =========================== //
        if (missileTimer > 3f)
        {
            GameObject obj = Instantiate(missile, FirePoint.transform.position, FirePoint.transform.rotation);
            obj.GetComponent<Missile>().SetTarget(targetIndex);
            missileTimer = 0f;
        }
    }
    // ============================ 사격 & 미사일 모드 변경 =========================== //
    public void ChangeAttackMode()
    {
        missileMode = !missileMode;
        GameManager.Instance._UI.TextAttackMode();
    }
    public void ShootKeyUp() // 사격 종료 시 muzzle 이펙트 비활성화
    {
        muzzleFlash.SetActive(false);
    }
    public void WKeyUp() // 전진 키 미 입력 시 배출구 이펙트 비활성화
    {
        exhaustOutlet.SetActive(false);
    }
    public bool GetAttackMode() // 현재 공격모드 가져오기
    {
        return missileMode;
    }
    public void LockTarget()
    {
        GameManager.Instance._UI.FocusTarget(targetIndex);
    }
    public void NextTargetIndex() // E 키
    {
        if (targetIndex < GameManager.Instance.Target.Count - 1)
        {
            targetIndex += 1;
            //Debug.Log("Index : " + targetIndex);
        }
        else targetIndex = 0;
    }
    public void BeforeTargetIndex() // Q 키
    {
        if (targetIndex > 0)
        {
            targetIndex -= 1;
            //Debug.Log("Index : " + targetIndex);
        }
        else targetIndex = GameManager.Instance.Target.Count - 1;
    }
    public void ResetTargetIndex()
    {
        targetIndex = 0;
    }
    private void CalculateVelocity()
    {
        velocity = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }
    public float GetVelocity()
    {
        return velocity;
    }
    public float GetHP()
    {
        return HP;
    }
    public void DecreaseHP(float damage) // 체력 감소
    {
        if (HP <= 0f) // 체력이 0 이하
        {
            HP = 0f; // 0 고정
        }
        else // 0 초과면
        {
            HP -= damage; // 데미지 만큼 체력 감소
        }
    }
    private void Dead() // 사망 함수
    {
        if(HP <= 0f && isDead == false)
        {
            Instantiate(GameManager.Instance._Effect.GetExplosion(), transform.position, transform.rotation);
            GameManager.Instance._UI.GameOver();
            Debug.Log("GameOver");
            isDead = true; // 무한 루프 방지
        }
    }
    private void OnCollisionEnter(Collision collision) // 충돌 시
    {
        if(collision.gameObject.tag == "Terrain") // 지형 충돌
        {
            Instantiate(GameManager.Instance._Effect.GetExplosion(), transform.position, transform.rotation);
            GameManager.Instance._UI.GameOver();
            Debug.Log("GameOver");
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Collision");
            DecreaseHP(10f);
            Instantiate(GameManager.Instance._Effect.GetExplosion(), transform.position, transform.rotation);
        }
    }
    /*
    private void FixRotation() // 기울기, 회전 최댓값 고정
    {

    }
    public void PlayerControl()
    {
        if(Input.GetKey(KeyCode.D))
        {
            rigidBody.AddForce(Vector3.right * speed * Time.deltaTime);
        }
    }*/
}
