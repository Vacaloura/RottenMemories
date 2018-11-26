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

   /* public static DialogManager Instance() {
        if (!modalPanel) {
            modalPanel = FindObjectOfType(typeof(DialogManager)) as DialogManager;
            if (!modalPanel)
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
        }

        return modalPanel;
    }*/

    // Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
    public void Choice(string question, UnityAction DialogEventA, UnityAction DialogEventB, UnityAction DialogEventC, bool lastDialogue, string npcName, int numOfAns) {
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
            if (numOfAns == 3) {
                cButton.gameObject.SetActive(true);
                cButton.GetComponentInChildren<Text>().text = dialogueDB[npcName + "_" + "C"];
            }
        }
        else {
            aButton.gameObject.SetActive(false);
            bButton.gameObject.SetActive(false);
            cButton.gameObject.SetActive(false);
        }
    }
    
    public void ClosePanel() {
        modalPanelObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void InitializeDialog() {
        dialogueDB.Add("SeñoraRamos_1", "Buenas, señora Ramos ¿Qué tal el día? ¿No habrá visto a Lúculo?");
        dialogueDB.Add("SeñoraRamos_2", "No habrás visto tú a mi marido... Desde que se fue ayer contigo no lo he vuelto a ver...");
        dialogueDB.Add("SeñoraRamos_A", "¿Qué marido?");
        dialogueDB.Add("SeñoraRamos_B", "Ayer... No recuerdo nada de lo de ayer");
        dialogueDB.Add("SeñoraRamos_C", "Me dijo que volvería a casa... Yo es que me fui antes");
        dialogueDB.Add("SeñoraRamos_AR", "¿Qué gato?");
        dialogueDB.Add("SeñoraRamos_BR", "Ya...");
        dialogueDB.Add("SeñoraRamos_CR", "Bueno, le preguntaré al resto a ver si alguien sabe algo");

        dialogueDB.Add("Carlos_1", "Carlos... ¡Carlos!");
        dialogueDB.Add("Carlos_2", "Carlos: ¿Qué pasa? Tío, ¿te encuentras bien?");
        dialogueDB.Add("Carlos_A", "Si, como en mi vida");
        dialogueDB.Add("Carlos_B", "La verdad es que...");
        dialogueDB.Add("Carlos_C", "Sinceramente no");
        dialogueDB.Add("Carlos_AR", "Carlos: ¿Al final el zombie no te hizo nada? Me pareció ver... Da igual...");
        dialogueDB.Add("Carlos_BR", "Carlos: Lo sabía, te mordió.");
        dialogueDB.Add("Carlos_CR", "Carlos: ¿Estás herido?");

        dialogueDB.Add("Paco_1", "Anxo: Hola Paco");
        dialogueDB.Add("Paco_2", "Paco: Hombre Anxo, menos mal, que bien te veo. Pensé que no la contabas.");
        dialogueDB.Add("Paco_A", "¿Recuerdas qué pasó ayer?");
        dialogueDB.Add("Paco_B", "No habrás visto a Lúculo...");
        dialogueDB.Add("Paco_AR", "Sí tío, nos rodearon... Una pena lo de Ramos ¿No te acuerdas?");
        dialogueDB.Add("Paco_BR", "Me suena verlo cerca de... Espera, compra la información, ya sabes cómo funciona.");
        dialogueDB.Add("Paco__1", "Anxo: Tengo prisa, Paco.");
        dialogueDB.Add("Paco__2", "Paco: Y yo sed, Anxo.");
        dialogueDB.Add("Paco__3", "Anxo: ¿Qué será esta vez?");
        dialogueDB.Add("Paco__4", "Paco: Un buen vino, Ribeiro estaría bien...");
        dialogueDB.Add("Paco___1", "Anxo: Aquí tienes tu vino, Paco. Ahora cuéntame, ¿has visto a mí gato?");
        dialogueDB.Add("Paco___2", "Paco: Me pareció verlo cerca de los cubos de basura, poniéndolo todo hecho un asco, como siempre");
    }
}
