
using System.Collections.Generic;
using UnityEngine;


public class PlaneUIIntercept : MonoBehaviour
{
    public List<Transform> planeTransformList;
    public float avoidanceRadius = 5;
    public float crowdedOffset = 10;
    public Vector3 dir;

    private void Start()
    {
       
    }
    void Update()
    {
        if (planeTransformList.Count > 2)
        {
            foreach (Transform plane in planeTransformList)
            {
                foreach (Transform otherPlanes in planeTransformList)
                {
                    if (plane != otherPlanes)
                    {
                        //CheckPlanesPos(plane, otherPlanes);
                    }
                }
            }
        }
     
    }

    private void CheckPlanesPos(Transform plane, Transform otherPlanes) 
    {
        float dist = Vector3.Distance(plane.position, otherPlanes.position);
        if (dist <= avoidanceRadius) { AvoidCrowdedPlaneUI(plane, otherPlanes); }
    }

    private void AvoidCrowdedPlaneUI(Transform plane, Transform otherPlanes) 
    {
       PlaneInfoDisplay planeRectManager = plane.GetComponent<PlaneInfoDisplay>();
       PlaneInfoDisplay otherPlanesRectManager = otherPlanes.GetComponent<PlaneInfoDisplay>();

        if (planeRectManager && otherPlanesRectManager != null) 
        { 
            Debug.Log("Need Avoidance!!!");
            planeRectManager.b_crowded = true;
            otherPlanesRectManager.b_crowded = true;

             dir = (otherPlanesRectManager.textPos.position - planeRectManager.textPos.position).normalized;

            //planeRectManager.avoidanceDir = dir;
            //otherPlanesRectManager.avoidanceDir = -dir;

            //planeRectManager.textPos.position += dir * crowdedOffset;
            // otherPlanesRectManager.textPos.position -= dir * crowdedOffset;

        }

    }

}
