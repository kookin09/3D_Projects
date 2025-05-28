using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC1Dialogue : MonoBehaviour
{
    public TextMeshProUGUI NPCDialogue;
    public Image DialoguePannel;
    
    public TextMeshProUGUI dialogueText;
    public string[] dialogues; // 대사 목록을 저장할 배열
    private int dialogueIndex = 0; // 현재 출력할 대사의 인덱스
    private bool isTalking = false; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (!isTalking) return; 

            if (dialogueIndex < dialogues.Length) // 아직 출력할 대사가 남아 있다면
            {
                dialogueText.text = dialogues[dialogueIndex]; // 현재 인덱스 대사 출력
                dialogueIndex++; // 다음 대사로 인덱스 증가
            }
            else
            {
                dialogueText.text = ""; // 대사 다 끝나면 텍스트 비우기
                isTalking = false; 
                dialogueIndex = 0; // 인덱스 초기화
                NPCDialogue.gameObject.SetActive(false);
                DialoguePannel.gameObject.SetActive(false);
            }
        }
    }

    public void StartDialogue1()
    {
        isTalking = true; 
        NPCDialogue.gameObject.SetActive(true);
        DialoguePannel.gameObject.SetActive(true);
        dialogueIndex = 0; // 인덱스 처음부터
        dialogueText.text = dialogues[dialogueIndex]; // 첫 번째 대사 출력
        dialogueIndex++; // 다음 대사로 이동
    }
}
