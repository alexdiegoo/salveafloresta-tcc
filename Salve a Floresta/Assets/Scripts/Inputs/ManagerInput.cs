using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerInput : MonoBehaviour
{
    private float inputX;

    public event Action OnButtonEvent;
    private bool isDown;

    public bool IsDown()
    {
        return isDown;
    }

    public void SetInputMove(InputAction.CallbackContext value)
    {
        inputX = value.ReadValue<Vector2>().x;
    }

    public float GetInputX()
    {
        return inputX;
    }

    public void OnButtonDown()
    {
        OnButtonEvent?.Invoke();
    }

    public void GetButtonDown(InputAction.CallbackContext value)
    {
        if(value.performed)
        {
            OnButtonDown();
        }

        isDown = value.performed;
    }
}
