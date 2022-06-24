using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeFactoryScript : MonoBehaviour
{
    public List<XRRayInteractor> Interactors = null;
    public GameObject CubePrefab = null;

    public GameObject RedSlider = null;
    public GameObject GreenSlider = null;
    public GameObject BlueSlider = null;

    private GameObject currentCube;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        foreach (XRRayInteractor interactor in Interactors)
        {
            interactor.selectEntered.AddListener(SelectMethods);
        }
        replaceCube();

    }

    private void SelectMethods(SelectEnterEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.gameObject.Equals(currentCube)) return;
        
        currentCube.GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log(currentCube.GetComponent<Rigidbody>().isKinematic);
        replaceCube();
    }

    private void replaceCube()
    {
        currentCube = Instantiate(CubePrefab, transform.position, Quaternion.identity);
        meshRenderer = currentCube.GetComponent<MeshRenderer>();
        currentCube.GetComponent<Rigidbody>().isKinematic = true;
    }
    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.color = new Color(
            transform.InverseTransformPoint(RedSlider.transform.position).z * 4 + 0.5f,
            transform.InverseTransformPoint(GreenSlider.transform.position).z * 4 + 0.5f,
            transform.InverseTransformPoint(BlueSlider.transform.position).z * 4 + 0.5f);
    }
}
