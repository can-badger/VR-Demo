using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TeleportController : MonoBehaviour
{
    public XRNode leftHand = XRNode.LeftHand;
    public XRNode rightHand = XRNode.RightHand;

    private InputDevice leftDevice;
    private InputDevice rightDevice;

    private int currentIndex = 0;
    private bool canTeleport = true;
    public float teleportCooldown = 0.5f;

    void Start()
    {
        TryInitializeDevices();

        if (TeleportPointManager.Instance.teleportPoints.Count > 0)
        {
            transform.position = TeleportPointManager.Instance.teleportPoints[currentIndex].position;
        }
    }

    void Update()
    {
        if (!canTeleport) return;

        // Buton değerlerini oku
        bool aButton = false; // Sağ el - A
        bool xButton = false; // Sol el - X

        bool aSuccess = rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out aButton);
        bool xSuccess = leftDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out xButton);

        if (aSuccess && aButton)
        {
            GoToNextPoint();
        }
        else if (xSuccess && xButton)
        {
            GoToPreviousPoint();
        }
    }

    void TryInitializeDevices()
    {
        List<InputDevice> leftDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(leftHand, leftDevices);
        if (leftDevices.Count > 0)
        {
            leftDevice = leftDevices[0];
            Debug.Log($"[XR] Left device initialized: {leftDevice.name}");
        }

        List<InputDevice> rightDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(rightHand, rightDevices);
        if (rightDevices.Count > 0)
        {
            rightDevice = rightDevices[0];
            Debug.Log($"[XR] Right device initialized: {rightDevice.name}");
        }
    }

    void GoToNextPoint()
    {
        if (currentIndex < TeleportPointManager.Instance.teleportPoints.Count - 1)
        {
            currentIndex++;
            TeleportToCurrent();
        }
    }

    void GoToPreviousPoint()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            TeleportToCurrent();
        }
    }

    void TeleportToCurrent()
    {
        Transform targetPoint = TeleportPointManager.Instance.teleportPoints[currentIndex];
        Vector3 targetPos = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);
        StopAllCoroutines();
        StartCoroutine(SmoothTeleport(targetPos, 0.3f));
        StartCoroutine(Cooldown());
    }

    IEnumerator SmoothTeleport(Vector3 targetPos, float duration = 0.3f)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    IEnumerator Cooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }
}
