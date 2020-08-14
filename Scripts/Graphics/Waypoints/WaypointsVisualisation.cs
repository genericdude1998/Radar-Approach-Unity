using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointsVisualisation : MonoBehaviour
{
    private string waypointName;
    public Vector2 offset;
    
    public Vector2 size;
    private Vector2 pixelRatio;
    public GUIStyle style;
    

    void Start()
    {
        waypointName = gameObject.name;
        size = new Vector3(100, 100);
        pixelRatio = new Vector2(1188, 638);
    }

    private void OnGUI()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos = new Vector2(screenPos.x, Screen.height - screenPos.y);

        //offset = new Vector2(0, 1 - Screen.height);

        Vector2 finalScreenPos = screenPos + offset; // resolution where the manual offset is acceptable

        Rect labelRect = new Rect(finalScreenPos, size);
       
        style.fontSize = 10;
        GUI.Label(labelRect, waypointName, style);
    }
}
