
using System.Collections.Generic;
using UnityEngine;


public class PlaneSpawner : MonoBehaviour // add randomiser for names/ cargo/ type etc.
{
    public List<PlaneTrasponder> approachPlaneList;
    public List<PlaneTrasponder> landedPlaneList;
    public GameObject planePrefab;
    float timeElapsed;
    public int secondsToNextPlaneSpawn = 1;

    public string activeRunwayComingFromThe = "West";

    public GameObject[] vorGOArray;
    public GameObject BIGselector;
    public GameObject LAMselector;
    public GameObject OCKselector;
    public GameObject BNNselector;

    public float initialSpeedAtSpawn = 250;

    public float altitudeAtBIG = 7000;
    public float altitudeAtLAM = 7000;
    public float altitudeAtOCK = 7000;
    public float altitudeAtBNN = 7000;
    

    public GameObject BIGWpEast;
    public GameObject LAMWpEast;
    public GameObject OCKWpEast;
    public GameObject BNNWpEast;

    public GameObject BIGWpWest;
    public GameObject LAMWpWest;
    public GameObject OCKWpWest;
    public GameObject BNNWpWest;

    private bool b_nameAlreadyTaken;

    public int planeCount;
    public int planeCountLimit = 25; // you can dynamically change the limit of planes to be in airspace based on the name count
    private int leftCount;
    private int rightCount;


    // UI Intercept
    public PlaneUIIntercept UIIntercept;

    // Start is called before the first frame update
    private void Start()
    {
        if (planePrefab == null) { Debug.LogWarning("PlanePrefab not found!"); }
        timeElapsed = 0.0f;

        vorGOArray = new GameObject[4];
        vorGOArray[0] = BIGselector;
        vorGOArray[1] = LAMselector;
        vorGOArray[2] = OCKselector;
        vorGOArray[3] = BNNselector;

        approachPlaneList = new List<PlaneTrasponder>();
        landedPlaneList = new List<PlaneTrasponder>();
        InitialiseUIInterceptList();

        SpawnAPlane();

    }

