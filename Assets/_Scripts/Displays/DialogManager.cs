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
    [HideInInspector] public static Dictionary<string, string> SeñoraRamos = new Dictionary<string, string>();
    [HideInInspector] public static Dictionary<string, string> Carlos = new Dictionary<string, string>();
    [HideInInspector] public static Dictionary<string, string> Paco = new Dictionary<string, string>();
    [HideInInspector] public static Dictionary<string, string> Paco_2 = new Dictionary<string, string>();
    [HideInInspector] public static Dictionary<string, string> Jaime = new Dictionary<string, string>();





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


    public void DisplayPhrase(string phrase)
    {
        aButton.gameObject.SetActive(false);
        bButton.gameObject.SetActive(false);
        cButton.gameObject.SetActive(false);
        modalPanelObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        modalPanelObject.transform.SetAsLastSibling();
        this.question.text = phrase;
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

        SeñoraRamos.Add("P_1", "Buenas, señora Ramos ¿Qué tal el día? ¿No habrá visto a Lúculo?");
        SeñoraRamos.Add("P_2", "No habrás visto tú a mi marido... Desde que se fue ayer contigo no lo he vuelto a ver...");
        SeñoraRamos.Add("B_A", "¿Qué marido?");
        SeñoraRamos.Add("B_B", "Ayer... No recuerdo nada de lo de ayer");
        SeñoraRamos.Add("B_C", "Me dijo que volvería a casa... Yo es que me fui antes");
        SeñoraRamos.Add("R_A", "¿Qué gato?");
        SeñoraRamos.Add("R_B", "Ya...");
        SeñoraRamos.Add("R_C", "Bueno, le preguntaré al resto a ver si alguien sabe algo");
        SeñoraRamos.Add("I_1", "Yo de ti cogería algo de comida antes de salir...");
        SeñoraRamos.Add("I_2", "Cariño... cariño...");

        Carlos.Add("P_1", "Carlos... ¡Carlos!");
        Carlos.Add("P_2", "¿Qué pasa? Tío, ¿te encuentras bien?");
        Carlos.Add("B_A", "Si, como en mi vida");
        Carlos.Add("B_B", "La verdad es que...");
        Carlos.Add("B_C", "Sinceramente no");
        Carlos.Add("R_A", "¿Al final el zombie no te hizo nada? Me pareció ver... Da igual...");
        Carlos.Add("R_B", "Lo sabía, te mordió.");
        Carlos.Add("R_C", "¿Estás herido?");
        Carlos.Add("A_B", "Que va, simplemente tengo mal cuerpo, Lo de ayer fue muy intenso");
        Carlos.Add("A_C", "Me torcí el tobillo y me arde horrores");
        Carlos.Add("P_3", "Has visto a Jaime?");
        Carlos.Add("P_4", "Todavía no me he cruzado con él");
        Carlos.Add("I_1", "Qué tranquilo está todo hoy");
        Carlos.Add("I_2", "Uy... Espero que no llueva");

        Paco.Add("P_1", "Hola Paco");
        Paco.Add("P_2", "Hombre Anxo, menos mal, que bien te veo. Pensé que no la contabas.");
        Paco.Add("B_A", "¿Recuerdas qué pasó ayer?");
        Paco.Add("B_B", "No habrás visto a Lúculo...");
        Paco.Add("R_A", "Sí tío, nos rodearon... Una pena lo de Ramos ¿No te acuerdas?");
        Paco.Add("R_B", "Me suena verlo cerca de... Espera, compra la información, ya sabes cómo funciona.");
        Paco.Add("P_3", "Tengo prisa, Paco.");
        Paco.Add("P_4", "Y yo sed, Anxo.");
        Paco.Add("P_5", "¿Qué será esta vez?");
        Paco.Add("P_6", "Un buen vino, Ribeiro estaría bien...");
        Paco.Add("I_1", "Tengo mucha sed.");
        Paco.Add("I_2", "No veo aquí mi vino");

        Paco_2.Add("P_1", "Anxo: Aquí tienes tu vino, Paco. Ahora cuéntame, ¿has visto a mí gato?");
        Paco_2.Add("P_2", "Paco: Me pareció verlo cerca de los cubos de basura, poniéndolo todo hecho un asco, como siempre");
        Paco_2.Add("I_1", "Glu glu glu...");
        Paco_2.Add("I_2", "Que buena cosecha...");

        Jaime.Add("P_1", "¡Anxo! Dichosos los ojos...");
        Jaime.Add("P_2", "Canalla... ¡Nos abandonaste!");
        Jaime.Add("P_3", "No me juzgues, era un sálvese quien pueda. De todas formas no me fui muy lejos por si necesitábais ayuda");
        Jaime.Add("P_4", "¿Como la que necesitó el pobre Señor Ramos?");
        Jaime.Add("P_5", "¿Qué querías, que hiciera como tú y me interpusiera? Podríamos haber muerto los dos. De hecho, es un milagro que tú sigas con vida");
        Jaime.Add("B_A", "¿Yo me interpuse?");
        Jaime.Add("B_B", "¿Viste la pelea?");
        Jaime.Add("B_C", "¿Alguien más estuvo presente?");
        Jaime.Add("R_A", "Qué pasa, ¿no te acuerdas o quieres que cuente tu hazaña como una epopeya, payaso? " +
            "El móvil del viejo empezó a sonar y un zombie corrió hacia él. Nunca los había visto así, la verdad. Tú corriste también y te pusiste entre el zombie y el viejo.");
        Jaime.Add("R_B", "A ver, estaba alejado y detrás de unos árboles. Vi a grandes rasgos como te liaste a golpes con el pobre desgraciado... aunque él también repartía que daba gusto");
        Jaime.Add("R_C", "Tsk... Que mucho hablas de mí pero el resto huyeron como ratas... Que yo sepa no había nadie más ¿Quieres que lo vaya contando por ahí?");
        Jaime.Add("P_6", "Así que fue eso...");
        Jaime.Add("P_7", "Sí, y no espereis que vuelva a una expedición con vosotros. Yo paso");
        Jaime.Add("P_8", "Tranquilo, yo también. Bueno...");
        Jaime.Add("P_9", "Por cierto, el gato ese regalado y obeso es tuyo, ¿verdad?");
        Jaime.Add("P_10", "¡Lúculo! Mi gato no está gordo, tiene mucho pelo ¿Lo has visto?");
        Jaime.Add("P_11", "Ya... Sí, lo vi hace no mucho cazando mariposas en la linde con el bosque. Encuéntralo antes que los zombies...");
        Jaime.Add("P_12", "¡Gracias!");
        Jaime.Add("I_1", "Prefiero que me dejes tranquilo");
        Jaime.Add("I_2", "No tengo nada más que hablar contigo");



















    }
}
