using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Vector2 moveInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform cameraContainer;
    [SerializeField] private float sensitivity = 1.0f;
    private float xRotation = 0f;
    
    void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 camForward = cameraContainer.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraContainer.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        
        xRotation -= delta.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraContainer.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        transform.Rotate(Vector3.up * delta.x * sensitivity);
    }
}
