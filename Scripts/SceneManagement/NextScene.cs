
using UnityEngine;

public class NextScene : MonoBehaviour
{
  public void LoadNextScene()  
    { 
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); 
    }
    
}
