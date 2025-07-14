using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InfoCardHover : MonoBehaviour
{
    public Transform player;
    public RectTransform cardPanel;
    public GameObject text,bg;
    public Vector2 collapsedSize = new Vector2(100f, 100f);
    public Vector2 expandedSize = new Vector2(500f, 400f);
    public float animationTime = 0.25f;

    private Coroutine resizeRoutine;
    private bool isHovering = false;
    private InputDevice leftDevice;

    void Start()
    {
        leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        cardPanel.sizeDelta = collapsedSize;

        var interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(_ => isHovering = true);
            interactable.hoverExited.AddListener(_ => isHovering = false);
        }
    }

    void Update()
    {
        if (!leftDevice.isValid)
            leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        bool trigger;
        leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out trigger);

        if (isHovering && trigger)
        {
            if (resizeRoutine == null)
                resizeRoutine = StartCoroutine(ResizePanel(expandedSize));
        }
        else if (!trigger)
        {
            if (resizeRoutine == null)
                resizeRoutine = StartCoroutine(ResizePanel(collapsedSize));
        }

        if (cardPanel.sizeDelta == expandedSize)
        {
            text.SetActive(true);
            bg.SetActive(true);
        }
        else
        {
            text.SetActive(false);
            bg.SetActive(false);
        }
        
        gameObject.transform.LookAt(player);
    }

    IEnumerator ResizePanel(Vector2 targetSize)
    {
        Vector2 startSize = cardPanel.sizeDelta;
        float time = 0f;

        while (time < animationTime)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, time / animationTime);
            cardPanel.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
            yield return null;
        }

        cardPanel.sizeDelta = targetSize;
        resizeRoutine = null;
    }
}