using UnityEngine;
using UnityEngine.UI;

public class WeatherRadarManager : MonoBehaviour
{
   public Material weatherRadarClear;
   public Material weatherRadarFoggy;
   public Material weatherRadarRainy;
   public Material weatherRadarOvercast;

    private WeatherRandomiser weatherRandom;
    private MeshRenderer meshRenderer;

    private float delayedTime;
    private float getTimeStaticRadar;
    float timePassedToNextRefresh;
    bool b_refreshed = false;
    public float refreshRate = 5;

    
    

    // Start is called before the first frame update
    void Start()
    {
        weatherRandom = FindObjectOfType<WeatherRandomiser>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (weatherRandom.curSkyCondit == "Clear") { meshRenderer.material = weatherRadarClear; }
        if (weatherRandom.curSkyCondit == "Foggy") { meshRenderer.material = weatherRadarFoggy; }
        if (weatherRandom.curSkyCondit == "Overcast") { meshRenderer.material = weatherRadarOvercast; }
        if (weatherRandom.curSkyCondit == "Rainy") { meshRenderer.material = weatherRadarRainy; }

        float windSpeed = weatherRandom.windSpeed /2500;
        delayedTime += Time.fixedDeltaTime;
        float angle = 360 + 180 - weatherRandom.windDirFromHeading + 90; // offset heading

        Vector2 windSpeedVector = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

     

        if (Mathf.Ceil(delayedTime) % refreshRate == 0 && !b_refreshed)
        {
            meshRenderer.material.SetVector("WindSpeed", windSpeedVector * windSpeed * delayedTime);
            getTimeStaticRadar = delayedTime;
            b_refreshed = true;
            
        }

        else if(b_refreshed)
        { 
            meshRenderer.material.SetVector("WindSpeed", windSpeedVector * windSpeed * getTimeStaticRadar);
            timePassedToNextRefresh += Time.fixedDeltaTime;
            if (timePassedToNextRefresh >= refreshRate)
            {
                b_refreshed = false;
                timePassedToNextRefresh = 0.0f;
            }

        }
    }
}
