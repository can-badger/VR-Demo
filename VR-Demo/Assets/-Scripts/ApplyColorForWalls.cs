using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ApplyColorForWalls : MonoBehaviour
{
    [Header("UI & Input")]
    public GameObject canvasObject;
    public InputActionReference gripPress;
    public InputActionReference navigateX;
    public InputActionReference triggerPress;

    [Header("Seçenekler")]
    public List<Button> options = new List<Button>();

    private int currentIndex = 0;
    private bool isGripHeld = false;
    private float navCooldown = 0.3f;
    private float navTimer = 0f;

    void OnEnable()
    {
        gripPress.action.Enable();
        navigateX.action.Enable();
        triggerPress.action.Enable();
    }

    void Update()
    {
        // Grip kontrolü
        isGripHeld = gripPress.action.ReadValue<float>() > 0.5f;
        canvasObject.SetActive(isGripHeld);

        if (!isGripHeld) return;

        // Analog navigasyon
        Vector2 joystick = navigateX.action.ReadValue<Vector2>();
        float x = joystick.x;
        navTimer -= Time.deltaTime;

        if (Mathf.Abs(x) > 0.5f && navTimer <= 0f)
        {
            if (x > 0.5f)
                currentIndex = Mathf.Min(currentIndex + 1, options.Count - 1);
            else if (x < -0.5f)
                currentIndex = Mathf.Max(currentIndex - 1, 0);

            UpdateHighlight();
            navTimer = navCooldown;
        }

        // Trigger ile seçim
        if (triggerPress.action.WasPressedThisFrame())
        {
            options[currentIndex].onClick.Invoke();
        }
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < options.Count; i++)
        {
            ColorBlock colors = options[i].colors;
            colors.normalColor = (i == currentIndex) ? Color.yellow : Color.white;
            options[i].colors = colors;

            if (i == currentIndex)
                options[i].Select(); // bazı UI sistemlerinde highlight için işe yarar
        }
    }
}