using UnityEngine;


public class PlaneTrasponder : MonoBehaviour {
    //general info
    public string planeName;
    public string registrationNumber;
    public string aircraftType;
    public string destArrAirports;
    public float refreshRate = .5f;
    private float minSpeed = 140;
    private float maxSpeed = 250;
    private float minAltitude = 1500;
    private float maxAltitude = 10000;

    //navigatinal info
    public float expectedSpeedInKTS = 250;
    private float delayedSpeed;
    public float curSpeedInKTS;
    public float decelerationFactor = 0.1f; // kts per second 
    //[Range(1,360)] if you want slider
    public float expectedHeading = 0;
    private float delayedRotTDeltaTime;
    public float curHeading;
    public float headingSpeedDegSec = 3;
    public float expectedAltitude = 0;
    private float delayedAltitudeTDeltaTime;
    public float curAltitude;
    public float verticalSpeed = 25;

    float elapsedTime;

    [SerializeField]
    public bool b_overIt;
    public bool b_onLOC;
    public bool b_onGS;
    public bool b_clearedForILS;
    public string runwayToLand = "";

    public bool b_tooClose;

    public void Start()
    {
        b_overIt = false;
        curSpeedInKTS = expectedSpeedInKTS;
        b_onLOC = false;
        b_onGS = false;
        b_clearedForILS = false;

        expectedHeading = Mathf.Round(transform.rotation.eulerAngles.y);

      

    }

    private void FixedUpdate()
    { 
        //speed
        delayedSpeed += expectedSpeedInKTS / 180000;

        float decelerationAmount = Mathf.Abs(curSpeedInKTS - expectedSpeedInKTS);
        float decelerationTime = decelerationAmount / decelerationFactor;

        curSpeedInKTS = Mathf.Lerp(curSpeedInKTS, expectedSpeedInKTS, 1 / decelerationTime * Time.fixedDeltaTime);

      
            //heading
            Quaternion tarHeading = Quaternion.Euler(0, expectedHeading, 0);
            Quaternion curHeading = transform.rotation;
            float angle = Quaternion.Angle(tarHeading, curHeading);
            float angleSpeedTime = angle / headingSpeedDegSec;

            delayedRotTDeltaTime += Time.fixedDeltaTime;

            this.curHeading = transform.rotation.eulerAngles.y; // this because it conflicts with the local name quaternion curHeading


            //Altitude
            float altitudeDifference = Mathf.Abs(curAltitude - expectedAltitude);
            float altitudeSpeedTime = altitudeDifference / verticalSpeed;

            delayedAltitudeTDeltaTime += Time.fixedDeltaTime;


            // delayed calculations applied

            elapsedTime += Time.fixedDeltaTime;
      
            if (elapsedTime >= refreshRate)
            {
                elapsedTime = 0.0f;
                // Translate
                transform.Translate(transform.forward * delayedSpeed, Space.World);
                delayedSpeed = 0.0f;
                //Heading
                transform.rotation = Quaternion.Slerp(transform.rotation, tarHeading, 1 / angleSpeedTime * delayedRotTDeltaTime);
                delayedRotTDeltaTime = 0.0f;


                curAltitude = Mathf.Lerp(curAltitude, expectedAltitude, 1 / altitudeSpeedTime * delayedAltitudeTDeltaTime);
                delayedAltitudeTDeltaTime = 0.0f;

            }


        CheckPlaneLimitation();


    }

    public void OnMouseOver()
    {
        b_overIt = true;
    }

    public void OnMouseExit()
    {
        b_overIt = false;
    }

    private void CheckPlaneLimitation() 
    {
        //checks speed limits
        if (expectedSpeedInKTS > maxSpeed) { expectedSpeedInKTS = maxSpeed; }
        if (expectedSpeedInKTS < minSpeed) { expectedSpeedInKTS = minSpeed; }
        //check altitude
        if (expectedAltitude > maxAltitude) { expectedAltitude = maxAltitude; }
        if (expectedAltitude < minAltitude) { expectedAltitude = minAltitude; }
    }





}
