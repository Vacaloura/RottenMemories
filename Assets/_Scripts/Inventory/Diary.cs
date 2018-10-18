using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : Item {

    public int numberOfPages = 2;
    private List<string> pages = new List<string>(); 

    public Diary() : base("Diario", null, "Diario personal de Anxo. En el relata su día a día incluyendo los últimos acontecimientos", ItemType.Diary)
    {
        //pages.AddRange(); TODO añadir páginas iniciales
    }


    private void AddPage(string page)
    {
        pages.Add(page);
    }

    private string ReadPage(int page)
    {
        return pages[page];
    }
}
