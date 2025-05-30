using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour , IInteractableNPC
{
    public NPC1Dialogue dialogue1;
    public void InteractNPC1()
    {
        Debug.Log(gameObject.name + " 와(과) 상호작용됨!");
        dialogue1.StartDialogue1();//대사 출력 함수
    }

    public void InteractNPC2()
    {
        Debug.Log(gameObject.name + " 와(과) 상호작용됨!");
        dialogue1.StartDialogue1();
    }

    public void InteractNPC3()
    {
        Debug.Log(gameObject.name + " 와(과) 상호작용됨!");
        dialogue1.StartDialogue1();
    }
}
