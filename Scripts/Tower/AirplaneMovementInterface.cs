
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class AirplaneMovementInterface : MonoBehaviour
{

    //Selection of plane
    private RaycastHit target;
    public GameObject targetPlaneGO;
    public List<GameObject> selectedPlanesList;
    private MeshRenderer tarPlaneIcon;
    private PlaneTrasponder tarPlaneTrasponder;
    public Canvas canvas;
    public List<PlaneTrasponder> landedPlanes;

    //Camera Zoom

    Camera cam;
    public float radarHorizontalRadius;


    //Info Panel
    public GameObject InfoPanelGO;
    public GameObject InfoPanelReadingsGO;
    public Text[] InfoPanelReadingsArr;

    //private Text InfoTitle; if you want to change the title
    private Text InfoCallsign;
    private Text InfoSpeed;
    private Text InfoHeading;
    private Text InfoAltitude;
    private Text RunwayToLand;

    //Control Panel
    public GameObject ControlPanelGO;
    public GameObject ControlPanelReadingsGO;
    public GameObject ControlPanelInputFieldsGO;
    public Text[] ControlPanelReadingsArr;
    public Text ILSGauge;

    //private Text ControlTitle; if you want to change the titl
    private Text ControlSpeed;
    private Text ControlHeading;
    private Text ControlAltitude;
    public InputField[] ControlInputFieldsArr;
    private InputField SpeedInputField;
    private InputField HeadingInputField;
    private InputField AltitudeInputField;
    public TransmitInstrcButton transmitInstructionButtonEventListener;

    //Cleared ILS
    public ClearedILSButtonEventListener clearedILSButtonEventListener;
    public ILS ils27L;
    public ILS ils27R;
    public ILS ils9L;
    public ILS ils9R;


    private bool b_openInfoPanel;
    private bool b_openControlPanel;


    public Material defaultPlaneMat;  //shaders plane default
    public Material selectedPlaneMat; // shaders plane selected

    //Weather Radar

    public MeshRenderer weatherRadar;
    private bool b_openWeatherRadar;
    public Text weatherRadarText;


    private void Start()
    {
        canvas.gameObject.SetActive(true);

        if (canvas != null)
        {

            if (InfoPanelGO != null && ControlPanelGO != null)
            {
                InfoPanelGO.SetActive(false);
                ControlPanelGO.SetActive(false);
                b_openInfoPanel = false;
                selectedPlanesList = new List<GameObject>();

                InfoPanelReadingsArr = InfoPanelReadingsGO.GetComponentsInChildren<Text>();
                // InfoTitle = InfoPanelTextArr[0];
                InfoCallsign = InfoPanelReadingsArr[0];
                InfoSpeed = InfoPanelReadingsArr[1];
                InfoHeading = InfoPanelReadingsArr[2];
                InfoAltitude = InfoPanelReadingsArr[3];
                RunwayToLand = InfoPanelReadingsArr[4];

                // Control Panel Readings
                ControlPanelReadingsArr = ControlPanelReadingsGO.GetComponentsInChildren<Text>();
                ControlSpeed = ControlPanelReadingsArr[1];
                ControlHeading = ControlPanelReadingsArr[2];
                ControlAltitude = ControlPanelReadingsArr[3];

                // Control Panel Input Fields
                ControlInputFieldsArr = ControlPanelInputFieldsGO.GetComponentsInChildren<InputField>();
                SpeedInputField = ControlInputFieldsArr[0];
                HeadingInputField = ControlInputFieldsArr[1];
                AltitudeInputField = ControlInputFieldsArr[2];

                // Camera
                cam = Camera.main;
                radarHorizontalRadius = 50;
            }

            else { Debug.LogError("No Panels Found!"); }
        }
        else { Debug.LogError("No canvas"); }

        if (transform.childCount > 0) 
        {
            weatherRadar = GetComponentInChildren<MeshRenderer>();
        }

        b_openControlPanel = false;
        b_openInfoPanel = false;
        b_openWeatherRadar = false;

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // info panel
        {
            if (!b_openInfoPanel)
            {
                InfoPanelGO.SetActive(true);
                b_openInfoPanel = true;
            }

            else
            {
                InfoPanelGO.SetActive(false);
                b_openInfoPanel = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C)) // control panel
        {
            if (!b_openControlPanel)
            {
                ControlPanelGO.SetActive(true);
                b_openControlPanel = true;
            }

            else
            {
                ControlPanelGO.SetActive(false);
                b_openControlPanel = false;
            }
        }

        SelectPlane();

        if (targetPlaneGO != null)
        {
            // this displays information to info panel and the control panel
            if (tarPlaneTrasponder != null)
            {
                DisplayInfo(tarPlaneTrasponder);
                DisplayControlCurrentInfo(tarPlaneTrasponder);
            }

            if (transmitInstructionButtonEventListener.b_over)
            {
                if (tarPlaneTrasponder != null) { TransmitInstructions(tarPlaneTrasponder); }
                else { Debug.LogWarning("No Plane Selected Found!"); }

                transmitInstructionButtonEventListener.b_over = false; // this resets the eventListener
            }

            if (clearedILSButtonEventListener.b_over)
            {
                if (tarPlaneTrasponder != null) { ClearToILS(tarPlaneTrasponder); }
                else { Debug.LogWarning("No Plane Selected"); }

                clearedILSButtonEventListener.b_over = false;
            }
        }

        ZoomCam();

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            ToggleWeatherRadar();

        }

    }


    private void SelectPlane() // selects plane with Raycast
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask planeLayer = 1 << 8;

            if (Physics.Raycast(ray, out target, Mathf.Infinity, planeLayer))
            {
                targetPlaneGO = target.collider.gameObject;
                tarPlaneTrasponder = targetPlaneGO.GetComponent<PlaneTrasponder>();
                tarPlaneIcon = targetPlaneGO.GetComponent<MeshRenderer>();

                // this changes the color of the icon of the plane
                SetSelectedColor(tarPlaneIcon);

                selectedPlanesList.Add(targetPlaneGO);

                if (selectedPlanesList.Count > 1)
                {
                    if (selectedPlanesList[0] != selectedPlanesList[1])
                    {
                        ResetDefaultColor(selectedPlanesList[0].GetComponent<MeshRenderer>());
                        SetSelectedColor(selectedPlanesList[1].GetComponent<MeshRenderer>());
                        ClearInputFields();
                    }
                    selectedPlanesList.RemoveAt(0);
                }

            }

           else if (selectedPlanesList != null && selectedPlanesList[0] != targetPlaneGO) // 
            {
                ClearInfo();
                ClearControlCurrentInfo();
                ResetDefaultColor(tarPlaneIcon); // check default color
                ClearTarPlaneTrasponders();
                ClearInputFields();
            }

        }

    }

    private void DisplayInfo(PlaneTrasponder tarPlane)
    {
        InfoCallsign.text = "Callsing: " + tarPlane.planeName;
        InfoSpeed.text = "Speed: " + Mathf.Round(tarPlane.expectedSpeedInKTS).ToString() + " " + "KTS";
        InfoHeading.text = "Heading: " + Mathf.Round(tarPlane.expectedHeading).ToString() + " " + "DEG";
        InfoAltitude.text = "Altitude: " + Mathf.Round(tarPlane.expectedAltitude).ToString() + " " + "FT";
        RunwayToLand.text = " " + tarPlane.runwayToLand.ToString();
    }

    private void DisplayControlCurrentInfo(PlaneTrasponder tarPlane)
    {
        ControlSpeed.text = Mathf.Round(tarPlane.curSpeedInKTS).ToString() + "\r\n" + "Exptd " + Mathf.Round(tarPlane.expectedSpeedInKTS).ToString() + " " + "KTS";
        ControlHeading.text = Mathf.Round(tarPlane.curHeading).ToString() + "\r\n" + "Exptd " + Mathf.Round(tarPlane.expectedHeading).ToString() + " " + "DEG";
        ControlAltitude.text = Mathf.Round(tarPlane.curAltitude).ToString() + "\r\n" + "Exptd " + Mathf.Round(tarPlane.expectedAltitude).ToString() + " " + "FT";
        if (tarPlane.b_onLOC) { ILSGauge.text = "ON ILS"; }
        else { ILSGauge.text = "OFF ILS"; }
    }

    private void ClearInfo()
    {
        InfoCallsign.text = "";
        InfoSpeed.text = "";
        InfoHeading.text = "";
        InfoAltitude.text = "";
        RunwayToLand.text = "";
    }

    private void ClearControlCurrentInfo()
    {
        ControlSpeed.text = "";
        ControlHeading.text = "";
        ControlAltitude.text = "";
        ILSGauge.text = "OFF ILS";
    }

    private void ClearInputFields()
    {
        SpeedInputField.text = "";
        HeadingInputField.text = "";
        AltitudeInputField.text = "";
    }

    private void ClearTarPlaneTrasponders()
    {
        selectedPlanesList.Clear();
        targetPlaneGO = null;
        tarPlaneTrasponder = null;
    }

    private void SetSelectedColor(MeshRenderer tarPlaneIcon)
    {
        tarPlaneIcon.material = selectedPlaneMat;
    }
    private void ResetDefaultColor(MeshRenderer tarPlaneIcon)
    {
        tarPlaneIcon.material = defaultPlaneMat;
    }

    //needs to be public to use the method in onclick() method
    public void TransmitInstructions(PlaneTrasponder tarPlane) // add the cleared ILS???
    {
        bool b_float = true;
        Debug.Log("Transmitted instructions!");

        foreach(InputField text in ControlInputFieldsArr) // checks ig there are any letters in the input fields
        {
            foreach (char c in text.text) 
            {
                if (char.IsLetter(c)) { b_float = false; Debug.Log(b_float); break; }
            }
        }

        if (b_float)
        {
            if (ControlInputFieldsArr[0].text.Length > 0) tarPlane.expectedSpeedInKTS = float.Parse(ControlInputFieldsArr[0].text);
            if (ControlInputFieldsArr[1].text.Length > 0) tarPlane.expectedHeading = float.Parse(ControlInputFieldsArr[1].text);
            if (ControlInputFieldsArr[2].text.Length > 0) tarPlane.expectedAltitude = float.Parse(ControlInputFieldsArr[2].text);
        }

        ClearInputFields();
    }

    private void ZoomCam() {
        if (radarHorizontalRadius >= 10) { cam.orthographicSize = radarHorizontalRadius * .5f; }
    }

    private void ClearToILS(PlaneTrasponder tarplane)
    {
        if (tarplane.runwayToLand == "27L")
        {
            if (ils27L.landingPlane == null) // only one plane at a time on ILS
            {
                tarplane.b_clearedForILS = true;
                ils27L.landingPlane = tarplane;

            } // sets the current selected plane to landing plane for ILS 
            else if (ils27L.secondLandingPlane == null && tarplane != ils27L.landingPlane) 
            { 
                tarplane.b_clearedForILS = true; 
                ils27L.secondLandingPlane = tarplane; 
            }

        }

        if (tarplane.runwayToLand == "27R")
        {
            if (ils27R.landingPlane == null) // only one plane at a time on ILS
            {
                tarplane.b_clearedForILS = true;
                ils27R.landingPlane = tarplane;

            } // sets the current selected plane to landing plane for ILS 

            else if (ils27R.secondLandingPlane == null && tarplane != ils27R.landingPlane)
            {
                tarplane.b_clearedForILS = true;
                ils27R.secondLandingPlane = tarplane;
            }
        }

        if (tarplane.runwayToLand == "9L")
        {
            if (ils9L.landingPlane == null) // only one plane at a time on ILS
            {
                tarplane.b_clearedForILS = true;
                ils9L.landingPlane = tarplane;

            } // sets the current selected plane to landing plane for ILS 


            else if (ils9L.secondLandingPlane == null && tarplane != ils9L.landingPlane)
            {
                tarplane.b_clearedForILS = true;
                ils9L.secondLandingPlane = tarplane;
            }
        }

        if (tarplane.runwayToLand == "9R")
        {
            if (ils9R.landingPlane == null) // only one plane at a time on ILS
            {
                tarplane.b_clearedForILS = true;
                ils9R.landingPlane = tarplane;

            } // sets the current selected plane to landing plane for ILS 

            else if (ils9R.secondLandingPlane == null && tarplane != ils9R.landingPlane)
            {
                tarplane.b_clearedForILS = true;
                ils9R.secondLandingPlane = tarplane;
            }
        }
    }

    private void ToggleWeatherRadar() 
    {
        if (!b_openWeatherRadar) 
        { 
            weatherRadar.enabled = true;
            weatherRadarText.text = "Weather Radar  ON";
            b_openWeatherRadar = true; 
        }
        else 
        { 
            weatherRadar.enabled = false;
            weatherRadarText.text = "Weather Radar  OFF";
            b_openWeatherRadar = false; 
        }
    }

  


}