    private void FixedUpdate()
    { // spawns a plane in seconds determined
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed >= secondsToNextPlaneSpawn && planeCount < planeCountLimit)
        {
            SpawnAPlane();
            Debug.Log("SpawnAPlane");
            timeElapsed = 0.0f;
        }
    }

    private void SpawnAPlane()
    {
        GameObject planeSpawned;
        Vector3 randomVORPos = RandomVORSpawnPos();

        planeSpawned = Instantiate(planePrefab, randomVORPos, SpawnRotation(randomVORPos)); // abit higher over the VOR and Waypoints

        PlaneTrasponder planeTrans = planeSpawned.GetComponent<PlaneTrasponder>();
        // Randomise Aircraft Type
      planeTrans.aircraftType = RandomiseAircraftType();

        // randomize positions and rotations based on the jetways and the airways used! // check if is not already in the list of approaching planes
        string randomname = RandomisePlaneName();

        foreach (PlaneTrasponder planes in approachPlaneList)  // check if name is already taken
        {
            if (planes.name == randomname) { b_nameAlreadyTaken = true; }
            else b_nameAlreadyTaken = false;
        }

        foreach (PlaneTrasponder landedPlane in landedPlaneList)
        {
            if (landedPlane.name == randomname) { b_nameAlreadyTaken = true; }
            else b_nameAlreadyTaken = false; 
        }

        if (!b_nameAlreadyTaken) 
        {
            planeSpawned.name = randomname;
            planeSpawned.GetComponent<PlaneTrasponder>().planeName = randomname;
            approachPlaneList.Add(planeSpawned.GetComponent<PlaneTrasponder>());
            planeCount++;
        }

        // sets preferred runway // also you can eliminate this // add this in different functions
        planeSpawned.GetComponent<PlaneTrasponder>().runwayToLand = SetPlaneLandingRunway();

        if (planeTrans.runwayToLand.Contains("R")) { rightCount++; }
        else if (planeTrans.runwayToLand.Contains("L")) { leftCount++; }

        //sets altitude based on VOR ALTITUTDE
        planeSpawned.GetComponent<PlaneTrasponder>().curAltitude = SetPlaneAltitudeAtSpawn(randomVORPos);
        planeSpawned.GetComponent<PlaneTrasponder>().expectedAltitude = SetPlaneAltitudeAtSpawn(randomVORPos);
        //Sets default speed 250kts under 10000'
        planeSpawned.GetComponent<PlaneTrasponder>().curSpeedInKTS = initialSpeedAtSpawn;
        planeSpawned.GetComponent<PlaneTrasponder>().expectedSpeedInKTS = initialSpeedAtSpawn;

        AddToUiInterceptList(planeSpawned.transform);

    }



    private Vector3 RandomVORSpawnPos()
    {
        int index = UnityEngine.Random.Range(0, 4); // RANDOM VOR INDEX
        Vector3 randomPos = vorGOArray[index].transform.position;
        return randomPos;
    }

    private Quaternion SpawnRotation(Vector3 randomVORPos)
    {
        if (activeRunwayComingFromThe == "West") // rotations names are inverted need to fix!!!
        {
            if (randomVORPos == BIGselector.transform.position) { return Quaternion.LookRotation(BIGWpEast.transform.position - randomVORPos); }
            if (randomVORPos == LAMselector.transform.position) { return Quaternion.LookRotation(LAMWpEast.transform.position - randomVORPos); }
            if (randomVORPos == OCKselector.transform.position) { return Quaternion.LookRotation(OCKWpEast.transform.position - randomVORPos); }
            if (randomVORPos == BNNselector.transform.position) { return Quaternion.LookRotation(BNNWpEast.transform.position - randomVORPos); }

        }

        if (activeRunwayComingFromThe == "East")
        {
            if (randomVORPos == BIGselector.transform.position) { return Quaternion.LookRotation(BIGWpWest.transform.position - randomVORPos); }
            if (randomVORPos == LAMselector.transform.position) { return Quaternion.LookRotation(LAMWpWest.transform.position - randomVORPos); }
            if (randomVORPos == OCKselector.transform.position) { return Quaternion.LookRotation(OCKWpWest.transform.position - randomVORPos); }
            if (randomVORPos == BNNselector.transform.position) { return Quaternion.LookRotation(BNNWpWest.transform.position - randomVORPos); }

        }
        return Quaternion.identity;
    }

    private string RandomisePlaneName() 
    {
        string[] alphabetcode = { "AZ", "CH", "KLM", "DHL", "AM" };
        string[] number = { "111", "123", "156", "765", "432" };
        string randomName = alphabetcode[UnityEngine.Random.Range(0, alphabetcode.Length)] + number[UnityEngine.Random.Range(0, number.Length)];
        return randomName;
    }

    private string RandomiseAircraftType() 
    {
        string[] aircraftTypeArr = { "A319", "A320", "A321", "A330", "A380", "B737","B747", "B787", "B777" };

        string randomAircraftType = aircraftTypeArr[Random.Range(0, aircraftTypeArr.Length)];
        return randomAircraftType;

    }

    private string SetPlaneLandingRunway() 
    { 
        if (activeRunwayComingFromThe == "West") 
        {
            if (rightCount >= leftCount)
            {
                string runwayname = "27" + "L";
                return runwayname;
            }

            else 
            { 
                string runwayname = "27" + "R";
                return runwayname;
            }
        }

        if (activeRunwayComingFromThe == "East")
        {
            if (rightCount >= leftCount)
            {
                string runwayname = "9" + "L";
                return runwayname;
            }

            else
            {
                string runwayname = "9" + "R";
                return runwayname;
            }

        }
        return "No Runway";
      
    }

    private float SetPlaneAltitudeAtSpawn(Vector3 randomVORPos)  
    {
        if (randomVORPos == BIGselector.transform.position) { return altitudeAtBIG; }
        if (randomVORPos == LAMselector.transform.position) { return altitudeAtLAM; }
        if (randomVORPos == OCKselector.transform.position) { return altitudeAtOCK; }
        if (randomVORPos == BNNselector.transform.position) { return altitudeAtBNN; }
        return 7000;
    }


    private void InitialiseUIInterceptList()
    {       
        UIIntercept.planeTransformList = new List<Transform>();
    }
    private void AddToUiInterceptList(Transform planeSpawnedTransform) 
    {
        UIIntercept.planeTransformList.Add(planeSpawnedTransform);
    }

  
}
