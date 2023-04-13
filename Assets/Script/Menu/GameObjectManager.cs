using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameObjectManager
{
    public bool isNewGame = true;
    //Singleton
    private static GameObjectManager _instance;
    public static GameObjectManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameObjectManager();
            }
            return _instance;
        }
    }

    //Pause game
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    //Resume game
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    //Save game
    // public void SaveGame(GameObject player)
    // {
    //     string path = Path.Combine(Application.persistentDataPath, "player.hd");
    //     FileStream file = File.Create(path);
    //     //create hero data
    //     controllerHero heroController = player.GetComponent<controllerHero>();
    //     HeroData heroData = new HeroData(heroController.health, heroController.transform.position);
    //     //create binary formatter
    //     BinaryFormatter formatter = new BinaryFormatter();
    //     formatter.Serialize(file, heroData);
    //     file.Close();
    //     Debug.Log("Game saved "+ path); 
    // }
    // //Load game
    // public void LoadGame(GameObject player)
    // {
    //     string path = Path.Combine(Application.persistentDataPath, "player.hd");
    //     if(File.Exists(path))
    //     {
    //         FileStream file = File.Open(path, FileMode.Open);
    //         BinaryFormatter formatter = new BinaryFormatter();
    //         HeroData heroData = (HeroData)formatter.Deserialize(file);
    //         file.Close();

    //         //Load hero data
    //         controllerHero heroController = player.GetComponent<controllerHero>();
    //         heroController.health = heroData.health;
    //         heroController.transform.position = new Vector2(heroData.position[0], heroData.position[1]);
    //         Debug.Log("Game loaded "+ path);
    //     }
    // }
}
