using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStrings : MonoBehaviour
{

    [HideInInspector] public static GameStrings gameStringsInstance;
    public string selectedLanguage = "Español";

    private void Awake()
    {
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

    public string GetString(string petition, string additionalMessage)
    {
        switch (selectedLanguage)
        {
            default:

            case "Español":
                switch (petition)
                {
                    //Victoria y derrota
                    case "PlayerDeath":
                        return "GAME OVER:\n" + additionalMessage + "\nPulse ESC para salir o RETROCESO para volver al último checkpoint.";
                    case "PlayerDeathByMadness":
                        return "Tu locura ha aumentado demasiado. Te has transformado en zombie.";
                    case "PlayerDeathDiscovered":
                        return "¡Te han descubierto!";
                    case "PlayerWin":
                        return "YOU WIN!: " + additionalMessage + " \nCargando siguiente nivel...";
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
                        return "Lo sabía, te mordió";
                    case "CarlosR_C":
                        return "¿Estás herido?";
                    case "CarlosA_B":
                        return "Que va, simplemente tengo mal cuerpo. Lo de ayer fue muy intenso";
                    case "CarlosA_C":
                        return "Me torcí el tobillo y me arde horrores";
                    case "CarlosN_3":
                        return "¿Has visto a Jaime?";
                    case "CarlosP_4":
                        return "Todavía no me he cruzado con él";
                    case "CarlosI_1":
                        return "Que tranquilo está todo hoy";
                    case "CarlosI_2":
                        return "Uy... Espero que no llueva";
                    case "PacoP_1":
                        return "Hola Paco";
                    case "PacoN_2":
                        return "Hombre Anxo, menos mal, qué bien te veo. Pensé que no la contabas";
                    case "PacoB_A":
                        return "¿Recuerdas qué pasó ayer?";
                    case "PacoB_B":
                        return "No habrás visto a Lúculo...";
                    case "PacoR_A":
                        return "Sí tío, nos rodearon... Una pena lo de Ramos ¿No te acuerdas?";
                    case "PacoR_B":
                        return "Me suena verlo cerca de... Espera, compra la información, ya sabes cómo funciona";
                    case "PacoP_3":
                        return "Tengo prisa, Paco";
                    case "PacoN_4":
                        return "Y yo sed, Anxo";
                    case "PacoP_5":
                        return "¿Qué será esta vez?";
                    case "PacoN_6":
                        return "Un buen vino, Ribeiro estaría bien...";
                    case "PacoI_1":
                        return "Tengo mucha sed";
                    case "PacoI_2":
                        return "No veo aquí mi vino";
                    case "Paco_2P_1":
                        return "Aquí tienes tu vino, Paco. Ahora cuéntame, ¿has visto a mi gato?";
                    case "Paco_2N_2":
                        return "Me pareció verlo cerca de los cubos de basura, poniéndolo todo hecho un asco, como siempre";
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
                        "El móvil del viejo empezó a sonar y un zombie corrió hacia él. Nunca los había visto así, la verdad. Tú corriste también y te pusiste entre el zombie y el viejo";
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
                        return "Ya... Sí, lo vi hace no mucho cazando mariposas en la linde por el bosque. Encuéntralo antes que los zombies...";
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
                        return "Diario personal de Anxo. En él relata su día a día incluyendo los últimos acontecimientos.";
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
                        return "Botella de vino que alguien perdió mientras escapaba. Casualmente es un Ribeira.";
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

                    case "Find":
                        return "¡Has encontrado " + additionalMessage + "!";
                    case "Thrown":
                        return "Has tirado " + additionalMessage + " D:";
                    case "Consumed":
                        return "Has consumido " + additionalMessage + " :D";

                    //Otros
                    case "LockedDoor":
                        return "La curiosidad mató al gato... Debo encontrar a Lúculo.";
                    case "MakedUp":
                        return "Ya estoy guapo, al lío.";
                    case "CantPick":
                        return "Tengo que ver como llego hasta ahí arriba.";
                    case "FoodRamos":
                        return "Sin comida hay quien se vuelve loco...";
                    case "BadLooking":
                        return "Tienes muy mal aspecto... ¡A ti te han mordido!\nLástima. Me caías bien. Cuidaré de tu gato. Ahora muere.";
                    case "ShouldNotTalk":
                        return "No creo que deba hablarle con este aspecto. Necesito pasar desapercibido";
                    case "Interact":
                        return "Pulsa E para interactuar";
                    case "Skip":
                        return "Pulsa E para saltar";
                    default:
                        return "NULL";

                }

            case "Galego":
                switch (petition)
                {
                    //Victoria y derrota
                    case "PlayerDeath":
                        return "GAME OVER:\n" + additionalMessage + "\nPulse ESC para saír ou RETROCESO pra volver ó último checkpoint.";
                    case "PlayerDeathByMadness":
                        return "A túa locura aumentou demasiado. Transformácheste en zombie.";
                    case "PlayerDeathDiscovered":
                        return "Descubríronche!";
                    case "PlayerWin":
                        return "YOU WIN!: " + additionalMessage + " \nCargando seguinte nivel...";
                    case "CatEaten":
                        return "Comícheste ó teu gato. Eres un monstro sen sentimentos.";
                    case "CatRescued":
                        return "Atopaches a Lúculo e leváchelo á casa a salvo.";

                    //Dialogos
                    case "RamosP_1":
                        return "Boas, señora Ramos. Qué tal o día? Non vería vostede a Lúculo?";
                    case "RamosN_2":
                        return "Non verías ti ao meu marido... Dende que se foi onte contigo non o volvín ver...";
                    case "RamosB_A":
                        return "Qué marido?";
                    case "RamosB_B":
                        return "Onte... Non recordo nada do de onte";
                    case "RamosB_C":
                        return "Díxome que volvería a casa... Eu é que funme antes";
                    case "RamosR_A":
                        return "Qué gato?";
                    case "RamosR_B":
                        return "Xa...";
                    case "RamosR_C":
                        return "Bueno, preguntareille ao resto a ver se alguén sabe algo";
                    case "RamosI_1":
                        return "Eu de ti collería algo de comida antes de saír...";
                    case "RamosI_2":
                        return "Cariño... cariño...";
                    case "CarlosP_1":
                        return "Carlos... Carlos!";
                    case "CarlosN_2":
                        return "Qué pasa? Tío, atópaste ben?";
                    case "CarlosB_A":
                        return "Si, como na miña vida";
                    case "CarlosB_B":
                        return "A verdade é que...";
                    case "CarlosB_C":
                        return "Sinceramente non";
                    case "CarlosR_A":
                        return "Ao final o zombie non che fixo nada? Pareceume ver... Da igual...";
                    case "CarlosR_B":
                        return "Sabíao, mordeuche";
                    case "CarlosR_C":
                        return "Estás ferido?";
                    case "CarlosA_B":
                        return "Que va, simplemente teño mal corpo. O de onte foi moi intenso";
                    case "CarlosA_C":
                        return "Torcinme o tobillo e árdeme horrores";
                    case "CarlosN_3":
                        return "Viches a Jaime?";
                    case "CarlosP_4":
                        return "Todavía non me crucei con el";
                    case "CarlosI_1":
                        return "Que tranquilo está todo hoxe";
                    case "CarlosI_2":
                        return "Ui... Espero que non chova";
                    case "PacoP_1":
                        return "Ola Paco";
                    case "PacoN_2":
                        return "Home Anxo, menos mal, qué ben te vexo. Pensei que non a contabas";
                    case "PacoB_A":
                        return "Recordas qué pasou onte?";
                    case "PacoB_B":
                        return "Non haberás visto a Lúculo...";
                    case "PacoR_A":
                        return "Sí tío, rodeáronnos... Unha pena o de Ramos. Non te lembras?";
                    case "PacoR_B":
                        return "Sóame velo preto de... Agarda, compra a información, xa sabes cómo furrula";
                    case "PacoP_3":
                        return "Teño présa, Paco";
                    case "PacoN_4":
                        return "E eu sede, Anxo";
                    case "PacoP_5":
                        return "Qué será desta vez?";
                    case "PacoN_6":
                        return "Un bo viño, Ribeiro estaría ben...";
                    case "PacoI_1":
                        return "Teño moita sede";
                    case "PacoI_2":
                        return "Non vexo aquí o meu viño";
                    case "Paco_2P_1":
                        return "Aquí tes o teu viño, Paco. Agora cóntame, viches ao meu gato?";
                    case "Paco_2N_2":
                        return "Pareceume velo preto dos cubos de lixo, poñendo todo noxento, coma sempre";
                    case "Paco_2I_1":
                        return "Glu glu glu...";
                    case "Paco_2I_2":
                        return "Que boa colleita...";
                    case "JaimeN_1":
                        return "Anxo! Dichósolos ollos...";
                    case "JaimeP_2":
                        return "Canalla... Deixáchesnos!";
                    case "JaimeN_3":
                        return "Non me xuzgues, era un sálvese quen poida. De todas formas non me fun moi lonxe por se precisabades axuda";
                    case "JaimeP_4":
                        return "Como a que precisara o pobre señor Ramos?";
                    case "JaimeN_5":
                        return "Que querías, que fixera coma ti e me interpuxera? Poderíamos ter morto os dous. De feito, é unha miragre que ti sigas con vida";
                    case "JaimeB_A":
                        return "Eu interpúxenme?";
                    case "JaimeB_B":
                        return "Viches a pelexa?";
                    case "JaimeB_C":
                        return "Alguén máis estivo presente?";
                    case "JaimeR_A":
                        return "Qué pasa, non te lembras ou queres que conte a túa hazaña coma unha epopeya, paspán? " +
                        "O móbil do vello comezou a sonar e un zombie correu canda el. Nunca os vira así, a verdade. Ti corriches tamén e puxécheste entre o zombie e o vello";
                    case "JaimeR_B":
                        return "A ver, estaba alonxado e detrás dunhas árbores. Vin a grandes rasgos como te liabas a golpes co pobre desgraciado... aída que el tamén repartía que daba gusto";
                    case "JaimeR_C":
                        return "Tsk... Que moito falas de min pero o resto liscaron coma ratas... Que eu saiba no había ninguén máis. Queres que o vaia contando por aí?";
                    case "JaimeN_6":
                        return "Así que foi eso...";
                    case "JaimeN_7":
                        return "Si, e non agardedes que volva a unha expedición convosco. Eu paso";
                    case "JaimeP_8":
                        return "Tranquilo, eu tamén. Bueno...";
                    case "JaimeN_9":
                        return "Por certo, o gato ese regalado e obeso é teu, verdade?";
                    case "JaimeP_10":
                        return "Lúculo! O gato non está gordo, ten moito pelo. Víchelo?";
                    case "JaimeN_11":
                        return "Xa... Si, vírao fai non moito cazando bolboretas na linde polo bosque. Atópao antes ca os zombies...";
                    case "JaimeP_12":
                        return "Grazas!";
                    case "JaimeI_1":
                        return "Prefiro que me deixes tranquilo";
                    case "JaimeI_2":
                        return "Non teño nada máis que falar contigo";

                    //Inventario
                    case "NonRemovable":
                        return "Creo que non debería tirar esto.";
                    case "DiaryDescription":
                        return "Diario persoal de Anxo. Nel relata o seu día a día incluíndo os últimos acontecemientos.";
                    case "HarpoonDescription":
                        return "Arpón usado pra cazar pequenos peces en expedicións de submarinismo.";
                    case "EmptyMunition":
                        return "Quedaches sen virotes!";
                    case "HarpoonName":
                        return "Arpón";
                    case "DiaryName":
                        return "Diario";
                    case "FoodName":
                        return "Fiambreira";
                    case "FoodDescription":
                        return "Comida que te axudará a mantenerte cordo.";
                    case "MakeUpName":
                        return "Maquillaxe";
                    case "MakeUpDescription":
                        return "Maquillaxe que algún veciño deixou abandonada na fuxida.";
                    case "WineName":
                        return "Botella de viño";
                    case "WineDescription":
                        return "Botella de viño que alguén perdeu mentres fuxía. Casualmente é un Ribeira.";
                    case "LadderName":
                        return "Escaleira";
                    case "LadderDescription":
                        return "Unha escaleira. É útil pra alcanzar sitios elevados.";
                    case "CatName":
                        return "Lúculo";
                    case "CatDescription":
                        return "A túa única compañía dende a morte da túa muller.";

                    case "Virotes":
                        return " virotes)\n";
                    case "Unidades":
                        return " unidades)\n";

                    case "Find":
                        return "Atopaches " + additionalMessage + "!";
                    case "Thrown":
                        return "Ciscaches " + additionalMessage + " D:";
                    case "Consumed":
                        return "Consumiches " + additionalMessage + " :D";

                    //Otros
                    case "LockedDoor":
                        return "A curiosidade matou ao gato... Debo atopar a Lúculo.";
                    case "MakedUp":
                        return "Xa estou guapo, ao lío.";
                    case "CantPick":
                        return "Teño que ver como chego ata aí arriba.";
                    case "FoodRamos":
                        return "Sen comida hai quen se volve tolo...";
                    case "BadLooking":
                        return "Tes moi mal aspecto... A ti mordéronche!\nLástima. Caíasme ben. Coidarei do gato. Agora morre.";
                    case "ShouldNotTalk":
                        return "Non creo que deba falarlle con este aspecto. Preciso pasar desapercibido";
                    case "Interact":
                        return "Preme E pra interactuar";
                    case "Skip":
                        return "Preme E pra saltar";
                    default:
                        return "NULL";

                }
            case "English":
                return "Not ready";
        }

    }

}
