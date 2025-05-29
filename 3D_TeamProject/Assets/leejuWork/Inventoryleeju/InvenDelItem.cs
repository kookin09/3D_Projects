using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenDelItem : MonoBehaviour
{
    public Inventory inven;
    
    public void ClickDelSlotItem(int slotIndex)
    {


        inven.AssignPlace[slotIndex].data = null;
        inven.AssignPlace[slotIndex].CurItemStack = 0;


        inven.AssignUIPlace[slotIndex].SetUINullItemStack();
    }
}
