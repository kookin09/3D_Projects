using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leejuItemSlot
{
    public ItemData data;
    public int CurItemStack;

    public leejuItemSlot(ItemData data, int stack)
    {
        this.data = data;
        this.CurItemStack = stack;
    }
}
