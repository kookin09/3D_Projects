using System;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition thirsty { get { return uiCondition.thirsty; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;
    public float noThirstyHealthDecay;
    public event Action onTakeDamage;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);      //      매초 마다 배고픔 차감
        thirsty.Subtract(thirsty.passiveValue * Time.deltaTime);    //      매초 마다 목마름 차감
        stamina.Add(stamina.passiveValue * Time.deltaTime);         //      매초 마다 스태미나 회복

        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);  //      배고픔이 0보다 낮을 때 체력 차감
        }

        if (thirsty.curValue <= 0f)
        {
            health.Subtract(noThirstyHealthDecay * Time.deltaTime); //      목마름이 0보다 낮을 때 체력 차감
        }

        if (health.curValue < 0f)       //      체력이 0보다 낮을 때 사망 함수 호출
        {
            Die();
        }
    }

    public void Heal(float amount)      //      체력 회복
    {
        health.Add(amount);
    }

    public void Eat(float amount)       //      배고픔 회복
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)     //      목마름 회복
    {
        thirsty.Add(amount);
    }

    public void Die()                   //      사망
    {
        Debug.Log("GAME OVER");
    }

    public void TakePhysicalDamage(int damage)      //      피해입음
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)            //      스태미나 소모
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }
}