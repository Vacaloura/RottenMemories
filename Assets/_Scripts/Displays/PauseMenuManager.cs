using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {

    [HideInInspector] public static PauseMenuManager pauseMenuManagerInstance;
    public Button slot1;
    public Button slot2;
    public Button slot3;
    public Button returnButton;
    public Button resumeButton;
    public Slider musicSlider;
    public Slider fxSlider;
    public GameObject pausePanel, audioPanel, savePanel, helpPanel;

    private void Awake() {
        if (pauseMenuManagerInstance == null)
            pauseMenuManagerInstance = this;
        else Debug.LogError("Tried to create a second PauseMenuManager");
    }

    // Use this for initialization
    void Start () {
        slot1.onClick.AddListener(delegate { GameController.gameControllerInstance.SavePlayerDataToDisk("myGameData.txt"); });
        slot2.onClick.AddListener(delegate { GameController.gameControllerInstance.SavePlayerDataToDisk("myGameData2.txt"); });
        slot3.onClick.AddListener(delegate { GameController.gameControllerInstance.SavePlayerDataToDisk("myGameData3.txt"); });
        returnButton.onClick.AddListener(ReturnToMenu);
        resumeButton.onClick.AddListener(TogglePausePanel);

        musicSlider.onValueChanged.AddListener(delegate { GameController.gameControllerInstance.ChangeVolume(3); });
        fxSlider.onValueChanged.AddListener(delegate { GameController.gameControllerInstance.ChangeVolume(4); });
        float audioBusValue;
        bool changedAttenuation;
        changedAttenuation = GameController.gameControllerInstance.myMixer.GetFloat("AmbientVolume", out audioBusValue);
        if (changedAttenuation) musicSlider.value = audioBusValue;
        changedAttenuation = GameController.gameControllerInstance.resonanceMixer.GetFloat("FXVolume", out audioBusValue);
        if (changedAttenuation) fxSlider.value = audioBusValue;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePausePanel();
            if (pausePanel.activeInHierarchy) {
                pausePanel.transform.SetAsLastSibling();
            }
        }
    }

    private bool pausePanelState = false;
    CursorLockMode previousCursor;
    bool previousVis;
    void TogglePausePanel() {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        pausePanel.SetActive(!pausePanelState);
        audioPanel.SetActive(false);
        savePanel.SetActive(false);
        helpPanel.SetActive(false);
        if (!pausePanelState) {
            previousCursor = Cursor.lockState;
            previousVis = Cursor.visible;
            //Debug.Log(previousVis + "-" + previousCursor);
            Cursor.lockState = CursorLockMode.None; //Debería ser confined pero no funciona
            Cursor.visible = true;
        } else {
            Cursor.lockState = previousCursor;
            Cursor.visible = previousVis;
        }
        pausePanelState = !pausePanelState;
    }

    void ReturnToMenu() {
        SceneManager.LoadScene(0);
    }
}
