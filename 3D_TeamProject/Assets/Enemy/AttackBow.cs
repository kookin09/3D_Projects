using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBow : MonoBehaviour
{
    public Animator animator;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 25f;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;

    private void OnEnable()
    {
        lastAttackTime = 0f; // 무기 전환 시 초기화
    }

    // 애니메이션 이벤트로 호출
    public void OnAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    // 애니메이션 이벤트에서 호출
    public void OnHit()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * arrowSpeed;
        }
    }
}
