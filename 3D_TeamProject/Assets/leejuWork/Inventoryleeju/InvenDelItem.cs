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

    public void ClickUseSlotItem(int slotIndex)
    {
        //현재 stack이 0보다 같거나 작아지면 아이템비우고
        if (inven.AssignPlace[slotIndex].CurItemStack <= 0)
        {
            //실제로직
            inven.AssignPlace[slotIndex].data = null;
            inven.AssignPlace[slotIndex].CurItemStack = 0;
            //ui에서 표시해줄로직
            inven.AssignUIPlace[slotIndex].SetUINullItemStack();
        }
        //현재 아이템stack이 (0보다 크면)호출 될때마다 stack을 1씩 줄인다.
        else
        {
            //실제 로직
            inven.AssignPlace[slotIndex].CurItemStack -= 1;
            //ui에서 받아올 로직
            inven.AssignUIPlace[slotIndex].SetUIItemStack(inven.AssignPlace[slotIndex].data, inven.AssignPlace[slotIndex].CurItemStack);
        }
    }
}
