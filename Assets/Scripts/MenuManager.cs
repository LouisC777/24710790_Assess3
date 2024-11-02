using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour
{
    
    public void LoadLevel1()
    {
        SceneManager.LoadScene("PacStudent"); 
    }

    
    public void LoadStartScreen()
    {
        SceneManager.LoadScene("StartScene"); 
    }
}
