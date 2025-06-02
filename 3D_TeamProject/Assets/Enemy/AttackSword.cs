using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSword : MonoBehaviour
{
    public Animator animator;
    public int damage = 20;
    public float attackCooldown = 1f;

    private float lastAttackTime;

    private void OnEnable()
    {
        lastAttackTime = 0f; // 무기 전환 시 초기화
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    // 애니메이션 이벤트로 호출
    public void OnHit()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * 1.5f, 1.5f);
        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent<IDamagable>(out var target))
            {
                target.TakePhysicalDamage(damage);
            }
        }
    }
}
