using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// ========================================= 정보 관련 ========================================= //

	private float HP; // =============== 내구도
	private bool isDead = false; // 사망 여부(폭발 이펙트 재생을 1번만 하기위해 필요함)
	private AttackMode currentAttackMode = AttackMode.Gun;

	// ========================================= 이동 관련 ========================================= //

	private Rigidbody rigidBody; // 리지드바디
	private Vector3 movement; // 이동 벡터
	private float speed; // 속도(이동을 위한)
	private float velocity; // 진짜 속도

	// ========================================= 미사일 관련 ========================================= //

	private bool missileMode = false; // 미사일 모드 on/off
	private float missileTimer = 5f; // 미사일 발사 간격 타이머
	private GameObject missile;     // 미사일 오브젝트
	[SerializeField] private GameObject FirePoint; // 미사일 발사 지점
	private int targetIndex = 0;

	// ========================================= 머신건 관련 ========================================= //

	private float gunTimer = 0f; // 총 발사 간격 타이머
	[SerializeField] private GameObject muzzleFlash; // 총 발사 이펙트 오브젝트
	[SerializeField] private GameObject shootPoint; // 머신건 발사 지점
	private const float rayDistance = 1500.0f; // 레이 사거리
	private RaycastHit hit; // 레이 맞은 곳

	// ========================================= 기타 ========================================= //

	[System.Serializable]
	public class ExhaustOutLet
	{
		public GameObject left;
		public GameObject right;
		public void Toggle(bool isOn)
		{
			left.SetActive(isOn);
			right.SetActive(isOn);
		}
	}
	// 배출구 오브젝트
	[SerializeField] private ExhaustOutLet exhaustOutLet = new ExhaustOutLet();

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		HP = 100.0f;
		speed = 1950.0f;
	}
	private void Start()
	{
		GameManager.Instance.GetCurrentSceneObject<IngameSceneObject>().NotifyChangedAttackMode(currentAttackMode);
		muzzleFlash.SetActive(false);
		exhaustOutLet.Toggle(false);
	}
	private void Update()
	{
		if (Input.GetKey(KeyCode.W))
		{
			MoveFoward();
		}
		if (Input.GetKey(KeyCode.D))
		{
			MoveRight();
		}
		if (Input.GetKey(KeyCode.A))
		{
			MoveLeft();
		}
		if (Input.GetKey(KeyCode.Keypad8))
		{
			TiltForward();
		}
		if (Input.GetKey(KeyCode.Keypad5))
		{
			TiltBackward();
		}
		if (Input.GetKey(KeyCode.Keypad4))
		{
			TiltLeft();
		}
		if (Input.GetKey(KeyCode.Keypad6))
		{
			TiltRight();
		}
		if (Input.GetKey(KeyCode.Space))
		{
			Shoot();
		}
		if (Input.GetKeyUp(KeyCode.Space)) // 사격
		{
			ReleaseGunShoot();
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			exhaustOutLet.Toggle(false); // W 릴리즈 시에 추진기 비활성화
		}

		if (Input.GetKeyDown(KeyCode.R)) // 모드 변경
		{
			ChangeAttackMode();
		}

		else if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.GetGameOver()) // 게임 오버 시 재시작
		{
			ResetPlayer(); // 플레이어 초기화
			return;
		}
		else if (Input.GetKeyDown(KeyCode.E)) // 다음 타겟 변경
		{
			NextTargetIndex();
		}
		else if (Input.GetKeyDown(KeyCode.Q)) // 이전 타겟 변경
		{
			BeforeTargetIndex();
		}
		else if (Input.GetKeyDown(KeyCode.F)) // 자살
		{
			DecreaseHP(10f);
		}
		return;
		LockTarget(); // 타겟 고정
	}
	private void FixedUpdate()
	{
		CalculateVelocity();
		CalculateHeight();

		Dead();
		gunTimer += Time.deltaTime;
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
		exhaustOutLet.Toggle(true);
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
		switch (currentAttackMode)
		{
			case AttackMode.Gun:
				// ============================ 사격 이펙트 활성화 & 총알 생성 =========================== //
				muzzleFlash.SetActive(true);
				if (gunTimer > 0.2f)
				{
					GameObject obj = GameManager.Instance.GetCurrentSceneObject<IngameSceneObject>().GameObjectPool.GetObject(PoolObjectKey.Bullet);
					//obj.transform.position = transform.position + Vector3.forward * 5.0f + Vector3.left * 3.0f;
					obj.transform.position = shootPoint.transform.position;
					obj.transform.rotation = transform.rotation;
					obj.transform.Rotate(Vector3.right * 90.0f); // x 축으로 +90 도 회전
					if (obj.TryGetComponent(out Bullet b) == true)
					{
						b.Fly();
					}
					gunTimer = 0f;
				}
				// ============================ 레이캐스트 적 피격 판정 =========================== //
				Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.red, 0.1f);
				if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance))
				{
					if (hit.transform.tag == "Enemy")
					{
						Debug.Log("## Enemy Damaged");
						hit.transform.gameObject.GetComponent<EnemyNormal>().DecreaseHP();
					}
				}
				break;
			case AttackMode.Missile:
				ShootMissile();
				break;
		}
	}
	private void ShootMissile()
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
		int attackMode = (int)currentAttackMode + 1;
		if (attackMode > (int)AttackMode.Missile)
		{
			attackMode = 0;
		}
		currentAttackMode = (AttackMode)attackMode;

		GameManager.Instance.GetCurrentSceneObject<IngameSceneObject>().NotifyChangedAttackMode(currentAttackMode);
		//missileMode = !missileMode;
		//GameManager.Instance.UI.TextAttackMode();
	}
	public void ReleaseGunShoot() // 사격 종료 시 muzzle 이펙트 비활성화
	{
		muzzleFlash.SetActive(false);
	}
	public bool GetAttackMode() // 현재 공격모드 가져오기
	{
		return missileMode;
	}
	public void LockTarget() // 미사일 모드 시 적에게 타겟 고정
	{
		GameManager.Instance.UI.FocusTarget(targetIndex);
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
		velocity = rigidBody.velocity.magnitude * 3.6f;
		GameManager.Instance.GetCurrentSceneObject<IngameSceneObject>().NotifyChangedVelocity(velocity);
	}
	private void CalculateHeight()
	{
		float height = Mathf.Round(transform.position.y * 100) / 100;
		GameManager.Instance.GetCurrentSceneObject<IngameSceneObject>().NotifyChangedHeight(height);
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
		if (HP <= 0f && isDead == false)
		{
			Instantiate(GameManager.Instance._Effect.GetExplosion(), transform.position, transform.rotation);
			GameManager.Instance.UI.GameOver();
			Debug.Log("GameOver");
			isDead = true; // 무한 루프 방지
		}
	}
	private void OnCollisionEnter(Collision collision) // 충돌 시
	{
		if (collision.gameObject.CompareTag("Terrain")) // 지형 충돌
		{
			GameManager.Instance.GetCurrentSceneObject<IngameSceneObject>().CreateExplosionEffectAtPoint(transform);
			//GameManager.Instance.UI.GameOver();
			//Debug.Log("GameOver");
		}
		else if (collision.gameObject.CompareTag("Enemy")) // 적 충돌
		{
			//Debug.Log("Enemy Collision");
			DecreaseHP(10f);
			GameManager.Instance.GetCurrentSceneObject<IngameSceneObject>().CreateExplosionEffectAtPoint(transform);
		}
	}
	public void ResetPlayer() // 플레이어 리셋
	{
		rigidBody.isKinematic = true;
		GameManager.Instance.UI.GameRestart();
		transform.position = new Vector3(0, 600, 0);
		transform.rotation = Quaternion.Euler(0, 0, 0);
		HP = 100.0f;
		speed = 2000.0f;
		isDead = false;
		rigidBody.isKinematic = false;
	}
}
