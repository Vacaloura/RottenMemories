using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameController : MonoBehaviour {
    public GameData currentGameData = new GameData();
    public AudioMixer mixer;
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
        currentGameData.playerMadness = 0;
        currentGameData.playerPosX = -14.437f;
        currentGameData.playerPosY = 5.742f;
        currentGameData.playerPosZ = -36.241f;
        currentGameData.musicVolume = 0.5f;
        currentGameData.fxVolume = 0.5f;

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
        currentGameData.playerPosX = PlayerController.playerControllerInstance.transform.position.x;
        currentGameData.playerPosY = PlayerController.playerControllerInstance.transform.position.y;
        currentGameData.playerPosZ = PlayerController.playerControllerInstance.transform.position.z;

        currentGameData.foodTaken = PlayerController.playerControllerInstance.foodTaken;
        currentGameData.diaryPageTaken = PlayerController.playerControllerInstance.diaryPageTaken;
        currentGameData.makeUpTaken = PlayerController.playerControllerInstance.isMadeUp;
        currentGameData.ladderTaken = PlayerController.playerControllerInstance.hasLadder;
        currentGameData.wineTaken = PlayerController.playerControllerInstance.hasWine;
        currentGameData.luculoTaken = PlayerController.playerControllerInstance.hasCat;

        currentGameData.SceneID = SceneManager.GetActiveScene().buildIndex;
    }
    public void LoadPlayerData()
    {
        //SceneManager.LoadScene(currentGameData.SceneID+1);
        PlayerController.playerControllerInstance.madness = currentGameData.playerMadness;
        PlayerController.playerControllerInstance.transform.position = new Vector3(currentGameData.playerPosX, currentGameData.playerPosY, currentGameData.playerPosZ);

        GameObject.Find("Food000").SetActive(!currentGameData.foodTaken[0]);
        GameObject.Find("Food001").SetActive(!currentGameData.foodTaken[1]);
        GameObject.Find("Food002").SetActive(!currentGameData.foodTaken[2]);
        GameObject.Find("Food003").SetActive(!currentGameData.foodTaken[3]);
        GameObject.Find("Food004").SetActive(!currentGameData.foodTaken[4]);
        GameObject.Find("Food005").SetActive(!currentGameData.foodTaken[5]);
        GameObject.Find("DiaryPage1").SetActive(!currentGameData.diaryPageTaken[0]);
        GameObject.Find("DiaryPage2").SetActive(!currentGameData.diaryPageTaken[1]);
        GameObject.Find("DiaryPage3").SetActive(!currentGameData.diaryPageTaken[2]);
        GameObject.Find("DiaryPage4").SetActive(!currentGameData.diaryPageTaken[3]);
        PlayerController.playerControllerInstance.isMadeUp = currentGameData.makeUpTaken;
        PlayerController.playerControllerInstance.hasLadder = currentGameData.ladderTaken;
        PlayerController.playerControllerInstance.hasWine = currentGameData.wineTaken;
        PlayerController.playerControllerInstance.hasCat = currentGameData.luculoTaken;
        PlayerController.playerControllerInstance.hasFood = currentGameData.foodTaken[0];
    }

    public void ChangeVolume(int index) {
        if (index == 1) {
            mixer.SetFloat("AmbientVolume", GameObject.Find(Names.musicSlider).GetComponent<Slider>().value);
        } else if (index == 2) {
            mixer.SetFloat("FXVolume", GameObject.Find(Names.fxSlider).GetComponent<Slider>().value);
        }
    }

    public void SetDifficulty(int mode) {
        Debug.Log("setting difficulty");
        switch (mode) {
            case 0:
                currentGameData.difficulty = GameData.Difficulties.easy;
                currentGameData.zombieSpeed = 3.5f;
                currentGameData.zombieMaxAtackRange = 5f;
                currentGameData.zombieVisionRange = 20f;
                Debug.Log("Difficulty set to easy");
                break;
            case 1:
                currentGameData.difficulty = GameData.Difficulties.medium;
                currentGameData.zombieSpeed = 5f;
                currentGameData.zombieMaxAtackRange = 7.5f;
                currentGameData.zombieVisionRange = 30f;
                Debug.Log("Difficulty set to medium");
                break;
            case 2:
                currentGameData.difficulty = GameData.Difficulties.hard;
                currentGameData.zombieSpeed = 7f;
                currentGameData.zombieMaxAtackRange = 10f;
                currentGameData.zombieVisionRange = 45f;
                Debug.Log("Difficulty set to hard");
                break;
        }
    }


    /*void ChangeScene() {
     if (Input.GetKeyDown(KeyCode.RightArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
     if (Input.GetKeyDown(KeyCode.LeftArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
 }*/
}
