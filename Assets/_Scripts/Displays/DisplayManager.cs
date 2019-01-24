using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayManager : MonoBehaviour {

    public Text displayText;
    [HideInInspector] public GameObject interactText;
    [HideInInspector] public Transform intext;
    public GameObject displayTextPanel;
    public Image dialogSprite;

    public float displayTime;
    public float fadeTime;

    private IEnumerator fadeAlpha;

    [HideInInspector] public static DisplayManager displayManagerInstance;

    private void Awake()
    {
        try {
            interactText = GameObject.Find(Names.interactText);
            intext = interactText.transform.GetChild(0);
        } catch (System.Exception) { }
        //interactText.SetActive(false);
        if (displayManagerInstance == null)
            displayManagerInstance = this;
        else Debug.LogError("Tried to create a second DisplayManager");
    }


    /*public static DisplayManager Instance() {
        if (!displayManagerInstance) {
            displayManagerInstance = FindObjectOfType(typeof(DisplayManager)) as DisplayManager;
            if (!displayManagerInstance)
                Debug.LogError("There needs to be one active DisplayManager script on a GameObject in your scene.");
        }

        return displayManagerInstance;
    }*/

    private void Start()
    {
        UpdateMenus();
    }

    public void DisplayMessage(string message, float time) {
        displayTextPanel.SetActive(true);
        displayTime = time;
        displayText.text = message;
        SetAlpha();
    }

    void SetAlpha() {
        if (fadeAlpha != null) {
            StopCoroutine(fadeAlpha);
        }
        fadeAlpha = FadeAlpha();
        StartCoroutine(fadeAlpha);
    }

    IEnumerator FadeAlpha() {
        Color resetTextColor = displayText.color;
        Color resetPanelColor = displayTextPanel.GetComponent<Image>().color;
        resetTextColor.a = 1;
        resetPanelColor.a = 0.67f;
        displayText.color = resetTextColor;
        displayTextPanel.GetComponent<Image>().color = resetPanelColor;

        yield return new WaitForSeconds(displayTime);

        while (displayText.color.a > 0) {
            Color textColor = displayText.color;
            Color panelColor = displayTextPanel.GetComponent<Image>().color;
            textColor.a -= Time.deltaTime / fadeTime;
            panelColor.a -= Time.deltaTime / fadeTime;

            displayText.color = textColor;
            displayTextPanel.GetComponent<Image>().color = panelColor;
            yield return null;
        }
        displayTextPanel.SetActive(false);
        yield return null;
    }

    public Text removeButton;
    public Text interText;
    public Text madness;
    public Text saveButton;
    public Text audioButton;
    public Text backButton;
    public Text resumeButton;
    public Text helpButton;
    public Text efectLabel;
    public Text audioBack;
    public Text helpBack;
    public Text audioLabel;
    public Text musicLabel;
    public Text helpLabel;
    public Text saveBack;
    public Text helpText;

    private void UpdateMenus()
    {

        switch (GameStrings.gameStringsInstance.selectedLanguage)
        {
            default:

            case "Español":
                removeButton.text = "Eliminar";
                interText.text = "Pulsa 'E' para interactuar";
                madness.text = "LOCURA";
                saveButton.text = "Guardar";
                audioButton.text = "Audio";
                backButton.text = "Volver al menú principal";
                resumeButton.text = "Reanudar partida";
                helpButton.text = "Ayuda";
                audioLabel.text = "Ajustes de sonido";
                musicLabel.text = "Música";
                efectLabel.text = "Efectos";
                helpLabel.text = "Ayuda";
                audioBack.text = "Volver";
                saveBack.text = "Volver";
                helpBack.text = "Volver";
                helpText.text = "Utiliza las teclas WASD y el ratón para moverte por el mapa. Puedes pulsar la barra espaciadora para saltar y la tecla SHIFT para correr.\nPulsando la tecla TAB podrás acceder a tu inventario. En el podrás ver tus objetos haciendo click izquierdo sobre ellos así como utilizarlos haciendo doble click.\nPara interactuar con objetos o vecinos usa la tecla E mirando hacia ellos.\nCuando te encuentres con un zombie puedes dispararle usando el click derecho. ¡Cuidado!Si te quedas sin virotes pasarás un mal rato. Puedes recogerlos pulsando la tecla E.\nComo ya sabes, al pulsar ESCAPE accedes al menú de pausa. Además, si pulsas DELETE en cualquier momento volverás al último checkpoint.\nPor último, en la parte superior izquierda puedes ver tu icono de jugador y tu barra de locura ¡No dejes que se llene o perderás!";

                break;

            case "English":
                removeButton.text = "Remove";
                interText.text = "Press 'E' to interact";
                madness.text = "MADNESS";
                saveButton.text = "Save";
                audioButton.text = "Audio";
                backButton.text = "Go Back to Main Menu";
                resumeButton.text = "Resume Game";
                helpButton.text = "Help";
                audioLabel.text = "Sound settings";
                musicLabel.text = "Music";
                efectLabel.text = "FX";
                helpLabel.text = "Help";
                audioBack.text = "Go Back";
                saveBack.text = "Go Back";
                helpBack.text = "Go Back";
                helpText.text = "Find help playing";

                break;

        }
    }

}
