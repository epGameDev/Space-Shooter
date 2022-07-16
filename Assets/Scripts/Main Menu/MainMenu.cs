using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Update() 
    {
        if(Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene(1); 
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadGame()
    {
        // Loads Main Game Scene
        SceneManager.LoadScene(1); 

    }
}
