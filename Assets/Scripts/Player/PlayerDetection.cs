using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Action<Interactable> OnDetectInteractable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            OnDetectInteractable?.Invoke(other.gameObject.GetComponent<Interactable>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            OnDetectInteractable?.Invoke(null);
        }
    }
}
