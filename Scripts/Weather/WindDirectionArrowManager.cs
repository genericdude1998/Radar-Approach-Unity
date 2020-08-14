using UnityEngine.UI;
using UnityEngine;

public class WindDirectionArrowManager : MonoBehaviour
{
    public WeatherRandomiser weatherReport;
    public GameObject arrow;
    RectTransform arrowtransform;

    public Text text;
    string activeRunway;

    public Vector3 arrowOffset;
    // Start is called before the first frame update
    void Start()
    {
        weatherReport = FindObjectOfType<WeatherRandomiser>();
        arrow = GameObject.Find("WindArrow");
        arrowtransform = arrow.GetComponent<RectTransform>();
       
    }

    private void Update()
    {
        float windDir = weatherReport.windDirFromHeading;
        Quaternion rotation = Quaternion.Euler(0, 0, 360 + 180 - windDir);
        arrowtransform.rotation = rotation;

        activeRunway = "";
        if (windDir < 180) { activeRunway = "West"; }
        else { activeRunway = "East"; }

        string s = Mathf.Round(weatherReport.windSpeed).ToString() + "KT" + " " + "@" + Mathf.Round(windDir) + "\r\n" + weatherReport.curSkyCondit + " " + weatherReport.humidity.ToString() + "%" + "\r\n" + "Active Runway"+ ": " + activeRunway;

        text.text = s;

        //float pixelRatio = 1920 / 1080;

       // arrowtransform.position = arrowOffset + new Vector3(Screen.width, Screen.height, 0) / pixelRatio;

    }
    
   


}
