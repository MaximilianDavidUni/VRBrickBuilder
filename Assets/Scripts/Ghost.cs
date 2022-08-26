using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 *  Preview that will be displayed when placing a brick
 */
public class Ghost : MonoBehaviour
{

    [SerializeField] GridBuildingSystemVR grid; // Corresponding grid system
    [SerializeField] Material ghostMaterial;

    private Transform visual;   
    private PlacedObjectTypeSO placedObjectTypeSO;




    private void Start()
    {
        RefreshVisual();

        if(visual != null)
            visual.gameObject.SetActive(false);

        GridBuildingSystemVR.Instance.OnSelectedBrickChanged += Instance_OnSelectedChanged;
    }




    private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    {
        RefreshVisual();
    }




    private void LateUpdate()
    {
        try
        {
            if (grid.GetCurrentlyHeldPlacedObject() == null)
            {
                return;
            }
            if (!grid.GetCurrentlyHeldPlacedObject().isPickedUp())
            {
                return;
            }


            // Get the position and rotation to display the preview
            Vector3 targetPosition = GridBuildingSystemVR.Instance.GetSnapPoint(GridBuildingSystemVR.Instance.GetCurrentlyHeldPlacedObject());

            if (visual == null)
                RefreshVisual();

            transform.position = targetPosition;
            transform.rotation = GridBuildingSystemVR.Instance.GetPlacedObjectRotation();
        }
        catch (Exception e)
        {
            if (visual != null)
            {
                Destroy(visual.gameObject);
                visual = null;
            }
        }
    }


    
    /*
     *  Reloads the visual for the preview, based on the currently held brick type
     */
    private void RefreshVisual()
    {
        if(visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystemVR.Instance.GetCurrentGhosttPlacedObjectTypeSO();

        if(placedObjectTypeSO != null)
        {
            visual = Instantiate(placedObjectTypeSO.ghostVisual, Vector3.zero, Quaternion.identity);
            visual.localScale = grid.transform.localScale;
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            SetLayerRecursive(visual.gameObject, 0);
        }
    }




    /*
     *  Sets a given gameObject and all its children to the given layer
     */
    private void SetLayerRecursive(GameObject targetObject, int layer)
    {
        targetObject.layer = layer;
        foreach(Transform child in targetObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }




    /*
     *  Makes the preview visible
     */
    public void Activate()
    {
        if (visual == null)
            return;

        visual.gameObject.SetActive(true);
    }



    /*
     *  Make the preview invisible
     */
    public void Deactivate()
    {
        if (visual == null)
            return;

        visual.gameObject.SetActive(false);
    }



    /*
     *  Returns wether the preview is currently active
     */
    public bool IsActive()
    {
        if (visual == null)
            return false;

        return visual.gameObject.activeSelf;
    }
}