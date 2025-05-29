using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUI : MonoBehaviour
{
    public GameObject InvenUiCanvas;
    public PlayerMove move;
    void Start()
    {
        InvenUiCanvas = GameObject.Find("UI Canvas");

        InvenUiCanvas.SetActive(false); //  시작 시 꺼놓기
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool setUI = !InvenUiCanvas.activeSelf;
            InvenUiCanvas.SetActive(setUI);
            Debug.Log("인벤켜짐: " + setUI);
            //move.ToggleCursor();
            ToggleCursor(setUI);

        }
    }

    public void ToggleCursor(bool toggle)       //      인벤토리 열었을 때 시점 회전 X
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        move.canLook = !toggle;
    }
}
