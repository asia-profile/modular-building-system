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
    float height;
    public float scale = 0.2f;


    [SerializeField] private Toggle gridToggle;

    private void Start()
    {
        Vector3 boxHeight = pendingObject.GetComponent<MeshRenderer>().bounds.size;
        height = boxHeight.y;
    }

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
            if (Input.GetKeyDown(KeyCode.E)) //scale up
            {
                ScaleUp();
            }
            if (Input.GetKeyDown(KeyCode.Q)) //scale down
            {
                //transform.localScale = transform.localScale + new Vector3(0, -scale, 0);
                ScaleDown();
            }
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                ScaleUpX();
            }
            if (Input.GetKeyDown(KeyCode.S)) 
            {
                ScaleDownX();
            }
            if (Input.GetKeyDown(KeyCode.A)) 
            {
                ScaleUpZ();
            }
            if (Input.GetKeyDown(KeyCode.D)) 
            {
                ScaleDownZ();
            }
        }
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        if (Physics.Raycast(ray, out hit, 1000, layerMask)) 
        {
            position = hit.point;
            //Vector3 boxHeight = pendingObject.GetComponent<MeshRenderer>().bounds.size;
            //float height = boxHeight.y;
            position = hit.point + (0.5f * height * Vector3.up);
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

    public void ScaleUp() 
    {
        pendingObject.transform.localScale = pendingObject.transform.localScale + new Vector3(0, scale, 0);
    }

    public void ScaleDown()
    {
        pendingObject.transform.localScale = pendingObject.transform.localScale + new Vector3(0, -scale, 0);
    }

    public void ScaleUpX()
    {
        pendingObject.transform.localScale = pendingObject.transform.localScale + new Vector3(scale, 0, 0);
    }

    public void ScaleDownX()
    {
        pendingObject.transform.localScale = pendingObject.transform.localScale + new Vector3(-scale, 0, 0);
    }

    public void ScaleUpZ()
    {
        pendingObject.transform.localScale = pendingObject.transform.localScale + new Vector3(0, 0, scale);
    }
    public void ScaleDownZ()
    {
        pendingObject.transform.localScale = pendingObject.transform.localScale + new Vector3(0, 0, -scale);
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
