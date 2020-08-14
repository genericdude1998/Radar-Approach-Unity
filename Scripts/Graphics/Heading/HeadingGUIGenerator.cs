
using UnityEngine;

[ExecuteInEditMode]
public class HeadingGUIGenerator : MonoBehaviour
{
     public GameObject prefab;
    [Range(0.5f,10)]
    float angle = 10;
    public float range = 30;
    public Vector3[] headingPointsPosArr; // maybe list?
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        headingPointsPosArr = new Vector3[36];
        offset = new Vector3();
    }

    private void Update()
    {
       // range = Camera.main.transform.position.y / 3.1f;

        for (int i = 0; i < 36; i++)
        {
            angle = 10 * Mathf.Deg2Rad;
            Vector3 location;


            float x = Mathf.Cos(angle * i - Mathf.PI *.5f) * range;
            float y = 1;
            float z = Mathf.Sin(angle  * i - Mathf.PI * .5f) * range;

            location = new Vector3(x, y, z) + offset;

            headingPointsPosArr[i] = location;

        }
    }

}
