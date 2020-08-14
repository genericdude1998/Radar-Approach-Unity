
using UnityEngine;

[ExecuteInEditMode]
public class WaypointsLineRendManager : MonoBehaviour
{
    private LineRenderer[] lineRenderers;
    public Material lineMat;
    
    void Start()
    {
        lineRenderers = GetComponentsInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (LineRenderer line in lineRenderers)
        {
            line.material = lineMat;
           
        }

    }
}
