using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class GameController : MonoBehaviour {
    public GameData currentGameData = new GameData();
    public AudioMixer myMixer;
    public AudioMixer resonanceMixer;
    public Slider musicSlider;
    public Slider fxSlider;
    public GameObject warningPanel;
    public bool loadData = false;
    public bool gameWindowed = false;
    [HideInInspector] public static GameController gameControllerInstance;

    private Transform quiver;
    private Transform horde;

    public string selectedLanguage = "Español";

    private void Awake()
    {
        if (gameControllerInstance == null)
            gameControllerInstance = this;
        else
        {
            Debug.LogError("Tried to create a second GameController");
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        currentGameData.playerMadness = 0;
        currentGameData.playerPosX = -14.437f;
        currentGameData.playerPosY = 5.742f;
        currentGameData.playerPosZ = -36.241f;
        currentGameData.musicVolume = 0.0f;
        currentGameData.fxVolume = 0.0f;

        float audioBusValue;
        bool changedAttenuation;
        changedAttenuation = myMixer.GetFloat("AmbientVolume", out audioBusValue);
        Debug.Log("audioBusValue: " + audioBusValue);
        Debug.Log("changedAttenuation: " + changedAttenuation);
        if (changedAttenuation) musicSlider.value = audioBusValue;
        changedAttenuation = resonanceMixer.GetFloat("FXVolume", out audioBusValue);
        if (changedAttenuation) fxSlider.value = audioBusValue;

        gameWindowed = Screen.fullScreen;

        Debug.Log("GameControllerStart");
        //if (SceneManager.GetActiveScene().name != "MainMenus")
        //    LoadPlayerData();
    }


    // Update is called once per frame
    void Update () {

        /*if (Input.GetKeyDown("escape"))
        {
            PlayerController.playerControllerInstance.playerControl = false;
            Application.Quit();
        }*/

        if (Input.GetKeyDown(KeyCode.Backspace))
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
            LoadPlayerDataFromDisk("myGameData.txt");
            Debug.Log("Loaded myGameData.txt");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SavePlayerDataToDisk("myGameData.txt");
            Debug.Log("Saved myGameData.txt");

        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            LoadPlayerData(); //Carga de la memoria de juego (GameData)
            Debug.Log("Loaded");
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SavePlayerData();//Guarda a la memoria de juego (GameData)
            Debug.Log("Saved");
        }
    }

    public void SavePlayerDataToDisk(string fileName)
    {
        // Update local data with current game data
        SavePlayerData(); //optional

        // 1) Path check
        if (!Directory.Exists("myGameFolder")) Directory.CreateDirectory("myGameFolder");
        // 2) Binary formatter
        BinaryFormatter myFormatter = new BinaryFormatter();
        // 3) Create file
        if (File.Exists("myGameFolder/" + fileName)) File.Delete("myGameFolder/" + fileName);
        FileStream myFile = File.Create("myGameFolder/" + fileName);
        // 4) Reference to data being saved
        //GameStatistics localData = GameData.g_GameDataInstance.savedGameData;
        // 5) Writing data in binary form
        myFormatter.Serialize(myFile, currentGameData);
        // 6) Close file!!!!! EXTREMELY IMPORTANT
        myFile.Close();
    }

    public bool LoadPlayerDataFromDisk(string fileName)
    {
        // 0) Binary formatter
        BinaryFormatter myFormatter = new BinaryFormatter();
        // 1) File opening
        FileStream myFile = null;
        if (File.Exists("myGameFolder/"+fileName))
        {
            myFile = File.Open("myGameFolder/" + fileName, FileMode.Open);
            // 2) Deserialize to temporal variable
            currentGameData = (GameData)myFormatter.Deserialize(myFile);
            // 3) Close file!!!!! EXTREMELY IMPORTANT
            myFile.Close();
            return true;
        }
        else
        {
            warningPanel.SetActive(true);
            return false;
        }
      
    }

    void SavePlayerData()
    {
        //Player position and madness
        currentGameData.playerMadness = PlayerController.playerControllerInstance.madness;
        currentGameData.playerPosX = PlayerController.playerControllerInstance.transform.position.x;
        currentGameData.playerPosY = PlayerController.playerControllerInstance.transform.position.y;
        currentGameData.playerPosZ = PlayerController.playerControllerInstance.transform.position.z;

        //Urbanization objects
        currentGameData.foodTaken = PlayerController.playerControllerInstance.foodTaken;
        currentGameData.diaryPageTaken = PlayerController.playerControllerInstance.diaryPageTaken;

        //Player flags
        currentGameData.makeUpTaken = PlayerController.playerControllerInstance.isMadeUp;
        currentGameData.ladderTaken = PlayerController.playerControllerInstance.hasLadder;
        currentGameData.wineTaken = PlayerController.playerControllerInstance.hasWine;
        currentGameData.luculoTaken = PlayerController.playerControllerInstance.hasCat;

        //NPCs condition
        currentGameData.npcInteracted[0] = GameObject.Find("Carlos").GetComponent<InteractPerson>().alreadyInteracted;
        currentGameData.npcInteracted[1] = GameObject.Find("SeñoraRamos").GetComponent<InteractPerson>().alreadyInteracted;
        currentGameData.npcInteracted[2] = GameObject.Find("Jaime").GetComponent<InteractPerson>().alreadyInteracted;
        currentGameData.npcInteracted[3] = GameObject.Find("Paco").GetComponent<InteractPerson>().alreadyInteracted;

        //Inventory
        currentGameData.itemList = new List<Item>(Inventory.inventoryInstance.itemList);

        //Arrows
        currentGameData.arrowList.Clear();
        quiver = GameObject.Find(Names.quiverObject).transform;
        string parent;
        foreach (GameObject arrow in GameObject.FindGameObjectsWithTag("Arrow"))
        {
            if (arrow.transform.parent == null) parent = null;
            else parent = arrow.transform.parent.name;
            currentGameData.arrowList.Add(new ArrowData(new float[3] { arrow.transform.position.x, arrow.transform.position.y, arrow.transform.position.z }, parent, arrow.activeSelf, arrow.GetComponent<Rigidbody>().isKinematic));    
        }
        foreach(Transform arrow in quiver)
        {
            currentGameData.arrowList.Add(new ArrowData(new float[3] { arrow.position.x, arrow.position.y, arrow.transform.position.z }, arrow.parent.name, arrow.gameObject.activeSelf, arrow.GetComponent<Rigidbody>().isKinematic));
        }

        //Zombies condition
        currentGameData.zombieList.Clear();
        horde = GameObject.Find("Zombies").transform;
        foreach (Transform zombie in horde)
        {
            ZombieController zc = zombie.gameObject.GetComponent<ZombieController>();
            currentGameData.zombieList.Add(new ZombieData(new float[3] { zombie.position.x, zombie.position.y, zombie.transform.position.z }, zc.life, zombie.gameObject.activeSelf, zc.firstAttackFlag));
        }

        //Difficulty settings
        currentGameData.timeDamage = PlayerController.playerControllerInstance.timeDamage;
        currentGameData.timeIncrease = PlayerController.playerControllerInstance.timeIncrease;

        //currentGameData.SceneID = SceneManager.GetActiveScene().buildIndex;

    }
    public void LoadPlayerData()
    {
        int i = 0;
        //SceneManager.LoadScene(currentGameData.SceneID+1);
        
        //Player position and madness
        PlayerController.playerControllerInstance.madness = currentGameData.playerMadness;
        PlayerController.playerControllerInstance.transform.position = new Vector3(currentGameData.playerPosX, currentGameData.playerPosY, currentGameData.playerPosZ);

        //Urbanization objects
        i = 0;
        GameObject.Find("Tupper0").SetActive(!currentGameData.foodTaken[0]);
        GameObject.Find("Tupper1").SetActive(!currentGameData.foodTaken[1]);
        GameObject.Find("Tupper2").SetActive(!currentGameData.foodTaken[2]);
        GameObject.Find("Tupper3").SetActive(!currentGameData.foodTaken[3]);
        GameObject.Find("Tupper4").SetActive(!currentGameData.foodTaken[4]);
        GameObject.Find("Tupper5").SetActive(!currentGameData.foodTaken[5]);
        GameObject.Find("DiaryPage1").SetActive(!currentGameData.diaryPageTaken[0]);
        GameObject.Find("DiaryPage2").SetActive(!currentGameData.diaryPageTaken[1]);
        GameObject.Find("DiaryPage3").SetActive(!currentGameData.diaryPageTaken[2]);
        GameObject.Find("DiaryPage4").SetActive(!currentGameData.diaryPageTaken[3]);

        GameObject.Find(Names.makeup).SetActive(!currentGameData.makeUpTaken);
        GameObject.Find(Names.wine).SetActive(!currentGameData.ladderTaken);
        GameObject.Find(Names.ladder).SetActive(!currentGameData.wineTaken);
        GameObject.Find(Names.cat).SetActive(!currentGameData.luculoTaken);

        //Player flags
        PlayerController.playerControllerInstance.isMadeUp = currentGameData.makeUpTaken;
        PlayerController.playerControllerInstance.hasLadder = currentGameData.ladderTaken;
        PlayerController.playerControllerInstance.hasWine = currentGameData.wineTaken;
        PlayerController.playerControllerInstance.hasCat = currentGameData.luculoTaken;
        PlayerController.playerControllerInstance.hasFood = currentGameData.foodTaken[0];

        //NPCs condition
        GameObject.Find("Carlos").GetComponent<InteractPerson>().alreadyInteracted = currentGameData.npcInteracted[0];
        GameObject.Find("SeñoraRamos").GetComponent<InteractPerson>().alreadyInteracted = currentGameData.npcInteracted[1];
        GameObject.Find("Jaime").GetComponent<InteractPerson>().alreadyInteracted = currentGameData.npcInteracted[2];
        GameObject.Find("Paco").GetComponent<InteractPerson>().alreadyInteracted = currentGameData.npcInteracted[3];

        //Inventory
        Inventory.inventoryInstance.itemList = new List<Item>(currentGameData.itemList);
        Inventory.inventoryInstance.UpdateSlots();

        //Arrows
        quiver = GameObject.Find(Names.quiverObject).transform;
        i = 0;
        foreach (Transform arrow in quiver)
        {
            if (currentGameData.arrowList[i].parentName == null) arrow.parent = null;
            else arrow.parent = GameObject.Find(currentGameData.arrowList[i].parentName).transform;
            arrow.position = currentGameData.arrowList[i].getPos();
            arrow.gameObject.SetActive(currentGameData.arrowList[i].isActive);
            arrow.GetComponent<Rigidbody>().isKinematic = currentGameData.arrowList[i].isKinematic;
            i++;
        }

        //Zombies condition
        horde = GameObject.Find("Zombies").transform;
        i = 0;
        foreach (Transform zombie in horde)
        {
            ZombieController zc = zombie.gameObject.GetComponent<ZombieController>();
            currentGameData.zombieList.Add(new ZombieData(new float[3] { zombie.position.x, zombie.position.y, zombie.transform.position.z }, zc.life, zombie.gameObject.activeSelf, zc.firstAttackFlag));
            zombie.position = currentGameData.zombieList[i].getPos();
            zc.life = currentGameData.zombieList[i].life;
            zombie.gameObject.SetActive(currentGameData.zombieList[i].isActive);
            zc.firstAttackFlag = currentGameData.zombieList[i].firstAttackFlag;
            i++;
        }

        //Difficulty settings
        PlayerController.playerControllerInstance.timeDamage = currentGameData.timeDamage;
        PlayerController.playerControllerInstance.timeIncrease = currentGameData.timeIncrease;
    }

    public void ChangeVolume(int index) {
        if (index == 1) {
            myMixer.SetFloat("AmbientVolume", musicSlider.value);
        } else if (index == 2) {
            resonanceMixer.SetFloat("FXVolume", fxSlider.value);
        } else if (index == 3) {
            myMixer.SetFloat("AmbientVolume", PauseMenuManager.pauseMenuManagerInstance.musicSlider.value);
        } else if (index == 4) {
            resonanceMixer.SetFloat("FXVolume", PauseMenuManager.pauseMenuManagerInstance.fxSlider.value);
        }
    }

    public void SetDifficulty(int difficult) {
        //Debug.Log("Setting difficulty");
        switch (difficult) {
            case 0:
                currentGameData.difficulty = GameData.Difficulties.easy;
                currentGameData.zombieSpeed = 3.5f;
                currentGameData.zombieMaxAtackRange = 5f;
                currentGameData.zombieVisionRange = 20f;
                currentGameData.timeDamage = 0;
                currentGameData.timeIncrease = 20;
                Debug.Log("Difficulty set to easy");
                break;
            case 1:
                currentGameData.difficulty = GameData.Difficulties.medium;
                currentGameData.zombieSpeed = 5f;
                currentGameData.zombieMaxAtackRange = 7.5f;
                currentGameData.zombieVisionRange = 30f;
                currentGameData.timeDamage = 5;
                currentGameData.timeIncrease = 20;
                Debug.Log("Difficulty set to medium");
                break;
            case 2:
                currentGameData.difficulty = GameData.Difficulties.hard;
                currentGameData.zombieSpeed = 7f;
                currentGameData.zombieMaxAtackRange = 7.5f;
                currentGameData.zombieVisionRange = 30f;
                currentGameData.timeDamage = 10;
                currentGameData.timeIncrease = 15;
                Debug.Log("Difficulty set to hard");
                break;
        }
    }


    /*void ChangeScene() {
     if (Input.GetKeyDown(KeyCode.RightArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
     if (Input.GetKeyDown(KeyCode.LeftArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
 }*/
}
