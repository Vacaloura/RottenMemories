using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Gestiona las selecciones en pantalla
public class DialogManager : MonoBehaviour {

    public Text question;
    public string iconName;
    public Button aButton;
    public Button bButton;
    public Button cButton;
    public GameObject modalPanelObject;
    [HideInInspector] public static Dictionary<string, string> SeñoraRamos = new Dictionary<string, string>();
    [HideInInspector] public static Dictionary<string, string> Carlos = new Dictionary<string, string>();
    [HideInInspector] public static Dictionary<string, string> Paco = new Dictionary<string, string>();
    [HideInInspector] public static Dictionary<string, string> Paco_2 = new Dictionary<string, string>();
    [HideInInspector] public static Dictionary<string, string> Jaime = new Dictionary<string, string>();

    public float pause = 0.025f;
    string message;
    [HideInInspector] public Text dialogShowed;
    [HideInInspector] public bool textRetard;




    [HideInInspector] public static DialogManager modalPanel;
    private void Awake()
    {
        if (modalPanel == null)
            modalPanel = this;
        else Debug.LogError("Tried to create a second modalPanel");
    }

    private void Start() {
        InitializeDialog();
    }

    IEnumerator TypeLetters() {
        // Iterate over each letter
        textRetard = true;
        yield return new WaitForSeconds(0.1f);
        foreach (char letter in message.ToCharArray()) {
            dialogShowed.text += letter; // Add a single character to the GUI text
            if (!Input.GetKey(KeyCode.E)) {
                yield return new WaitForSeconds(pause);
            }
        }
        textRetard = false;
    }

    public void DisplayPhrase(string phrase, string iconName)
    {
        aButton.gameObject.SetActive(false);
        bButton.gameObject.SetActive(false);
        cButton.gameObject.SetActive(false);
        DisplayManager.displayManagerInstance.dialogSprite.sprite = Resources.Load<Sprite>(iconName);
        modalPanelObject.SetActive(true);
        dialogShowed = GameObject.Find("DialogScrollView").transform.GetChild(0).GetComponent<Text>();
        message = phrase;
        dialogShowed.text = ""; // Clear the GUI text
        StartCoroutine(TypeLetters());
        Cursor.lockState = CursorLockMode.None;
        modalPanelObject.transform.SetAsLastSibling();
        //this.question.text = phrase;
    }

