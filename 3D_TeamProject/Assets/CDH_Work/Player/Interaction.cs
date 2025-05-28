using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;     //      상호작용 물체 감지 주기 0.05초
    private float lastCheckTime;        //      마지막 물체 감지
    public float maxCheckDistance;      //      물체 감지 범위 한도
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red, checkRate);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()        //      상호작용 물체에 Ray가 닿을 시 Prompt true 및 호출
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractablePrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
