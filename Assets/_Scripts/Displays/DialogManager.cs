using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Gestiona las selecciones en pantalla
public class DialogManager : MonoBehaviour {

    public Text question;
    public Image iconImage;
    public Button aButton;
    public Button bButton;
    public Button cButton;
    public GameObject modalPanelObject;
    [HideInInspector] public static Dictionary<string, string> dialogueDB = new Dictionary<string, string>();

    private static DialogManager modalPanel;

    private void Start() {
        InitializeDialog();
    }

    public static DialogManager Instance() {
        if (!modalPanel) {
            modalPanel = FindObjectOfType(typeof(DialogManager)) as DialogManager;
            if (!modalPanel)
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
        }

        return modalPanel;
    }

    // Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
    public void Choice(string question, UnityAction DialogEventA, UnityAction DialogEventB, UnityAction DialogEventC, bool lastDialogue, string npcName) {
        modalPanelObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        modalPanelObject.transform.SetAsLastSibling();


        aButton.onClick.RemoveAllListeners();
        aButton.onClick.AddListener(DialogEventA);
        aButton.onClick.AddListener(ClosePanel);

        bButton.onClick.RemoveAllListeners();
        bButton.onClick.AddListener(DialogEventB);
        bButton.onClick.AddListener(ClosePanel);

        cButton.onClick.RemoveAllListeners();
        cButton.onClick.AddListener(DialogEventC);
        cButton.onClick.AddListener(ClosePanel);


        this.question.text = question;

        this.iconImage.gameObject.SetActive(false);
        if (lastDialogue) {
            aButton.gameObject.SetActive(true);
            aButton.GetComponentInChildren<Text>().text = dialogueDB[npcName + "_" + "A"];
            bButton.gameObject.SetActive(true);
            bButton.GetComponentInChildren<Text>().text = dialogueDB[npcName + "_" + "B"];
            cButton.gameObject.SetActive(true);
            cButton.GetComponentInChildren<Text>().text = dialogueDB[npcName + "_" + "C"];
        }
        else {
            aButton.gameObject.SetActive(false);
            bButton.gameObject.SetActive(false);
            cButton.gameObject.SetActive(false);
        }
    }
    
    void ClosePanel() {
        modalPanelObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void InitializeDialog() {
        dialogueDB.Add("NPC001_1", "Carlos...Carlos!");
        dialogueDB.Add("NPC001_2", "Carlos: Qué pasa? Tío, te encuentras bien?");
        dialogueDB.Add("NPC001_A", "Si, como en mi vida");
        dialogueDB.Add("NPC001_B", "La verdad es que…");
        dialogueDB.Add("NPC001_C", "Sinceramente no");
        dialogueDB.Add("NPC001_AR", "Carlos: Al final el zombie no te hizo nada? Me pareció ver...Da igual...");
        dialogueDB.Add("NPC001_BR", "Carlos: Lo sabía, te mordió.");
        dialogueDB.Add("NPC001_CR", "Carlos: Estás herido?");
    }
}
