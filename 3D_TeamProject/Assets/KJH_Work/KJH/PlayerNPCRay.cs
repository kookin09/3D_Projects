using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using TMPro;
using UnityEngine;

public class PlayerNPCRay : MonoBehaviour
{
    private float checkRate = 0.05f;
    private float lastCheckTime;
    public float NPCrayDistance;
    public LayerMask NPCLayer;

    private Camera cam;
    public TextMeshProUGUI NPCText;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, NPCrayDistance, NPCLayer)) // NPC 감지
        {
            NPCDialogue npc = hit.collider.GetComponent<NPCDialogue>();
            if (npc != null)
            {
                NPCText.text = npc.npcDialogue;
                NPCText.gameObject.SetActive(true);
                //Debug.Log("NPC 감지됨: " + hit.collider.name);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    int layer = hit.collider.gameObject.layer;

                    switch (layer)
                    {
                        case int npc1 when npc1 == LayerMask.NameToLayer("NPC1"):
                            Debug.Log("NPC1 레이어에 닿음");
                            hit.collider.GetComponent<IInteractableNPC>()?.InteractNPC1();
                            break;

                        case int npc2 when npc2 == LayerMask.NameToLayer("NPC2"):
                            Debug.Log("NPC2 레이어에 닿음");
                            hit.collider.GetComponent<IInteractableNPC>()?.InteractNPC2();
                            break;

                        case int npc3 when npc3 == LayerMask.NameToLayer("NPC3"):
                            Debug.Log("NPC3 레이어에 닿음");
                            hit.collider.GetComponent<IInteractableNPC>()?.InteractNPC3();
                            break;

                        default:
                            Debug.LogWarning("정의되지 않은 NPC 레이어 감지");
                            break;
                    }
                }
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * NPCrayDistance, Color.red);
    }
}
