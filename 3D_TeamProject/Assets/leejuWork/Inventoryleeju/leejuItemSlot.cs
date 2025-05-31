using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leejuItemSlot
{
    public ItemData data;
    public int CurItemStack;
    public string CurItemName;
    public string CurItemDescription;

    public leejuItemSlot(ItemData data, int stack,string name,string Des)
    {
        this.data = data;
        this.CurItemStack = stack;
        this.CurItemName = name;
        this.CurItemDescription = Des;
    }
}
