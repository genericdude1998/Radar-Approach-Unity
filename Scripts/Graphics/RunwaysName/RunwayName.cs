
using UnityEngine;
[ExecuteInEditMode]
public class RunwayName : MonoBehaviour
{
    string runNameLeft;
    string runNameRight;
    Vector2 size;
    Vector2 screenPosLeft;
    Vector2 screenPosRight;
    public float offset_X;
    public GUIStyle style;

    private void OnGUI()
    {
        size = new Vector2(100, 100);

        if (transform.position.z < 0)
        {
            screenPosLeft = Camera.main.WorldToScreenPoint(transform.position - Vector3.right * transform.localScale.x / 1.8f + Vector3.forward * .5F);
            screenPosRight = Camera.main.WorldToScreenPoint(transform.position + Vector3.right * transform.localScale.x / 2.2F + Vector3.forward * .5F);

            runNameLeft = "9R"; runNameRight = "27L";
        }
        else
        {
            screenPosLeft = Camera.main.WorldToScreenPoint(transform.position - Vector3.right * transform.localScale.x / 1.8f - Vector3.forward * 3.5F);
            screenPosRight = Camera.main.WorldToScreenPoint(transform.position + Vector3.right * transform.localScale.x / 2.2F - Vector3.forward * 3.5F);

            runNameLeft = "9L"; runNameRight = "27R";
        }

        

        Rect leftRect = new Rect(screenPosLeft, size);
        GUI.Label(leftRect, runNameLeft, style);

        Rect rightRect = new Rect(screenPosRight, size);
        GUI.Label(rightRect, runNameRight, style);



    }
}
