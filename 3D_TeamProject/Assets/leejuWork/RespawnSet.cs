using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnSet : MonoBehaviour
{
    //public bool isGameOver = false;
    public GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        CheckingGameOver();

    }

    void CheckingGameOver()
    {
        if (player.transform.position.y < -10f)
        {

            player.transform.position = new Vector3(0, 3, 0);

        }
    }
}
