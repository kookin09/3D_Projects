using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leejuItemSlot
{
    public leejuItemSO itemInfo;
    public int CurItemStack;

    public leejuItemSlot(leejuItemSO itemInfo, int stack)
    {
        this.itemInfo = itemInfo;
        this.CurItemStack = stack;
    }
}
