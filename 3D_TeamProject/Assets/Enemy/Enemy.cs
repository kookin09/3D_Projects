using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum AIState //적 상태
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

public enum EnemyType //적 타입
{
    Aggressive,
    Passive
}

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Type")]
    public EnemyType enemyType = EnemyType.Aggressive; //적의 타입 설정

    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance;
    private AIState aiState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    public float fieldOfView = 120f;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        SetState(AIState.Wandering); //시작시 Wandering으로 상태로 진입
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position); //플레이어와 오브젝트의 거리계산
        animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState) //현재상태에 따라 로직 실행
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                if (enemyType == EnemyType.Aggressive)
                    AttackingUpdate();
                break;
            case AIState.Fleeing:
                if (enemyType == EnemyType.Passive)
                    FleeingUpdate();
                break;
        }
    }

    public void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Fleeing:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed; //애니메이션 속도 조정
    }

    void PassiveUpdate() //기본 상태
    {
        // 도착하면 Idle 상태로 전환 후 일정 시간 대기
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        // 감지 범위 내 플레이어 감지 시 상태 전환
        if (enemyType == EnemyType.Aggressive && playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
        else if (enemyType == EnemyType.Passive && playerDistance < detectDistance)
        {
            SetState(AIState.Fleeing);
        }
    }

    void WanderToNewLocation() // 랜덤 위치로 이동
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation() // 유효한 NavMesh 위치 반환
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    void AttackingUpdate() //공격
    {
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate) //쿨타임후 다시공격
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.condition.GetComponent<IDamagable>().TakePhysicalDamage(damage);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            if (playerDistance < detectDistance) //감지 거리 내에서 추적
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    //경로 실패시 다시 방황상태
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else
            {
                //추적종료
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    void FleeingUpdate() //도망
    {
        //플레이어 반대방향으로 이동
        Vector3 directionAwayFromPlayer = (transform.position - CharacterManager.Instance.Player.transform.position).normalized;
        Vector3 fleeTarget = transform.position + directionAwayFromPlayer * maxWanderDistance;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(fleeTarget, path))
        {
            agent.SetDestination(fleeTarget);
        }

        if (playerDistance > detectDistance * 1.5f) //일정거리 멀어지면 다시 방황상태
        {
            SetState(AIState.Wandering);
        }
    }

    bool IsPlayerInFieldOfView() //시야각에 플레이어 확인
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    public void TakePhysicalDamage(int damage) //데미지 처리
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }

        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator DamageFlash() //피격시 색상변경
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }
}