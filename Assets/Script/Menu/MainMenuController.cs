using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    
    public GameObject settingMenu;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void PlayGame()
    {
        GameObjectManager.Instance.isNewGame = true;
        //Load scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void LoadGame()
    {
        GameObjectManager.Instance.isNewGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    
    public void Setting()
    {
        settingMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        GameObjectManager.Instance.PauseGame();
    }
    public void ResumeGame()
    {
        GameObjectManager.Instance.ResumeGame();
    }
    // public void SaveGame()
    // {
    //     GameObjectManager.Instance.SaveGame(player);
    // }
    //===========For pause menu in game==========
    public void GetMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}
