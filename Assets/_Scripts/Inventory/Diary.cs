using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Diary : Item {

    public int numberOfPages = 7;
    private List<string> diaryPages = new List<string>();
    private List<int> pagesInProperty = new List<int>();

    public Diary() : base(GameStrings.gameStringsInstance.GetString("DiaryName", null), "Icono_Diario", GameStrings.gameStringsInstance.GetString("DiaryDescription", null), ItemType.Diary)
    {
        InitializeDiary();
        pagesInProperty.Add(0);
    }

    public void AddPage(int page)
    {
        pagesInProperty.Add(page);
        pagesInProperty.Sort();
    }

    public string ReadPage(int page)
    {
        return diaryPages[pagesInProperty[page]];
    }

    public bool hasNext(int actualPage)
    {
        if (pagesInProperty.Count > actualPage+1) return true;
        else return false;
    }

    public bool hasPrevious(int actualPage)
    {
        if (actualPage == 0) return false;
        else return true;
    }

    public void InitializeDiary()
     {
        switch (GameStrings.gameStringsInstance.selectedLanguage)
        {
            default:

            case "Español":
                diaryPages.Add("Nota0");
                diaryPages.Add("Nota1");
                diaryPages.Add("Nota2");
                diaryPages.Add("Nota3");
                diaryPages.Add("Nota4");
                break;
            case "English":
                diaryPages.Add("Note0");
                diaryPages.Add("Note1");
                diaryPages.Add("Note2");
                diaryPages.Add("Note3");
                diaryPages.Add("Note4");
                break;
        }



        /*diaryPages.Add("Diario de Anxo");
        diaryPages.Add("Qué dolor de cabeza... En mi vida me he encontrado tan mal, ni cuando cogí el sarampión a los quince años y mira que casi me muero.\n" +
            "Pero... ¿Por qué estoy así? No recuerdo nada ¿Me golpeé la cabeza? Se me fue la mano con el whisky ?\n" +
            "Me arde el costado, tengo una herida muy fea, parece un mordisco.\n" +
            "Señor, ¿por qué a mí?\n\n" +
            "No estoy tan mal. Igual demasiadas ojeras y la piel un poco púrpura... Pero por lo demás fresco como una lechuga.\n" +
            "Tengo hambre ¿Ayer le di de comer a Lúculo? No me acuerdo.\n" +
            "¿Dónde está el gato?\n" +
            "Me habrá olido y habrá corrido, yo también lo haría. Debería ir a buscarlo, pero como salga así a la calle el primer vecino que me vea me machacará la cabeza... Debería pensar en algo.\n"
            );

        diaryPages.Add("Necesitamos víveres, estoy de comer judías con tomate hasta el gorro. Un grupo de vecinos armados y yo nos hemos adentrado al bosque para cazar algunas piezas.\n" +
            "Lo siento por los pobres animalitos... pero quiero carne.\n");

        diaryPages.Add("Estamos rodeados ¿A quién se le ocurre llevar el móvil con sonido a una expedición? ¡Modo avión, modo avión!\n" +
            "Está oscuro. Parece que cuando pasan estas cosas la noche se cierne con más velocidad. Escucho las voces de mis compañeros.\n" +
            "Carlos se ha derrumbado, Jaime ha huido y Ramos no da apagado el teléfono.\n" +
            "Huelo la carne pútrida, cada vez están más cerca. Debemos reagruparmos.\n" +
            "Ramos ha caído. Le mordieron en el cuello. Cuánta sangre. Tuvimos que tomar una decisión. Yo no quería. Pobre señora Ramos.\n");

        diaryPages.Add("Si es que para qué me hago el héroe si ni siquiera me importa si ese viejo vive o muere.\n" +
            "Ahora estoy jodido ¿Cuántas horas me quedarán de vida? Quizás ni tan siquiera sean horas...\n" +
            "Esta herida no para de sangrar... Noto la visión muy borrosa y estoy cansado;\n" +
            "lucho por llegar a casa, lo único que quiero es ver a Lúculo y dejarle la comida cerca para cuando yo no pueda dársela.\n" +
            "Pobre granuja. Espero que sepa cuidarse solo y que no se coma todo el pienso de golpe.\n" +
            "¿Lúculo? Lúculo, soy yo, papá. No voy a hacerte daño.\n" +
            "Ha huído. En cuanto me ha olido no se lo ha pensado dos veces. Pequeño traidor, no puedo juzgarle, yo tampoco me quedaría.\n" +
            "Así que por eso no estaba en casa esta mañana. Pobrecito, que miedo habrá pasado.");

        diaryPages.Add("Voy a salir a explorar el bosque con Lúculo. Espero que se porte bien. \n");*/
    }
}
