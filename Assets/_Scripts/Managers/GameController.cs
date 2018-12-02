using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameController : MonoBehaviour {
    public GameData currentGameData = new GameData();
    [HideInInspector] public static GameController gameControllerInstance;

    private void Awake()
    {
        if (gameControllerInstance == null)
            gameControllerInstance = this;
        else Debug.LogError("Tried to create a second GameController");
    }
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        currentGameData.posx = -14.437f;
        currentGameData.posy = 5.742f;
        currentGameData.posz = -36.241f;
        currentGameData.playerMadness = 20;
        //if (SceneManager.GetActiveScene().name != "MainMenus")
        //    LoadPlayerData();
    }


    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("escape"))
        {
            PlayerController.playerControllerInstance.playerControl = false;
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        CheckInputEntries();
    }

    private void CheckInputEntries()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            //LoadPlayerDataFromDisk();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SavePlayerDataToDisk();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            LoadPlayerData(); //Carga de la memoria de juego (GameData)
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SavePlayerData();//Guarda a la memoria de juego (GameData)
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {    // for testing
            currentGameData.playerMadness += 30;
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {    // for testing
           // currentGameData.playerStress -= 25;
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            DebugPlayerData();
        }
    }

    private void SavePlayerDataToDisk()
    {
        // Update local data with current game data
        SavePlayerData(); //optional

        // 1) Path check
        if (!Directory.Exists("myGameFolder")) Directory.CreateDirectory("myGameFolder");
        // 2) Binary formatter
        BinaryFormatter myFormatter = new BinaryFormatter();
        // 3) Create file
        FileStream myFile = File.Create("myGameFolder/myGameData.txt");
        // 4) Reference to data being saved
        //GameStatistics localData = GameData.g_GameDataInstance.savedGameData;
        // 5) Writing data in binary form
        myFormatter.Serialize(myFile, currentGameData);
        // 6) Close file!!!!! EXTREMELY IMPORTANT
        myFile.Close();
        Debug.Log("Saved!");
    }

    public void LoadPlayerDataFromDisk(string fileName)
    {
        // 0) Binary formatter
        BinaryFormatter myFormatter = new BinaryFormatter();
        // 1) File opening
        FileStream myFile = null;
        if (File.Exists("myGameFolder/"+fileName))
        {
            myFile = File.Open("myGameFolder/" + fileName, FileMode.Open);
        }
        // 2) Deserialize to temporal variable
        currentGameData = (GameData)myFormatter.Deserialize(myFile);
        // 3) Close file!!!!! EXTREMELY IMPORTANT
        myFile.Close();
        // 4) Decide what to do with the loaded data
        //LoadPlayerData();
    }

    private void DebugPlayerData()
    {
        Debug.Log("Object.name:" + this.name);
        Debug.Log("    Health:" + currentGameData.playerMadness);
        //Debug.Log("    Stress:" + currentGameData.playerStress);

    }

    void SavePlayerData()
    {
        currentGameData.playerMadness = PlayerController.playerControllerInstance.madness;
        currentGameData.posx = PlayerController.playerControllerInstance.transform.position.x;
        currentGameData.posy = PlayerController.playerControllerInstance.transform.position.y;
        currentGameData.posz = PlayerController.playerControllerInstance.transform.position.z;
        currentGameData.SceneID = SceneManager.GetActiveScene().buildIndex;
    }
    public void LoadPlayerData()
    {
        //SceneManager.LoadScene(currentGameData.SceneID+1);
        PlayerController.playerControllerInstance.madness = currentGameData.playerMadness;
        PlayerController.playerControllerInstance.transform.position = new Vector3(currentGameData.posx, currentGameData.posy, currentGameData.posz);

    }


    /*void ChangeScene() {
     if (Input.GetKeyDown(KeyCode.RightArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
     if (Input.GetKeyDown(KeyCode.LeftArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
 }*/
}
