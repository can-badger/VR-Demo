using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor;

    [Header("Input")]
    [SerializeField] private InputActionReference selectAction;  // Trigger inputu

    [Header("Haptics")]
    [SerializeField] private float hapticIntensity = 0.5f;
    [SerializeField] private float hapticDuration = 0.1f;

    private void Update()
    {
        if (selectAction.action.WasPerformedThisFrame())
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponent<InteractionScript>();
            if (interactable != null)
            {
                interactable.OnSelect();
                SendHaptics();
            }
        }
    }

    private void SendHaptics()
    {
        controllerInteractor.SendHapticImpulse(hapticIntensity, hapticDuration);
    }
}