using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour , IInteractableNPC
{
    public void InteractNPC1()
    {
        Debug.Log(gameObject.name + " 와(과) 상호작용됨!");
    }

    public void InteractNPC2()
    {
        Debug.Log(gameObject.name + " 와(과) 상호작용됨!");
    }

    public void InteractNPC3()
    {
        Debug.Log(gameObject.name + " 와(과) 상호작용됨!");
    }
}
