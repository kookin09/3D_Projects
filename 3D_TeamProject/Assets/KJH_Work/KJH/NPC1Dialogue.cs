using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; // DOTween 라이브러리 필요

public class NPC1Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI space;
    public Image dialoguePanel;
    public GameObject dialogueUI;
    

    public string[] dialogues; // 대사 목록을 저장할 배열
    private int dialogueIndex = 0;// 현재 출력할 대사의 인덱스
    private bool isTalking = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isTalking) return;

            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogues[dialogueIndex];
                isTyping = false;
            }
            else
            {
                dialogueIndex++;
                if (dialogueIndex < dialogues.Length)
                {
                    typingCoroutine = StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
                }
                else
                {
                    EndDialogue();
                    dialogueText.text = "";
                }
            }
        }
    }

    public void StartDialogue1()
    {
        dialogueIndex = 0;
        isTalking = true;

        // 먼저 오브젝트를 보이게 설정
        dialogueUI.SetActive(true);
        dialoguePanel.gameObject.SetActive(true);
        space.gameObject.SetActive(true);

        // 초기 상태를 작게 설정
        dialogueUI.transform.localScale = Vector3.zero;
        dialoguePanel.transform.localScale = Vector3.zero;

        // DOTween으로 팝업 애니메이션
        dialogueUI.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
        dialoguePanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);

        // 첫 대사 출력
        if (dialogues != null && dialogues.Length > 0)
        {
            typingCoroutine = StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
        }
    }

    IEnumerator TypeDialogue(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueIndex = 0;

        // 사라지는 애니메이션
        dialogueUI.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        dialoguePanel.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            dialogueUI.SetActive(false);
            dialoguePanel.gameObject.SetActive(false);
            space.gameObject.SetActive(false);
        });
    }
}