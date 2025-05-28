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

        //인벤토리 생성
        for (int i = 0; i < CurBackPackStack; i++)
        {
            AssignPlace.Add(new leejuItemSlot(null, 0));
        }

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

            int usedSlots = 0;

            for (int i = 0; i < AssignPlace.Count; i++)
            {
                leejuItemSlot slot = AssignPlace[i];

                if (slot.itemInfo != null)
                {
                    usedSlots++;
                    Debug.Log($"[슬롯 {i}] 아이템: {slot.itemInfo.ItemName}, 개수: {slot.CurItemStack}");
                }
            }

            int remainingSlots = CurBackPackStack - usedSlots;

            Debug.Log($"남은 잔여 인벤토리 칸 수: {remainingSlots}");

            nextDebugTime = Time.time + 3f;
            //Debug.Log("현재 가방칸 : " + CurBackPackStack);
            //nextDebugTime = Time.time + 3f;
        }
    }

    //
    public bool AddCanStackItem(leejuItemSO itemSO, int getItemStack)
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
        //canstack이 체크 해제되어있을 때 !itemSO.canStack
        else 
        {
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
        return false;
    }

}