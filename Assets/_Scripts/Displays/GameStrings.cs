using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStrings : MonoBehaviour {

    [HideInInspector] public static GameStrings gameStringsInstance;
    public string selectedLanguage = "Español";

    private void Awake() {
        if (gameStringsInstance == null)
            gameStringsInstance = this;
        else Debug.LogError("Tried to create a second GameStrings");

        try
        {
            selectedLanguage = GameController.gameControllerInstance.selectedLanguage;
        }
        catch (Exception e)
        {
            Debug.Log("No hay GameController - " + e.ToString());
        }
    }

    
    private void Start()
    {
        
    }

    public string GetString(string petition, string additionalMessage) {
        switch (selectedLanguage) {
            default: 

            case "Español":
                switch (petition) {
                    //Victoria y derrota
                    case "PlayerDeath":
                        return "GAME OVER:\n" + additionalMessage + "\nPulse ESC para salir o R para volver al último checkpoint.";
                    case "PlayerDeathByMadness":
                        return "Tu locura ha aumentado demasiado.Te has transformado en zombie.";
                    case "PlayerDeathDiscovered":
                        return "Te han descubierto!";
                    case "PlayerWin":
                        return "YOU WIN!: " + additionalMessage + " \nPulse ESC para salir o R para volver al último checkpoint.";
                    case "CatEaten":
                        return "Te has comido a tu gato. Eres un monstruo sin sentimientos.";
                    case "CatRescued":
                        return "Has encontrado a Lúculo y lo has llevado a casa a salvo.";

                    //Dialogos
                    case "RamosP_1":
                        return "Buenas, señora Ramos ¿Qué tal el día? ¿No habrá visto a Lúculo?";
                    case "RamosN_2":
                        return "No habrás visto tú a mi marido... Desde que se fue ayer contigo no lo he vuelto a ver...";
                    case "RamosB_A":
                        return "¿Qué marido?";
                    case "RamosB_B":
                        return "Ayer... No recuerdo nada de lo de ayer";
                    case "RamosB_C":
                        return "Me dijo que volvería a casa... Yo es que me fui antes";
                    case "RamosR_A":
                        return "¿Qué gato?";
                    case "RamosR_B":
                        return "Ya...";
                    case "RamosR_C":
                        return "Bueno, le preguntaré al resto a ver si alguien sabe algo";
                    case "RamosI_1":
                        return "Yo de ti cogería algo de comida antes de salir...";
                    case "RamosI_2":
                        return "Cariño... cariño...";
                    case "CarlosP_1":
                        return "Carlos... ¡Carlos!";
                    case "CarlosN_2":
                        return "¿Qué pasa? Tío, ¿te encuentras bien?";
                    case "CarlosB_A":
                        return "Si, como en mi vida";
                    case "CarlosB_B":
                        return "La verdad es que...";
                    case "CarlosB_C":
                        return "Sinceramente no";
                    case "CarlosR_A":
                        return "¿Al final el zombie no te hizo nada? Me pareció ver... Da igual...";
                    case "CarlosR_B":
                        return "Lo sabía, te mordió.";
                    case "CarlosR_C":
                        return "¿Estás herido?";
                    case "CarlosA_B":
                        return "Que va, simplemente tengo mal cuerpo, Lo de ayer fue muy intenso";
                    case "CarlosA_C":
                        return "Me torcí el tobillo y me arde horrores";
                    case "CarlosN_3":
                        return "Has visto a Jaime?";
                    case "CarlosP_4":
                        return "Todavía no me he cruzado con él";
                    case "CarlosI_1":
                        return "Qué tranquilo está todo hoy";
                    case "CarlosI_2":
                        return "Uy... Espero que no llueva";
                    case "PacoP_1":
                        return "Hola Paco";
                    case "PacoN_2":
                        return "Hombre Anxo, menos mal, que bien te veo. Pensé que no la contabas.";
                    case "PacoB_A":
                        return "¿Recuerdas qué pasó ayer?";
                    case "PacoB_B":
                        return "No habrás visto a Lúculo...";
                    case "PacoR_A":
                        return "Sí tío, nos rodearon... Una pena lo de Ramos ¿No te acuerdas?";
                    case "PacoR_B":
                        return "Me suena verlo cerca de... Espera, compra la información, ya sabes cómo funciona.";
                    case "PacoP_3":
                        return "Tengo prisa, Paco.";
                    case "PacoN_4":
                        return "Y yo sed, Anxo.";
                    case "PacoP_5":
                        return "¿Qué será esta vez?";
                    case "PacoN_6":
                        return "Un buen vino, Ribeiro estaría bien...";
                    case "PacoI_1":
                        return "Tengo mucha sed.";
                    case "PacoI_2":
                        return "No veo aquí mi vino";
                    case "Paco_2P_1":
                        return "Aquí tienes tu vino, Paco. Ahora cuéntame, ¿has visto a mi gato?";
                    case "Paco_2N_2":
                        return "Paco: Me pareció verlo cerca de los cubos de basura, poniéndolo todo hecho un asco, como siempre";
                    case "Paco_2I_1":
                        return "Glu glu glu...";
                    case "Paco_2I_2":
                        return "Que buena cosecha...";
                    case "JaimeN_1":
                        return "¡Anxo! Dichosos los ojos...";
                    case "JaimeP_2":
                        return "Canalla... ¡Nos abandonaste!";
                    case "JaimeN_3":
                        return "No me juzgues, era un sálvese quien pueda. De todas formas no me fui muy lejos por si necesitábais ayuda";
                    case "JaimeP_4":
                        return "¿Como la que necesitó el pobre Señor Ramos?";
                    case "JaimeN_5":
                        return "¿Qué querías, que hiciera como tú y me interpusiera? Podríamos haber muerto los dos. De hecho, es un milagro que tú sigas con vida";
                    case "JaimeB_A":
                        return "¿Yo me interpuse?";
                    case "JaimeB_B":
                        return "¿Viste la pelea?";
                    case "JaimeB_C":
                        return "¿Alguien más estuvo presente?";
                    case "JaimeR_A":
                        return "Qué pasa, ¿no te acuerdas o quieres que cuente tu hazaña como una epopeya, payaso? " +
                        "El móvil del viejo empezó a sonar y un zombie corrió hacia él. Nunca los había visto así, la verdad. Tú corriste también y te pusiste entre el zombie y el viejo.";
                    case "JaimeR_B":
                        return "A ver, estaba alejado y detrás de unos árboles. Vi a grandes rasgos como te liaste a golpes con el pobre desgraciado... aunque él también repartía que daba gusto";
                    case "JaimeR_C":
                        return "Tsk... Que mucho hablas de mí pero el resto huyeron como ratas... Que yo sepa no había nadie más ¿Quieres que lo vaya contando por ahí?";
                    case "JaimeN_6":
                        return "Así que fue eso...";
                    case "JaimeN_7":
                        return "Sí, y no espereis que vuelva a una expedición con vosotros. Yo paso";
                    case "JaimeP_8":
                        return "Tranquilo, yo también. Bueno...";
                    case "JaimeN_9":
                        return "Por cierto, el gato ese regalado y obeso es tuyo, ¿verdad?";
                    case "JaimeP_10":
                        return "¡Lúculo! Mi gato no está gordo, tiene mucho pelo ¿Lo has visto?";
                    case "JaimeN_11":
                        return "Ya... Sí, lo vi hace no mucho cazando mariposas en la linde con el bosque. Encuéntralo antes que los zombies...";
                    case "JaimeP_12":
                        return "¡Gracias!";
                    case "JaimeI_1":
                        return "Prefiero que me dejes tranquilo";
                    case "JaimeI_2":
                        return "No tengo nada más que hablar contigo";

                    //Inventario
                    case "NonRemovable":
                        return "Creo que no debería tirar esto.";
                    case "DiaryDescription":
                        return "Diario personal de Anxo. En él relata su día a día incluyendo los últimos acontecimientos";
                    case "HarpoonDescription":
                        return "Arpón usado para cazar pequeños peces en expediciones de submarinismo.";
                    case "EmptyMunition":
                        return "¡Te has quedado sin virotes!";
                    case "HarpoonName":
                        return "Arpón";
                    case "DiaryName":
                        return "Diario";
                    case "FoodName":
                        return "Fiambrera";
                    case "FoodDescription":
                        return "Comida que te ayudará a mantenerte cuerdo.";
                    case "MakeUpName":
                        return "Maquillaje";                    
                    case "MakeUpDescription":
                        return "Maquillaje que algún vecino dejó abandonado en la huída.";
                    case "WineName":
                        return "Botella de vino";
                    case "WineDescription":
                        return "Botella de vino que alguien perdió mientras escapaba. CASUALMENTE es un Ribeira.";
                    case "LadderName":
                        return "Escalera";
                    case "LadderDescription":
                        return "Una escalera. Es útil para alcanzar sitios elevados.";
                    case "CatName":
                        return "Lúculo";
                    case "CatDescription":
                        return "Tu única compañía desde que murió tu esposa.";

                    case "Virotes":
                        return " virotes)\n";
                    case "Unidades":
                        return " unidades)\n";

                    //Otros
                    case "LockedDoor":
                        return "La curiosidad mató al gato... Debo encontrar a Lúculo";
                    case "MakedUp":
                        return "Ya estoy guapo, al lío.";
                    case "CantPick":
                        return "Tengo que ver como llego hasta ahí arriba.";
                    case "FoodRamos":
                        return "Sin comida hay quien se vuelve loco...";
                    case "BadLooking":
                        return "Tienes muy mal aspecto... a ti te han mordido!\nLástima. Me caías bien. Cuidaré de tu gato. Ahora muere.";
                    default:
                        return "NULL";

                }

            case "Galego":
                return "Non dispoñible";

            case "English":
                return "Not ready";
        }

    }
   
}
