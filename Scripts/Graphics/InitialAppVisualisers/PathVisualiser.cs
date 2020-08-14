using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualiser : MonoBehaviour
{
    private GameObject[] vorArr;
    public GameObject BIG;
    public LineRenderer[] bigLineRendArr;
    public GameObject LAM;
    public LineRenderer[] lamLineRendArr;
    public GameObject OCK;
    public LineRenderer[] ockLineRendArr;
    public GameObject BNN;
    public LineRenderer[] bnnLineRendArr;

    bool b_openBIG = true;
    bool b_openLAM = true;
    bool b_openOCK = true;
    bool b_openBNN = true;

    private void Start()
    {
        vorArr = new GameObject[4];
        vorArr[0] = BIG;
        vorArr[1] = LAM;
        vorArr[2] = OCK;
        vorArr[3] = BNN;

        bigLineRendArr = BIG.GetComponentsInChildren<LineRenderer>();
        lamLineRendArr = LAM.GetComponentsInChildren<LineRenderer>();
        ockLineRendArr = OCK.GetComponentsInChildren<LineRenderer>();
        bnnLineRendArr = BNN.GetComponentsInChildren<LineRenderer>();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = 1 << 9;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                GameObject selectedVORGO = hit.collider.gameObject.transform.GetChild(0).gameObject; // sets active the path by finding the first child

                if (selectedVORGO == BIG) { ToggleBIGPath(); }

                if (selectedVORGO == LAM) { ToggleLAMPath(); }

                if (selectedVORGO == OCK) { ToggleOCKPath(); }

                if (selectedVORGO == BNN) { ToggleBNNPath(); }
              
            }
        }
    }

    private void ToggleBIGPath() 
    {
        Debug.Log("we selected BIG");

        foreach (LineRenderer linerend in bigLineRendArr)
        {
            if (!b_openBIG)
            {
                linerend.enabled = true;
            }

            else { linerend.enabled = false; }
        }
        foreach (LineRenderer linerend in bigLineRendArr) // they are all on in the array then change b_open accordingly
        {
            if (linerend.enabled == true) { b_openBIG = true; }
            if (linerend.enabled != true) { b_openBIG = false; }
        }
    }

    private void ToggleLAMPath()
    {
        Debug.Log("we selected LAM");

        foreach (LineRenderer linerend in lamLineRendArr)
        {
            if (!b_openLAM)
            {
                linerend.enabled = true;
            }

            else { linerend.enabled = false; }
        }
        foreach (LineRenderer linerend in lamLineRendArr) // they are all on in the array then change b_open accordingly
        {
            if (linerend.enabled == true) { b_openLAM = true; }
            if (linerend.enabled != true) { b_openLAM = false; }
        }
    }

    private void ToggleOCKPath()
    {
        Debug.Log("we selected OCK");

        foreach (LineRenderer linerend in ockLineRendArr)
        {
            if (!b_openOCK)
            {
                linerend.enabled = true;
            }

            else { linerend.enabled = false; }
        }
        foreach (LineRenderer linerend in ockLineRendArr) // they are all on in the array then change b_open accordingly
        {
            if (linerend.enabled == true) { b_openOCK = true; }
            if (linerend.enabled != true) { b_openOCK = false; }
        }
    }

    private void ToggleBNNPath()
    {
        Debug.Log("we selected BNN");

        foreach (LineRenderer linerend in bnnLineRendArr)
        {
            if (!b_openBNN)
            {
                linerend.enabled = true;
            }

            else { linerend.enabled = false; }
        }
        foreach (LineRenderer linerend in bnnLineRendArr) // they are all on in the array then change b_open accordingly
        {
            if (linerend.enabled == true) { b_openBNN = true; }
            if (linerend.enabled != true) { b_openBNN = false; }
        }
    }

}
