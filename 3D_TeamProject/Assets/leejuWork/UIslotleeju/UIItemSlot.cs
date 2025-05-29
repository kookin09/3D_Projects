using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    //슬롯에 표시할 이미지
    public Image icon;
    //슬롯에 표현할 stack
    public TextMeshProUGUI quantityText;

    
    public void SetUIItemStack(ItemData data,int stack)
    {
        icon.sprite = data.icon;
        quantityText.text = stack.ToString();
    }

    /// <summary>
    /// 아이템 Stack이 없을경우 아이콘 비우고 Stack도 비우고
    /// </summary>
    public void SetUINullItemStack()
    {
        icon.sprite = null;
        quantityText.text = "";
    }
}
