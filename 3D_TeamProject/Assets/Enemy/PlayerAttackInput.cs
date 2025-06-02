using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackInput : MonoBehaviour
{
    public AttackSword attackSword;
    public AttackBow attackBow;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;

        if (attackSword.gameObject.activeSelf)
        {
            attackSword.OnAttack();
        }
        else if (attackBow.gameObject.activeSelf)
        {
            attackBow.OnAttack();
        }
    }
}
