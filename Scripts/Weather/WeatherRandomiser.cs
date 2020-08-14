
using UnityEngine;

public class WeatherRandomiser : MonoBehaviour
{
    public float weatherReportTime;
    public float windSpeed;
    public float windDirFromHeading;
    public float humidity;
    public string[] skyCondition;
    public string curSkyCondit;

    float elapsedTime;

    public PlaneSpawner planeSpawner;

    private void Start()
    {
        skyCondition = new string[4];
        skyCondition[0] = "Foggy";
        skyCondition[1] = "Clear";
        skyCondition[2] = "Rainy";
        skyCondition[3] = "Overcast";

        planeSpawner = FindObjectOfType<PlaneSpawner>();

        RandomiseWeather();
        SetActiveRunwaysBasedOnWindForecast(planeSpawner);

    }

    private void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;

        if (elapsedTime > 600) { RandomiseWeather(); elapsedTime = 0; }
    }

    private void RandomiseWeather() 
    {
        windSpeed = Mathf.Round(Random.Range(0, 30));
        windDirFromHeading = Mathf.Round(Random.Range(0, 359));

        int randomIndx = Random.Range(0, 4);

        curSkyCondit = skyCondition[randomIndx].ToString();

        humidity = Random.Range(10, 100);


    }
    private void SetActiveRunwaysBasedOnWindForecast(PlaneSpawner spawner)
    {
        
        if (windDirFromHeading <= 180) { spawner.activeRunwayComingFromThe = "East"; }
        else { spawner.activeRunwayComingFromThe= "West"; }
    }



}
