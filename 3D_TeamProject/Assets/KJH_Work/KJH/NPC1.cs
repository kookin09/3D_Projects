using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1 : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log(gameObject.name + " 와(과) 상호작용됨!");
        // 예: 문 열기, 아이템 획득 등
        // gameObject.SetActive(false); 등도 가능
    }
}
