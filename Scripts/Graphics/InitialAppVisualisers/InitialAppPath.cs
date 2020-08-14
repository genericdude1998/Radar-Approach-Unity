using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InitialAppPath : MonoBehaviour
{
    public Vector3[] selectedInitialAppWaypoints;
    public int waypointsCount = 0;
    private float pathWidth = .1f;
    private LineRenderer selectedInitialAppLineRenderer;
    private bool b_openVisualiser;

    private void Start()
    {
        waypointsCount = transform.childCount;
        selectedInitialAppWaypoints = new Vector3[waypointsCount];

        for (int i = 0; i < waypointsCount; i++)
        {
            selectedInitialAppWaypoints[i] = transform.GetChild(i).transform.position;
        }

        selectedInitialAppLineRenderer = gameObject.GetComponent<LineRenderer>();
        selectedInitialAppLineRenderer.startWidth = pathWidth;
        selectedInitialAppLineRenderer.endWidth = pathWidth;

        //gameObject.SetActive(false);

    }

    private void Update()
    {
        selectedInitialAppLineRenderer.positionCount = waypointsCount;
        selectedInitialAppLineRenderer.SetPositions(selectedInitialAppWaypoints);
    }
}
