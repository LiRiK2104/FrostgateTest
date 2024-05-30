using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Services
{
    public class InputListener : MonoBehaviour
    {
        private InputActions _inputActions;
    
        internal event Action<Vector2> PointerDown;
        
        internal Vector2 PointerPosition { get; private set; }


        private void OnEnable()
        {
            InitializeInput();
        }

        private void OnDisable()
        {
            FinalizeInput();
        }


        private void ReadPointerPosition(InputAction.CallbackContext context)
        {
            PointerPosition = context.ReadValue<Vector2>();
        }
        
        private void InvokePointerDown(InputAction.CallbackContext context)
        {
            PointerDown?.Invoke(PointerPosition);
        }

        private void InitializeInput()
        {
            _inputActions ??= new InputActions();
            
            _inputActions.Player.PointerDown.performed += InvokePointerDown;
            _inputActions.Player.PointerPosition.performed += ReadPointerPosition;
            
            _inputActions.Enable();
        }
        
        private void FinalizeInput()
        {
            _inputActions.Player.PointerDown.performed -= InvokePointerDown;
            _inputActions.Player.PointerPosition.performed -= ReadPointerPosition;
            
            _inputActions.Disable();
        }
    }
}
