using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        
        Physics.Raycast(origin, direction * 3f);//NPC 감지 ray
        Debug.DrawRay(origin, direction * 3f, Color.red);
        
    }
}
