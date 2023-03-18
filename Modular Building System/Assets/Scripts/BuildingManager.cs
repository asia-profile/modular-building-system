using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject pendingObject;
    [SerializeField] private Material[] materials;

    private Vector3 position; //position where we put object
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    public float rotateAmount;

    public float gridSize;
    
    bool gridOn = true;
    public bool canPlace = true;
    

    [SerializeField] private Toggle gridToggle;

    // Update is called once per frame
    void Update()
    {
        //check object
        if (pendingObject != null) 
        {
            UpdateMaterials();
            pendingObject.transform.position = position;

            if (gridOn) 
            {
                pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(position.x),
                    RoundToNearestGrid(position.y),
                    RoundToNearestGrid(position.z)
                    );
            } else { pendingObject.transform.position = position; }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
            //UpdateMaterials();
        }
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //where to put the object
        if (Physics.Raycast(ray, out hit, 1000, layerMask)) //how far ray goes
        {
            position = hit.point; //get raycast hit and impact point - where to put ut
        }
    }

   void UpdateMaterials()
    {
        if(canPlace)
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[0];
        }
        if(!canPlace)
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[1];
        }
    }

    //need to select objects in game
    public void SelectObject(int index) 
    {
        pendingObject = Instantiate(objects[index], position, transform.rotation);
    }

    public void PlaceObject() 
    {
        pendingObject.GetComponent<MeshRenderer>().material = materials[2];
        pendingObject = null;
    }

   public void RotateObject()
    {
        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }

    public void ToggleGrid()
    {
        if (gridToggle.isOn)
        {
            gridOn = true;
        }
        else 
        {
            gridOn = false;
        }
    }

    float RoundToNearestGrid(float position) 
    {
        //changeable grid system
        float xDiff = position % gridSize;
        position -= xDiff;
        if (xDiff> (gridSize)/2)
        {
            position += gridSize;
        }
        return position;
    }
}
