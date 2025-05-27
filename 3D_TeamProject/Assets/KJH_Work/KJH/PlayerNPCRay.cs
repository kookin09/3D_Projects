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
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
        
            if (Physics.Raycast(ray, out hit, NPCrayDistance, NPCLayer)) // NPC 감지 RAY
            {
                NPCDialogue npc = hit.collider.GetComponent<NPCDialogue>();
                if (npc != null)
                {
                    NPCText.text = npc.npcDialogue;
                    NPCText.gameObject.SetActive(true);
                    Debug.Log("NPC 감지됨: " + hit.collider.name);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                       // var interactable = hit.collider.GetComponent<IInteractable>();
                       // if (interactable != null)
                        {
                        //    interactable.Interact();
                        }
                    }
                }
            }
            else
            {
                NPCText.gameObject.SetActive(false);
            }
            Debug.DrawRay(ray.origin, ray.direction * NPCrayDistance, Color.red);
        }
        
        
    }
}
