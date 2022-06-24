using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangingColorTest : MonoBehaviour
{
    public InputActionReference colorReference = null;
    private MeshRenderer meshRenderer = null;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        
    }

    private void Update()
    {
        if (!isActiveAndEnabled) return;
        float value = colorReference.action.ReadValue<float>();
        UpdateColor(value);
    }

    private void UpdateColor(float value)
    {
        meshRenderer.material.color = new Color(value, value, value);
    }
}
