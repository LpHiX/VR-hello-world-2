using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OriginFactoryScript : MonoBehaviour
{
    public GameObject OriginFactoryPrefab = null;

    public List<XRRayInteractor> Interactors = null;

    private GameObject currentFactory = null;
    private GameObject heldObject = null;

    // Start is called before the first frame update
    void Start()
    {
        replaceFactory();
        foreach (XRRayInteractor interactor in Interactors)
        {
            interactor.selectEntered.AddListener(SelectMethods);
            interactor.selectExited.AddListener(ExitMethods);
        }

    }

    private void SelectMethods(SelectEnterEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.gameObject.Equals(currentFactory)) return;
        heldObject = currentFactory;
        replaceFactory();
    }

    private void replaceFactory()
    {
        currentFactory = Instantiate(OriginFactoryPrefab, transform.position, transform.rotation);
        currentFactory.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void ExitMethods(SelectExitEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.gameObject.Equals(heldObject)) return;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject = null;
    }
}
