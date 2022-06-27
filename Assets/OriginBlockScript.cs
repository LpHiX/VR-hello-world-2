using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GridObject
{
    public GameObject GameObject;
    public XRSocketInteractor SocketInteractor;

    public GridObject(GameObject GameObject, XRSocketInteractor SocketInteractor)
    {
        this.GameObject = GameObject;
        this.SocketInteractor = SocketInteractor;
    }
}
public class OriginBlockScript : MonoBehaviour
{
    private Dictionary<Vector3Int, GameObject> containedBlocks = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, GridObject> containedGrids = new Dictionary<Vector3Int, GridObject>();

    public GameObject GridPrefab = null;
    public float GridSize = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        runAdjacentBlocks(addGrid, transform, false);
        containedBlocks.Add(new Vector3Int(0, 0, 0), gameObject);
    }
    private void gridSelected(SelectEnterEventArgs arg0)
    {
        arg0.interactableObject.transform.SetParent(transform);
        containedBlocks.Add(Vector3Int.RoundToInt( transform.InverseTransformPoint(arg0.interactorObject.transform.position)), arg0.interactableObject.transform.gameObject);
        runAdjacentBlocks(addGrid, arg0.interactorObject.transform, false);
    }
    private void gridExited(SelectExitEventArgs arg0)
    {
        arg0.interactableObject.transform.SetParent(null);
        containedBlocks.Remove(Vector3Int.RoundToInt( transform.InverseTransformPoint(arg0.interactorObject.transform.position)));
        runAdjacentBlocks(removeGrid, arg0.interactorObject.transform, false);
    }
    private bool addGrid(Vector3 position)
    {
        if (containedGrids.ContainsKey(Vector3Int.RoundToInt(position))) return false;
        if (containedBlocks.ContainsKey(Vector3Int.RoundToInt(position))) return false;
        GameObject gridGameObject = Instantiate(GridPrefab, transform.TransformPoint(position), transform.rotation, transform);
        GridObject gridObject = new GridObject(gridGameObject, gridGameObject.GetComponent<XRSocketInteractor>());
        gridObject.SocketInteractor.selectEntered.AddListener(gridSelected);
        gridObject.SocketInteractor.selectExited.AddListener(gridExited);
        containedGrids.Add(Vector3Int.RoundToInt(position), gridObject);
        return true;
    }

    public bool removeGrid(Vector3 position)
    {
        if (!containedGrids.ContainsKey(Vector3Int.RoundToInt(position))) return false;
        if (runAdjacentBlocks(gridNotUseless, containedGrids[Vector3Int.RoundToInt(position)].GameObject.transform, true))
        { 
            
            return false;
        }
        containedGrids[Vector3Int.RoundToInt(position)].SocketInteractor.selectExited.RemoveAllListeners();
        Destroy(containedGrids[Vector3Int.RoundToInt(position)].GameObject);
        containedGrids.Remove(Vector3Int.RoundToInt(position));
        return true;
    }

    private bool gridNotUseless(Vector3 position)
    {
        // Grid is not useless if it is connected to a block
        return containedBlocks.ContainsKey(Vector3Int.RoundToInt(position));
    }

    private bool runAdjacentBlocks(Func<Vector3, bool> method, Transform childTrans, bool performOnSelf)
    {
        bool outBool = false;
        if (method.Invoke(transform.InverseTransformPoint(childTrans.position) + GridSize * new Vector3(1, 0, 0))) outBool = true;
        if (method.Invoke(transform.InverseTransformPoint(childTrans.position) + GridSize * new Vector3(-1, 0, 0))) outBool = true;
        if (method.Invoke(transform.InverseTransformPoint(childTrans.position) + GridSize * new Vector3(0, 1, 0)))  outBool = true;
        if (method.Invoke(transform.InverseTransformPoint(childTrans.position) + GridSize * new Vector3(0, -1, 0))) outBool = true;
        if (method.Invoke(transform.InverseTransformPoint(childTrans.position) + GridSize * new Vector3(0, 0, 1)))  outBool = true;
        if (method.Invoke(transform.InverseTransformPoint(childTrans.position) + GridSize * new Vector3(0, 0, -1))) outBool = true;
        if (performOnSelf)
        {
            if (method.Invoke(transform.InverseTransformPoint(childTrans.position))) outBool = true;
        }

        return outBool;
    }
}
