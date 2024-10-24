using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class MenuManager : MonoBehaviour
{
    // This method will be called when the "Level 1" button is pressed
    public void LoadLevel1()
    {
        SceneManager.LoadScene("PacStudent"); // Load the PacStudent scene
    }

    // This method will be called when the "Exit" button is pressed
    public void LoadStartScreen()
    {
        SceneManager.LoadScene("StartScene"); // Load the Start Screen scene
    }
}
