using System;
using System.Collections;
using System.Collections.Generic;
using Eggtato.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private PlayerEventSO playerEvent;

    private InputSystem_Actions inputSystem;

    public new void Awake()
    {
        base.Awake();

        inputSystem = new InputSystem_Actions();
        inputSystem.Player.Enable();
        inputSystem.Player.Attack.performed += MouseDown_Performed;
    }


    private void MouseDown_Performed(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // playerEvent.Event.OnMouseDown?.Invoke(hit.transform.gameObject, mousePosition);
        }
    }

    private void OnDestroy()
    {
        inputSystem.Dispose();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }
}