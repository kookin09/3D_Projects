using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public SOBackPack BackPack;
    public float nextDebugTime = 3f;

    //현재 내 인벤토리는 몇칸짜리인지
    [SerializeField]
    private int CurBackPackStack;

    //실제 인벤토리에 아이템 정보를 저장할 공간을 할당;
    public List<leejuItemSlot> AssignPlace = new List<leejuItemSlot>();

    void Start()
    {

        //SO의 계산은 반드시 start나 Awake 부터 해야함 SO자체가 런타임부터 인스펙터에서 끌어오는거라 ㅇㅇ
        CurBackPackStack = BackPack.MaxStackCount;

    }

    void Update()
    {
        DebugInventoryStackCount();
    }

    //   ↓↓↓여기서 부턴 사용자 정의 함수들↓↓↓

    //현재 내 가방칸수 로그찍기
    void DebugInventoryStackCount()
    {
        if (Time.time >= nextDebugTime)
        {
            Debug.Log("현재 가방칸 : " + CurBackPackStack);
            nextDebugTime = Time.time + 3f;
        }
    }

    //
    bool AddCanStackItem(leejuItemSO itemSO, int getItemStack)
    {
        //지금 얻은 아이템이 스택형이라면
        if (itemSO.canStack)
        {
            //슬롯한번 스캔땡기고
            for (int i = 0; i < AssignPlace.Count; i++)
            {
                //같은종류 아이템있는게 트루면
                if (AssignPlace[i].itemInfo == itemSO)
                {
                    //기존의 아이템슬롯에 개수 더해준다
                    AssignPlace[i].CurItemStack += getItemStack;
                    return true;
                }
            }
            //슬롯 스캔 한번 땡기고
            for (int i = 0; i < AssignPlace.Count; i++)
            {
                //비어있는 슬롯 찾으면
                if (AssignPlace[i].itemInfo == null)
                {
                    //아이템 저장하고
                    AssignPlace[i].itemInfo = itemSO;
                    //개수 지정
                    AssignPlace[i].CurItemStack = getItemStack;
                    return true;
                }
            }
        }
        else
        {
            return false;
        }
        return false;
    }

}