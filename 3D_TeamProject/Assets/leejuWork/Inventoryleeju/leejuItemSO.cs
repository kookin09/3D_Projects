using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "ScriptableObject/ItemInfo", order = 1)]
public class leejuItemSO : ScriptableObject
{
    //아이템 이름
    public string ItemName;

    //아이템 아이콘
    public Sprite ItemIcon;

    //섭취가 가능한지
    public bool canEdible;

    //중복스택가능한지
    public bool canStack;





}
