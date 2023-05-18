using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerInput : MonoBehaviour
{
    private float inputX;
    public void SetInputMove(InputAction.CallbackContext value)
    {
        inputX = value.ReadValue<Vector2>().x;
    }

    public float GetInputX()
    {
        return inputX;
    }
}
