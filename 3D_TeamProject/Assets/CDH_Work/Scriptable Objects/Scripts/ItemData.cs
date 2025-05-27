using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType        //      아이템 종류
{
    Consumable      //      까까
}

public enum ConsumableType      //      먹을 것 종류
{
    Hunger,     //      배고픔
    Drink,      //      마실것
    Health      //      체력
}

[System.Serializable]       //      밑의 "ItemDataConsumable" 을 Inspector에 시각화 밑 Unity 저장
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]     //      ScriptableObject를 쉽게 생성할 수 있도록 도와주는 속성
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;      //      아이템 표기 이름
    public string description;      //      아이템 설명
    public ItemType type;           //      아이템 종류
    public Sprite icon;             //      아이템에 쓰일 스프라이트 아이콘

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}

