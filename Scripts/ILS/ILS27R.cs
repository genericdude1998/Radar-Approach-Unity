
using UnityEngine;

public class ILS27R : MonoBehaviour
{
    public string runwayName = "27R";
    public PlaneTrasponder landingPlane;
    public GameObject ATCinterfaceGO;
    private AirplaneMovementInterface ATCInterface;
    private PlaneSpawner spawner;
    private bool b_withinAngleLimits;
    private float landingPlaneDist;
    private float localiserRadiusDist = 15f;
    private float glideSlopeRadiusDist;
    private float glideSlopeAltitude = 2500;
    private float localiserHeading = 270;
    private float ilsChangeHeadingSpeed = 8;
    private float localiserMaxAngleDiff = 10;// change later only for testing
    public GameObject interceptionPointGO;
    private Vector3 interceptPoint;
    private Vector3 landedPlanesPos;

    public float landedRadius = 1;


    // ADD THE ELEMINATION AFTER LANDING 
    void Start()
    {
        b_withinAngleLimits = false;
        interceptPoint = interceptionPointGO.transform.position;
        glideSlopeRadiusDist = Vector3.Distance(transform.position, interceptPoint);
        landingPlane = null;
        ATCInterface = ATCinterfaceGO.GetComponent<AirplaneMovementInterface>();

        landedPlanesPos = new Vector3(0, 0, 1);

        spawner = FindObjectOfType<PlaneSpawner>();
    }

    // Work on the detection of planeLanding // XXX
    void FixedUpdate()
    {
        if (landingPlane != null)
        {
            if (landingPlane.b_clearedForILS && landingPlane.runwayToLand == "27R")// add the runway name condition
            {

                landingPlaneDist = Vector3.Distance(transform.position, landingPlane.transform.position);

                Vector3 airplaneDir = landingPlane.transform.position - transform.position;
                float airplaneAngleDiff = Vector3.Angle(airplaneDir, transform.forward);

                if (airplaneAngleDiff < localiserMaxAngleDiff) { b_withinAngleLimits = true; } // 45 degree limit 
                if (landingPlaneDist <= localiserRadiusDist && b_withinAngleLimits)
                {
                    Localiser();
                    landingPlane.b_onLOC = true;
                }

                if (landingPlaneDist <= glideSlopeRadiusDist && landingPlane.curAltitude <= glideSlopeAltitude) { GlideSlope(glideSlopeRadiusDist, landingPlane.curAltitude); landingPlane.b_onGS = true; }

                if (landingPlaneDist <= landedRadius)
                {
                    Debug.Log("We landed!");
                    AddInLandedPlaneList();
                    RemoveLandedPlaneFromApproachPlaneList();
                    ResetLandedPlane();
                    RemoveLandingPlaneFromILS();
                }

            }
        }

    }

    private void Localiser()
    {
        Quaternion tarRot = Quaternion.LookRotation(interceptPoint - landingPlane.transform.position);
        landingPlane.expectedHeading = tarRot.eulerAngles.y;

        float distanceFromInterceptPoint = Vector3.Distance(landingPlane.transform.position, interceptPoint); // This is called after intercepting intercept point
        if (distanceFromInterceptPoint < .5 || landingPlane.b_onGS == true)
        {
            landingPlane.expectedHeading = localiserHeading;
        }
    }

    private void GlideSlope(float dist, float alt)
    {

        float curSpeed = landingPlane.curSpeedInKTS = 150;
        float time = dist / curSpeed;
        float descentRateInFPM = alt / time / 60;

        if (landingPlane.curAltitude > 10)
        {
            landingPlane.curAltitude -= descentRateInFPM / 3000; // per iteration check if accurate to runway threshold
            landingPlane.expectedAltitude = landingPlane.curAltitude;
        }

    }

    private void ResetLandedPlane()
    {
        landingPlane.transform.position = transform.position;
        landingPlane.curSpeedInKTS = 0;
        landingPlane.expectedSpeedInKTS = 0;
        landingPlane.curAltitude = 0;
        landingPlane.expectedAltitude = 0;
        landingPlane.gameObject.SetActive(false);
    }

    private void AddInLandedPlaneList()
    {
        ATCInterface.landedPlanes.Add(landingPlane);
    }

    private void RemoveLandedPlaneFromApproachPlaneList()  // try to remove from the approach Plane List!!!!!! w/o having reference exception
    {
        foreach (PlaneTrasponder plane in spawner.approachPlaneList)
        {
            if (plane == landingPlane)
            {
                spawner.approachPlaneList.Remove(plane);
                spawner.landedPlaneList.Add(plane);
                break;
            }
        }
    }

    private void RemoveLandingPlaneFromILS()
    {
        landingPlane = null;
    }
}
