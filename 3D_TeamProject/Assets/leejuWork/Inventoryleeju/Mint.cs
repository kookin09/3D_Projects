using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mint : MonoBehaviour
{

    public leejuItemSO itemData;

    void OnTriggerEnter(Collider collision)
    {
        //충돌한 애가 Player라면
        if (collision.CompareTag("Player"))
        {
            Debug.Log("아이템을 획득");

            //충돌한 애의 Inventory 클래스를 inventory라고 선언하고 여기에
            //충돌한 애의 Inventory라는 컴포넌트를 담는다
            Inventory inventory = collision.GetComponent<Inventory>();
            inventory.AddCanStackItem(itemData, 1);

            Destroy(gameObject);
        }
    }
}
