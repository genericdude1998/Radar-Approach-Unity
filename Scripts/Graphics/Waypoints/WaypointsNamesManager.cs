
using UnityEngine;
[ExecuteInEditMode]
public class WaypointsNamesManager : MonoBehaviour
{
    public WaypointsVisualisation[] waypointsNameArr;
    // Start is called before the first frame update
    private bool b_open;
    public GUIStyle stylePrefab;
    void Start()
    {
        waypointsNameArr = FindObjectsOfType<WaypointsVisualisation>();
        foreach (WaypointsVisualisation wp in waypointsNameArr )
        {
            wp.style = stylePrefab;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!b_open)
                foreach (WaypointsVisualisation wp in waypointsNameArr)
                {
                    wp.enabled = true;
                    b_open = true;
                }

            else
            {
                foreach (WaypointsVisualisation wp in waypointsNameArr)
                {
                    wp.enabled = false;
                    b_open = false;
                }
            }
        }
      
    }

    
    
}
