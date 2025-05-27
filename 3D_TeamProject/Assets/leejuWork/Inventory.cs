using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public SOBackPack BackPack;
    public float nextDebugTime = 0f;

    //현재 내 인벤토리는 몇칸짜리인지
    [SerializeField]
    private int CurBackPackStack;

    void Start()
    {
        //SO의 계산은 반드시 start나 Awake 부터 해야함 SO자체가 런타임부터 인스펙터에서 끌어오는거라 ㅇㅇ
        CurBackPackStack = BackPack.MaxStackCount;
        Debug.Log("현재 가방칸 : " + CurBackPackStack);
    }

    void Update()
    {
        if (Time.time >= nextDebugTime)
        {
            Debug.Log("현재 가방칸 : " + CurBackPackStack);
            nextDebugTime = Time.time + 3f;
        }
    }
}
