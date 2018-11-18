using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : Item {

    public int numberOfPages = 7;
    private List<string> diaryPages = new List<string>();
    private List<int> pagesInProperty = new List<int>();

    public Diary() : base("Diario", null, "Diario personal de Anxo. En el relata su día a día incluyendo los últimos acontecimientos", ItemType.Diary)
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
        diaryPages.Add("Diario de Anxo.");
        diaryPages.Add("Qué dolor de cabeza...En mi vida me he encontrado tan mal, ni cuando cogí el sarampión a los quince años y mira que casi me muero.\n" +
            "Pero...por qué estoy así, no recuerdo nada.Me golpee la cabeza ? Se me fue la mano con el whisky ?\n" +
            "Me arde el costado, tengo una herida muy fea, parece un mordisco.\n" +
            "Señor, por qué a mí ?\n\n" +
            "No estoy tan mal.Igual demasiadas ojeras y la piel un poco púrpura...pero por lo demás fresco como una lechuga.\n" +
            "Tengo hambre. ¿Ayer le di de comer a Lúculo ? No me acuerdo.\n" +
            "¿Dónde está el gato ?\n" +
            "Me habrá olido y habrá corrido, yo también lo haría. Debería ir a buscarlo, pero como salga así a la calle el primer vecino que me vea me machacará la cabeza... Debería pensar en algo.\n"
            );

        diaryPages.Add("Necesitamos víveres, estoy de comer judías con tomate hasta el gorro. Un grupo de vecinos armados y yo nos hemos adentrado al bosque para cazar algunas piezas.\n" +
            "Lo siento por los pobres animalitos...pero quiero carne.\n");

        diaryPages.Add("Estamos rodeados, a quien se le ocurre llevar el móvil con sonido a una expedición. Modo avión, modo avión!\n" +
            "Está oscuro, parece que cuando pasan estas cosas la noche se cierne con más velocidad.Escucho las voces de mis compañeros.\n" +
            "Carlos se ha derrumbado, Jaime ha huido y Ramos no da apagado el teléfono.\n" +
            "Huelo la carne pútrida, cada vez están más cerca, debemos reagruparmos.\n" +
            "Ramos ha caído.Le mordieron en el cuello.Cuánta sangre.Tuvimos que tomar una decisión.Yo no quería.Pobre señora Ramos.\n");

        diaryPages.Add("Voy a salir a explorar el bosque con Lúculo. Espero que se porte bien. \n");
    }
}
