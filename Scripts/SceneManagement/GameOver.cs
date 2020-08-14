using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    PlaneSpawner plnSpawner;
    // Start is called before the first frame update
    void Start()
    {
        plnSpawner = FindObjectOfType<PlaneSpawner>();
        Debug.Log(plnSpawner.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
