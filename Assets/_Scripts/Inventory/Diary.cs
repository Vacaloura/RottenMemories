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
    }
}
