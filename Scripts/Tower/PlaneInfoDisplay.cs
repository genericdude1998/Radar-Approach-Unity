
using UnityEngine;
using UnityEngine.UI;


public class PlaneInfoDisplay : MonoBehaviour
{

    public float offsetMult = .2f;

    public RectTransform textRect;
    public Text text;
    public Transform textPos;
    public LineRenderer linerend;
    public PlaneTrasponder planeInfo;
  
    public float distanceLineText = 1;

    public bool b_crowded;

  //  public PlaneUIIntercept planeUIIntercept;

   // public Vector3 avoidanceDir;

  //  int randomInt;



    private void Start()
    {
        linerend = GetComponent<LineRenderer>();
        b_crowded = false;
      //  planeUIIntercept = FindObjectOfType<PlaneUIIntercept>();
      //  randomInt = Random.Range(-1, 1);
    }
    private void Update()
    {
        if (transform.position.z >= 1) { textPos.localPosition = -transform.worldToLocalMatrix.MultiplyPoint(transform.forward) * offsetMult; }
        else{ textPos.localPosition = -transform.worldToLocalMatrix.MultiplyPoint(transform.forward) * offsetMult; }

        if (transform.position.z < 2 && transform.position.z > 0)  // this is for 27R and 9L landings for smoother display of callsign info
        {
            offsetMult = 1.5f;
            textPos.position = transform.position + Vector3.forward * offsetMult;
        }
        else { offsetMult = .2f; }

        if (transform.position.z < 0 && transform.position.z > -1.5)  // this is for 27R and 9L landings for smoother display of callsign info
        {
            offsetMult = 1.5f;
            textPos.position = transform.position + -Vector3.forward * offsetMult;
        }
        else { offsetMult = .2f; }

      // if (b_crowded) { textPos.position += 10 * avoidanceDir; b_crowded = false; }

        Vector2 textScreenPos = Camera.main.WorldToScreenPoint(textPos.position);

        RectTransform rectTrans = text.GetComponent<RectTransform>();
        Vector2 offset = rectTrans.rect.center;
        rectTrans.SetPositionAndRotation(textScreenPos + offset, Quaternion.identity);

        Vector3 dir = transform.position - textPos.position;
        Vector3 lineOffset = dir * distanceLineText;
        Vector3[] arr = { transform.position, textPos.position + lineOffset }; // this does the line offset

        linerend.SetPositions(arr);

        DisplayInfo();
    }


    private void DisplayInfo() 
    {
        string oneDigitHeading = "00" + Mathf.Round(planeInfo.curHeading).ToString(); // makes a 001 010 100 visual possible
        string twoDigitHeading = "0" + Mathf.Round(planeInfo.curHeading).ToString();
        string threeDigitsHeading = Mathf.Round(planeInfo.curHeading).ToString();

         string stringToPrint = "";
        if (Mathf.Ceil(planeInfo.curHeading).ToString().Length == 1) { stringToPrint = oneDigitHeading; } // if one digit, two digits,three digits etc
        if (Mathf.Ceil(planeInfo.curHeading).ToString().Length == 2) { stringToPrint = twoDigitHeading; }
        if (Mathf.Ceil(planeInfo.curHeading).ToString().Length == 3) { stringToPrint = threeDigitsHeading; }

        text.text = planeInfo.planeName + " " + planeInfo.aircraftType +"\r\n" + Mathf.Round(planeInfo.curAltitude) + "FT" + " " + "-" +  stringToPrint + " " + Mathf.Round(planeInfo.curSpeedInKTS) + "KT" +"\r\n" + planeInfo.destArrAirports;

    }
}
