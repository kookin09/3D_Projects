using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //레이의 시각화
    [Header("레이 길이")]
    [Tooltip("이거수정하면 레이길이달라짐")]
    public float RayLength = 0.6f;

    [Header("플레이어 이동 스탯")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        Move();
        IsGround();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    public void Onmove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }
    //움직임 +shift혹은 컨트롤? 이거 동시에눌러서웅크리기 하려면 modifier로 구현가능
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            Debug.Log("점프");
        }
    }
    bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {

            new Ray(transform.position+(transform.forward*0.2f)+(transform.up*0.01f),Vector3.down),
            new Ray(transform.position+(-transform.forward*0.2f)+(transform.up*0.01f),Vector3.down),
            new Ray(transform.position+(transform.right*0.2f)+(transform.up*0.01f),Vector3.down),
            new Ray(transform.position+(-transform.right*0.2f)+(transform.up*0.01f),Vector3.down)

        };

        for (int i = 0; i < rays.Length; i++)
        {
            //Debug.DrawRay사용법
            //DrawRay(설정해둔 레이배열.출발점 , 어디를향해쏠건지,보이는 색)

            //레이 배열의 출발점 : 배열.origin하면 됨 origin은 유니티 제공임
            //어디를 향해 쏠건지: 배열에 vector를 그대로 복붙하면됨
            //보이는 색 :  색도 커스텀 가능임

            Debug.DrawRay(rays[i].origin, Vector3.down * RayLength, Color.red);
            //레이 발사할 4개의 각출발점 4개 ,방향 아래로*레이의초기세팅길이,빨강 

            if (Physics.Raycast(rays[i], RayLength, groundLayerMask))
            {
                return true;
            }
        }
        return false;

    }

}
