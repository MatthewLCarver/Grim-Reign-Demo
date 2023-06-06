using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionController : MonoBehaviour
{
    Camera mainCamera;
    private InteractableType iType;
    private CharacterMover cm;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        cm = GetComponent<CharacterMover>();
    }

    /// <summary>
    /// Fire a Raycast from the camera to the centre of the screen and register a collision with any IInteractable
    /// objects, query their activation status and InteractOn or InteractOff
    /// </summary>
    /// <param name="_context"></param>
    public void PerformAction(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            // raycast from the camera to the centre of the screen
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 4))
            {
                // if we hit something, check if it has an IInteractable component
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable == null) return;
                
                // if it does, call the Interact method
                if (!interactable.IsActivated)
                {
                    interactable.InteractOn(ref iType);
                    cm.SetInteractableTypeAnimation(iType);

                }
                else
                {
                    interactable.InteractOff();
                }
            }
        }
    }
}