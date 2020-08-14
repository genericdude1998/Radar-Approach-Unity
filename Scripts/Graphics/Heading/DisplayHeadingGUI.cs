using UnityEngine;
[ExecuteInEditMode]
public class DisplayHeadingGUI : MonoBehaviour
{
    public HeadingGUIGenerator generator;
    public GUIStyle style;
    public Font font;
    public Color headingColor;
    // Start is called before the first frame update
    void Start()
    {
        if (generator == null) { Debug.Log("no generator!"); }
        
    }
    

   private void OnGUI()
   {
        for (int i = 0; i < generator.headingPointsPosArr.Length; i++)
        {
            Vector3 pos = generator.headingPointsPosArr[i];

            Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
            Vector2 labelSize = new Vector2(100, 100);

            Rect rect = new Rect(screenPos, labelSize);

            float number = i * 10;

            GUI.Label(rect, number.ToString(), style);
        }

    }
}
