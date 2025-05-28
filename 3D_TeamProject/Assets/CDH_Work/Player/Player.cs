using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMove controller;
    public PlayerCondition condition;

    public ItemData itemData;
    public Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerMove>();
        condition = GetComponent<PlayerCondition>();
    }
}
