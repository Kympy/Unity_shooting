using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private float speed = 200f;
    private GameObject target;
    private Rigidbody rigidBody;
    private float timer = 0f;
    private Vector3 direction;
    private GameObject body; // 미사일 몸체
    private float distance;
    private void Awake()
    {
        body = transform.GetChild(0).gameObject;
        rigidBody = GetComponent<Rigidbody>();
        //Target = GameObject.Find("Cube").gameObject;
        
    }
    private void Start()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
        Invoke("DestroyMissile", 6f); // 5초 후 자동 파괴
    }
    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        // ============================ 1.5초 후 적 유도 시작 =========================== //
        if (timer > 0.5f)
        {
            if (target == null) // 타겟이 없으면
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            else
            {
                rigidBody.velocity = transform.forward * speed;
                Quaternion q = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2f);
                //transform.position = Vector3.LerpUnclamped(transform.position, target.transform.position, speed * Time.deltaTime / distance);
                //direction = (target.transform.position - transform.position).normalized;
                //transform.position += direction * speed * Time.deltaTime;
                //transform.forward = direction;
            }
        }
        else
        {
            transform.Translate(Vector3.forward * speed / 2 * Time.deltaTime);
        }
    }
    private void DestroyMissile()
    {
        GameObject obj = Instantiate(GameManager.Instance._Effect.GetExplosion(), transform.position, transform.rotation);
        body.SetActive(false);
        Destroy(this.gameObject, 2f);
    }
    public void SetTarget(int index)
    {
        if(index > GameManager.Instance.Target.Count - 1)
        {
            target = null;
        }
        else
        {
            target = GameManager.Instance.Target[index].gameObject;
        }
    }
    private void OnTriggerEnter(Collider other) // 충돌 시 파괴
    {
        if(other.gameObject.tag != "Player" && other.gameObject.tag != "Missile")
        {
            Instantiate(GameManager.Instance._Effect.GetExplosion(), transform.position, transform.rotation);
            DestroyMissile();
        }
    }
}
