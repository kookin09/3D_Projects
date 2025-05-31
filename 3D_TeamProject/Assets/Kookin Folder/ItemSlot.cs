using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    public ItemData data;

    public void SetUIItemNameAndDescription()
    {
        itemNameText.text = data.displayName;
        itemDescriptionText.text = data.description;

    }
}
