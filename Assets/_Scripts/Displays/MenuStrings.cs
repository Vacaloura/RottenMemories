using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuStrings : MonoBehaviour {


    [HideInInspector] public static MenuStrings menuStringsInstance;
    public GameObject selector;

    private void Awake()
    {
        if (menuStringsInstance == null)
            menuStringsInstance = this;
        else Debug.LogError("Tried to create a second MenuStrings");
    }
    // Use this for initialization
    void Start () {
        try
        {
            Dropdown language = selector.GetComponent<Dropdown>();
            language.onValueChanged.AddListener(delegate { UpdateCanvas(language.options[language.value].text); });
            switch (GameController.gameControllerInstance.selectedLanguage)
            {
                default:

                case "Español":
                    language.value = 0;
                    break;
                case "English":
                    language.value = 1;
                    break;
                case "Galego":
                    language.value = 2;
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("No se encuentra el selector " + e.ToString());
        }
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public Text startButton;
    public Text languageButton;
    public Text DifficultyButton;
    public Text AudioButton;
    public Text QuitButton;
    public Text audioLabel;
    public Text musicLabel;
    public Text efectLabel;
    public Text audioBack;
    public Text languageLabel;
    public Text languageText;
    public Text languageBack;
    public Text newButton;
    public Text loadButton;
    public Text gameBack;
    public Text difficultyLabel;
    public Text difficultyText;
    public Text difficultyBack;
    public Text loadBack;


    public void UpdateCanvas(string selectedLanguage)
    {
        GameController.gameControllerInstance.selectedLanguage = selectedLanguage;
        switch (selectedLanguage)
        {
            default:

            case "Español":
                //MainMenu
                startButton.text = "Jugar";
                languageButton.text = "Idioma";
                DifficultyButton.text = "Dificultad";
                AudioButton.text = "Audio";
                QuitButton.text = "Salir";
                //Audio
                audioLabel.text = "Ajustes de audio";
                musicLabel.text = "Música";
                efectLabel.text = "Efectos:";
                audioBack.text = "Volver";
                //Language
                languageLabel.text = "Idioma";
                languageText.text = "Selecciona tu idioma:";
                languageBack.text = "Volver";
                //Game
                newButton.text = "Nueva partida";
                loadButton.text = "Cargar Partida";
                gameBack.text = "Volver";
                //Difficulty
                difficultyLabel.text = "Dificultad";
                difficultyText.text = "Elige tu nivel de dificultad:";
                difficultyBack.text = "Volve";
                //Load
                loadBack.text = "Volver";

                break;

            case "English":
                //MainMenu
                startButton.text = "Play";
                languageButton.text = "Language";
                DifficultyButton.text = "Difficulty";
                AudioButton.text = "Audio";
                QuitButton.text = "Quit";
                //Audio
                audioLabel.text = "Audio settings";
                musicLabel.text = "Music";
                efectLabel.text = "FX";
                audioBack.text = "Go Back";
                //Language
                languageLabel.text = "Language";
                languageText.text = "Choose your language:";
                languageBack.text = "Go Back";
                //Game
                newButton.text = "New Game";
                loadButton.text = "Load Game";
                gameBack.text = "Go Back";
                //Difficulty
                difficultyLabel.text = "Difficulty";
                difficultyText.text = "Choose your difficulty level:";
                difficultyBack.text = "Go back";
                //Load
                loadBack.text = "Go back";

                break;

        }
    }
}
