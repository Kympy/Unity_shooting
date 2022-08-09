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

    private GameObject exhaustOutlet_L; // 배출구 오브젝트
    private GameObject exhaustOutlet_R; // 배출구 오브젝트

    private void Awake()
    {
        Time.timeScale = 1f;
        //GameManager.Instance._Player = GetComponent<Player>();
        //GameManager.Instance.InitGameManager();
        GameManager.Instance._UI.GameRestart();
        HP = 100.0f;
        speed = 2000.0f;
        missile = Resources.Load("Bullet/Missile") as GameObject;
    }
    void Start()
    {
        lastPosition = transform.position; // 속도 계산을 위한 마지막 위치 저장
        ShootPoint = GameObject.Find("ShootPoint").gameObject;
        FirePoint = GameObject.Find("FirePoint").gameObject;
        rigidBody = GetComponent<Rigidbody>();
        muzzleFlash = GameObject.FindGameObjectWithTag("Effect").gameObject;
        muzzleFlash.SetActive(false);
        exhaustOutlet_L = GameObject.FindGameObjectWithTag("ParticleL").gameObject;
        exhaustOutlet_L.SetActive(false);
        exhaustOutlet_R = GameObject.FindGameObjectWithTag("ParticleR").gameObject;
        exhaustOutlet_R.SetActive(false);
        //GameManager.Instance._Input.KeyAction -= PlayerControl;
        //GameManager.Instance._Input.KeyAction += PlayerControl;
    }
    private void Update()
    {
        LockTarget(); // 타겟 고정
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
        exhaustOutlet_L.SetActive(true);
        exhaustOutlet_R.SetActive(true);
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
        if (missileTimer > 5.5f)
        {
            GameObject obj = Instantiate(missile, FirePoint.transform.position, FirePoint.transform.rotation);
            obj.GetComponent<Missile>().SetTarget(targetIndex);
            missileTimer = 0f;
        }
    }
    // ============================ 사격 & 미사일 모드 변경 =========================== //
    public void ChangeAttackMode() // 공격모드 변경
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
        exhaustOutlet_L.SetActive(false);
        exhaustOutlet_R.SetActive(false);
    }
    public bool GetAttackMode() // 현재 공격모드 가져오기
    {
        return missileMode;
    }
    public void LockTarget() // 미사일 모드 시 적에게 타겟 고정
    {
        GameManager.Instance._UI.FocusTarget(targetIndex);
    }
    public void NextTargetIndex() // E 키
    {
        if (targetIndex < GameManager.Instance.GetTargetList().Count - 1)
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
        else targetIndex = GameManager.Instance.GetTargetList().Count - 1;
    }
    public void ResetTargetIndex() // 타겟 인덱스 초기화
    {
        targetIndex = 0;
    }
    private void CalculateVelocity() // 속도계산
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
    public void ResetHP()
    {
        HP = 100f;
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
        else if(collision.gameObject.tag == "Enemy") // 적 충돌
        {
            Debug.Log("Enemy Collision");
            DecreaseHP(10f);
            Instantiate(GameManager.Instance._Effect.GetExplosion(), transform.position, transform.rotation);
        }
    }
    public void ResetPlayer() // 플레이어 리셋
    {
        rigidBody.isKinematic = true;
        GameManager.Instance._UI.GameRestart();
        transform.position = new Vector3(0, 600, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        HP = 100.0f;
        speed = 2000.0f;
        isDead = false;
        rigidBody.isKinematic = false;
    }
}
