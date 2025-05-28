using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRayleeju : MonoBehaviour
{
    public Camera cam; // 플레이어 카메라
    public float rayDistance = 5f; // 레이 거리
    public LayerMask itemLayerCheck; // 아이템 전용 레이어


    void Start()
    {
        
    }

    void Update()
    {
        
    }
    /*
    void CheckInterRay()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 정중앙
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);


    }
    */
}