    public void DisplayButtons(int nOptions, UnityAction DialogEventA, UnityAction DialogEventB, UnityAction DialogEventC, string[] options)
    {
        modalPanelObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        modalPanelObject.transform.SetAsLastSibling();

        if (nOptions >= 1)
        {
            aButton.onClick.RemoveAllListeners();
            aButton.onClick.AddListener(DialogEventA);
            aButton.GetComponentInChildren<Text>().text = options[0];
            aButton.gameObject.SetActive(true);
        }
        if (nOptions >= 2)
        {
            bButton.onClick.RemoveAllListeners();
            bButton.onClick.AddListener(DialogEventB);
            bButton.GetComponentInChildren<Text>().text = options[1];
            bButton.gameObject.SetActive(true);
        }
        if (nOptions >= 3)
        {
            cButton.onClick.RemoveAllListeners();
            cButton.onClick.AddListener(DialogEventC);
            cButton.GetComponentInChildren<Text>().text = options[2];
            cButton.gameObject.SetActive(true);

        }

    }

    
    public void ClosePanel() {
        modalPanelObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void InitializeDialog() {
        SeñoraRamos.Clear();
        Carlos.Clear();
        Paco.Clear();
        Paco_2.Clear();
        Jaime.Clear();

        SeñoraRamos.Add("P_1", GameStrings.gameStringsInstance.GetString("RamosP_1", null));
        SeñoraRamos.Add("N_2", GameStrings.gameStringsInstance.GetString("RamosN_2", null));
        SeñoraRamos.Add("B_A", GameStrings.gameStringsInstance.GetString("RamosB_A", null));
        SeñoraRamos.Add("B_B", GameStrings.gameStringsInstance.GetString("RamosB_B", null));
        SeñoraRamos.Add("B_C", GameStrings.gameStringsInstance.GetString("RamosB_C", null));
        SeñoraRamos.Add("R_A", GameStrings.gameStringsInstance.GetString("RamosR_A", null));
        SeñoraRamos.Add("R_B", GameStrings.gameStringsInstance.GetString("RamosR_B", null));
        SeñoraRamos.Add("R_C", GameStrings.gameStringsInstance.GetString("RamosR_C", null));
        SeñoraRamos.Add("I_1", GameStrings.gameStringsInstance.GetString("RamosI_1", null));
        SeñoraRamos.Add("I_2", GameStrings.gameStringsInstance.GetString("RamosI_2", null));

        Carlos.Add("P_1", GameStrings.gameStringsInstance.GetString("CarlosP_1", null));
        Carlos.Add("N_2", GameStrings.gameStringsInstance.GetString("CarlosN_2", null));
        Carlos.Add("B_A", GameStrings.gameStringsInstance.GetString("CarlosB_A", null));
        Carlos.Add("B_B", GameStrings.gameStringsInstance.GetString("CarlosB_B", null));
        Carlos.Add("B_C", GameStrings.gameStringsInstance.GetString("CarlosB_C", null));
        Carlos.Add("R_A", GameStrings.gameStringsInstance.GetString("CarlosR_A", null));
        Carlos.Add("R_B", GameStrings.gameStringsInstance.GetString("CarlosR_B", null));
        Carlos.Add("R_C", GameStrings.gameStringsInstance.GetString("CarlosR_C", null));
        Carlos.Add("A_B", GameStrings.gameStringsInstance.GetString("CarlosA_B", null));
        Carlos.Add("A_C", GameStrings.gameStringsInstance.GetString("CarlosA_C", null));
        Carlos.Add("N_3", GameStrings.gameStringsInstance.GetString("CarlosN_3", null));
        Carlos.Add("P_4", GameStrings.gameStringsInstance.GetString("CarlosP_4", null));
        Carlos.Add("I_1", GameStrings.gameStringsInstance.GetString("CarlosI_1", null));
        Carlos.Add("I_2", GameStrings.gameStringsInstance.GetString("CarlosI_2", null));

        Paco.Add("P_1", GameStrings.gameStringsInstance.GetString("PacoP_1", null));
        Paco.Add("N_2", GameStrings.gameStringsInstance.GetString("PacoN_2", null));
        Paco.Add("B_A", GameStrings.gameStringsInstance.GetString("PacoB_A", null));
        Paco.Add("B_B", GameStrings.gameStringsInstance.GetString("PacoB_B", null));
        Paco.Add("R_A", GameStrings.gameStringsInstance.GetString("PacoR_A", null));
        Paco.Add("R_B", GameStrings.gameStringsInstance.GetString("PacoR_B", null));
        Paco.Add("P_3", GameStrings.gameStringsInstance.GetString("PacoP_3", null));
        Paco.Add("N_4", GameStrings.gameStringsInstance.GetString("PacoN_4", null));
        Paco.Add("P_5", GameStrings.gameStringsInstance.GetString("PacoP_5", null));
        Paco.Add("N_6", GameStrings.gameStringsInstance.GetString("PacoN_6", null));
        Paco.Add("I_1", GameStrings.gameStringsInstance.GetString("PacoI_1", null));
        Paco.Add("I_2", GameStrings.gameStringsInstance.GetString("PacoI_2", null));

        Paco_2.Add("P_1", GameStrings.gameStringsInstance.GetString("Paco_2P_1", null));
        Paco_2.Add("N_2", GameStrings.gameStringsInstance.GetString("Paco_2N_2", null));
        Paco_2.Add("I_1", GameStrings.gameStringsInstance.GetString("Paco_2I_1", null));
        Paco_2.Add("I_2", GameStrings.gameStringsInstance.GetString("Paco_2I_2", null));

        Jaime.Add("N_1", GameStrings.gameStringsInstance.GetString("JaimeN_1", null));
        Jaime.Add("P_2", GameStrings.gameStringsInstance.GetString("JaimeP_2", null));
        Jaime.Add("N_3", GameStrings.gameStringsInstance.GetString("JaimeN_3", null));
        Jaime.Add("P_4", GameStrings.gameStringsInstance.GetString("JaimeP_4", null));
        Jaime.Add("N_5", GameStrings.gameStringsInstance.GetString("JaimeN_5", null));
        Jaime.Add("B_A", GameStrings.gameStringsInstance.GetString("JaimeB_A", null));
        Jaime.Add("B_B", GameStrings.gameStringsInstance.GetString("JaimeB_B", null));
        Jaime.Add("B_C", GameStrings.gameStringsInstance.GetString("JaimeB_C", null));
        Jaime.Add("R_A", GameStrings.gameStringsInstance.GetString("JaimeR_A", null));
        Jaime.Add("R_B", GameStrings.gameStringsInstance.GetString("JaimeR_B", null));
        Jaime.Add("R_C", GameStrings.gameStringsInstance.GetString("JaimeR_C", null));
        Jaime.Add("N_6", GameStrings.gameStringsInstance.GetString("JaimeN_6", null));
        Jaime.Add("N_7", GameStrings.gameStringsInstance.GetString("JaimeN_7", null));
        Jaime.Add("P_8", GameStrings.gameStringsInstance.GetString("JaimeP_8", null));
        Jaime.Add("N_9", GameStrings.gameStringsInstance.GetString("JaimeN_9", null));
        Jaime.Add("P_10", GameStrings.gameStringsInstance.GetString("JaimeP_10", null));
        Jaime.Add("N_11", GameStrings.gameStringsInstance.GetString("JaimeN_11", null));
        Jaime.Add("P_12", GameStrings.gameStringsInstance.GetString("JaimeP_12", null));
        Jaime.Add("I_1", GameStrings.gameStringsInstance.GetString("JaimeI_1", null));
        Jaime.Add("I_2", GameStrings.gameStringsInstance.GetString("JaimeI_2", null));
    }
}
