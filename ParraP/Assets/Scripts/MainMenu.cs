using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject credits;
    public GameObject tutorial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OpenCredits() 
    {
        mainMenu.SetActive(false);
        tutorial.SetActive(false);
        credits.SetActive(true);
    }
    public void BackTOMainMenu() 
    {
        mainMenu.SetActive(true);
        tutorial.SetActive(false);
        credits.SetActive(false);
    }
    public void OpenTutorial() 
    {
        mainMenu.SetActive(false);
        tutorial.SetActive(true);
        credits.SetActive(false);
    }
}
