using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    PlaneSpawner spawner;
    bool gameOver = false;
    bool b_paused = false;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.FindObjectOfType<PlaneSpawner>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseTheGame();
        }


        if (spawner.planeCountLimit == spawner.landedPlaneList.Count)
        {
            LoadGameOver();
        }
    }

    private void OnGUI()
    {
        if (b_paused) 
        {
            text.enabled = true;
        }

        else
        {
            text.enabled = false;
        }
    }


    private void LoadGameOver()
    {
        if (!gameOver)
        {
            Debug.Log("Game Over! All Planes Landed");
            SceneManager.LoadScene(2);
            gameOver = true;
        }
    }

    private void PauseTheGame() 
    {
        if (!b_paused)
        {
            spawner.enabled = false;
            foreach (PlaneTrasponder plane in spawner.approachPlaneList)
            {
                plane.enabled = false;
            }

            b_paused = true;
        }

        else
        {
            spawner.enabled = true;
            foreach (PlaneTrasponder plane in spawner.approachPlaneList)
            {
                plane.enabled = true;
            }
            b_paused = false;
        }
    }
  
}
