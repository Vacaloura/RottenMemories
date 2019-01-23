using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;
    public Dropdown difficult;
    private bool buttonSelected;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false) {
            eventSystem.SetSelectedGameObject(selectedObject);
            Debug.Log("selectedObject: " + selectedObject.name);
            buttonSelected = true;
        }
    }

    private void OnDisable() {
        buttonSelected = false;
    }

    public void OnApplicationQuit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadNew() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameController.gameControllerInstance.loadData = false;
        GameController.gameControllerInstance.SetDifficulty(difficult.value);
    }

    public void LoadOld(string fileName)
    {
        GameController.gameControllerInstance.LoadPlayerDataFromDisk(fileName);
        GameController.gameControllerInstance.loadData = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
