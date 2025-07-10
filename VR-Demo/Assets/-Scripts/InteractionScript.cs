using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    [SerializeField] private GameObject uiObjectInside; // Bu kitaplığın içindeki UI objesi

    private bool isVisible = false;

    public void OnSelect()
    {
        if (uiObjectInside != null)
        {
            isVisible = !isVisible;
            uiObjectInside.SetActive(isVisible);
        }
    }
}
