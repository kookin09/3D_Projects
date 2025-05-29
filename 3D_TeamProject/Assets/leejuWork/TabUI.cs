using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUI : MonoBehaviour
{
    public GameObject InvenUiCanvas;

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
        }
    }
}
