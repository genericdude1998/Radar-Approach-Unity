using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    private PlaneSpawner spawner;
    private List<PlaneTrasponder> approachPlaneList;

    public float minSeparationNM;
    public float missedAppDistance = 5;

    public Material alarmMat;
    public Material defaultMat;
    public Text alertTitle;
    public Text alertInfo;
    public Text missedApp;


    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<PlaneSpawner>();  
    }

    // Update is called once per frame
    void Update()
    {
        approachPlaneList = spawner.approachPlaneList;

        if (approachPlaneList.Count >= 2)
        {
            foreach (PlaneTrasponder plane in approachPlaneList) // checks distances
            {
                foreach (PlaneTrasponder otherPlane in approachPlaneList)
                {
                    if (plane != otherPlane)
                    {
                        float dist = Vector3.Distance(plane.transform.position, otherPlane.transform.position);

                        if (dist <= minSeparationNM && !plane.b_tooClose && !otherPlane.b_tooClose)
                        {
                            Debug.LogWarning("Too Close"); 
                            DistanceAlarm(plane, otherPlane, dist);
                            plane.b_tooClose = true;
                            otherPlane.b_tooClose = true;
                        }

                        if(dist>minSeparationNM && plane.b_tooClose && otherPlane.b_tooClose)
                        {
                            ClearDistanceAlarm(plane, otherPlane);
                            plane.b_tooClose = false;
                            otherPlane.b_tooClose = false;

                        }

                    }
                }
            }

           foreach (PlaneTrasponder plane in approachPlaneList) 
           {
                float dist = Vector3.Distance(plane.transform.position, Vector3.zero);

                if (dist <= missedAppDistance && plane.curAltitude > 2500)
                {
                    missedApp.text = plane.name + " " + "Missed Approach" + " " + plane.runwayToLand; Debug.LogWarning("MissedApp!");
                    plane.GetComponent<MeshRenderer>().material = alarmMat;
                } // Expand with UI

            }

        }
        
    }

    private void DistanceAlarm(PlaneTrasponder plane, PlaneTrasponder otherPlane, float dist) 
    {
       MeshRenderer firstPlaneMeshRend = plane.gameObject.GetComponent<MeshRenderer>();
       MeshRenderer secondPlaneMeshRend = otherPlane.gameObject.GetComponent<MeshRenderer>();

        firstPlaneMeshRend.material = alarmMat;
        secondPlaneMeshRend.material = alarmMat;

        firstPlaneMeshRend.GetComponentInChildren<Text>().color = Color.red;
        secondPlaneMeshRend.GetComponentInChildren<Text>().color = Color.red;

        firstPlaneMeshRend.GetComponent<LineRenderer>().startColor = Color.red;
        firstPlaneMeshRend.GetComponent<LineRenderer>().endColor = Color.red;

        alertTitle.text = "Alert System";
        alertInfo.text = plane.name + " " + otherPlane.name + "\r\n" + "Traffic Alert less than" + " " + Mathf.Round(dist).ToString() + " " + "miles";

    }

    private void ClearDistanceAlarm(PlaneTrasponder plane, PlaneTrasponder otherPlane) 
    {
        MeshRenderer firstPlaneMeshRend = plane.gameObject.GetComponent<MeshRenderer>();
        MeshRenderer secondPlaneMeshRend = otherPlane.gameObject.GetComponent<MeshRenderer>();

        firstPlaneMeshRend.material = defaultMat;
        secondPlaneMeshRend.material = defaultMat;

        firstPlaneMeshRend.GetComponentInChildren<Text>().color = Color.white;
        secondPlaneMeshRend.GetComponentInChildren<Text>().color = Color.white;

        firstPlaneMeshRend.GetComponent<LineRenderer>().startColor = Color.white;
        firstPlaneMeshRend.GetComponent<LineRenderer>().endColor = Color.white;

        
        alertInfo.text = " ";

    }
}
