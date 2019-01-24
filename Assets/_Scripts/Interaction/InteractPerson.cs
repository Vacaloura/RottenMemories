using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;


public class InteractPerson : Interactable
{
    private UnityAction DialogActionA;
    private UnityAction DialogActionB;
    private UnityAction DialogActionC;
    private Dictionary<string, string> dictionary;
    private List<string> options = new List<string>();
    private int index = 0, nOptions=0;
    private bool wait = false, button=false, pacoFlag=true;
    public bool alreadyInteracted = false;
    private bool alreadyAnswered = false, firstR = true, SRFirstTime = true;
    private char res = 'A';
    public string hola = "Hola";

    public override void Start()
    {
        base.Start();
        
        try
        {
            dictionary = (Dictionary<string, string>)DialogManager.modalPanel.GetType().GetField(gameObject.name).GetValue(this);
        }catch(Exception e) { Debug.Log(e.ToString()); }
    }

    public override void Interact()
    {
        base.Interact();
        if (gameObject.name == "Paco" && PlayerController.playerControllerInstance.hasWine && pacoFlag)
        {
            int slot = 0;
            foreach (Item item in Inventory.inventoryInstance.itemList) {
                if (item.itemType == Item.ItemType.WineBottle) {
                    Inventory.inventoryInstance.RemoveItem(slot);
                    break;
                }
                slot++;
            }
            Animator anim = this.GetComponentInChildren<Animator>();
            //Debug.Log("Anim: " + anim.name);
            anim.SetTrigger("DrinkWine");
            anim.ResetTrigger("IsGreeting");

            dictionary = (Dictionary<string, string>)DialogManager.modalPanel.GetType().GetField("Paco_2").GetValue(this);
            alreadyInteracted = false; index = 0; pacoFlag = false;
        }

        if (!PlayerController.playerControllerInstance.isMadeUp)
        {
            StartCoroutine("Discovered");
            return;
        }
        if (alreadyInteracted)
        {
            if (gameObject.name != "SeñoraRamos")
                DisplayManager.displayManagerInstance.DisplayMessage(dictionary["I_1"], 2.0f);
            else {
                if (PlayerController.playerControllerInstance.hasFood)
                    if (SRFirstTime) {
                        this.transform.Translate(1.5f, 0, 0);
                        this.transform.Rotate(0, 180, 0);
                        DisplayManager.displayManagerInstance.DisplayMessage(GameStrings.gameStringsInstance.GetString("FoodRamos", null), 2.0f);
                        SRFirstTime = false;
                        StartCoroutine(PlayerController.playerControllerInstance.IncreaseByTime());
                    } else DisplayManager.displayManagerInstance.DisplayMessage(dictionary["I_2"], 2.0f);
                else DisplayManager.displayManagerInstance.DisplayMessage(dictionary["I_1"], 2.0f);
            }
        }
        else {

            PlayerController.playerControllerInstance.isTalking = true;
            DialogActionA = new UnityAction(DialogFunctionA);
            DialogActionB = new UnityAction(DialogFunctionB);
            DialogActionC = new UnityAction(DialogFunctionC);
            GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = false;
            StartCoroutine("DialogCor");
        }
        
        
    }
    private IEnumerator Discovered()
    {
        DisplayManager.displayManagerInstance.DisplayMessage(GameStrings.gameStringsInstance.GetString("BadLooking", null), 3.0f);
        yield return new WaitForSeconds(5);
        PlayerController.playerControllerInstance.PlayerDeath(GameStrings.gameStringsInstance.GetString("PlayerDeathDiscovered", null));
    }

    private IEnumerator DialogCor()
    {
        while (index < dictionary.Count)
        {
            //Debug.Log("Iteracion: " + index);
            switch (dictionary.ElementAt(index).Key[0])
            {

                case 'B':
                    //Debug.Log("B");
                    options.Clear();
                    nOptions = 0;
                    do
                    {
                        options.Add(dictionary.ElementAt(index).Value);
                        //Debug.Log(dictionary.ElementAt(index).Key);
                        index++; nOptions++;
                    } while (dictionary.ElementAt(index).Key[0] == 'B');

                    DialogManager.modalPanel.DisplayButtons(nOptions, DialogActionA, DialogActionB, DialogActionC, options.ToArray());
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    wait = true; button = true;
                    break;

                case 'P':
                    //Debug.Log("P: " + dictionary.ElementAt(index).Value);
                    DialogManager.modalPanel.DisplayPhrase(dictionary.ElementAt(index).Value, "sprite_Anxo_normal");
                    index++;
                    wait = true; button = false;
                    if (dictionary.ElementAt(index).Key[0] == 'B') wait = false;
                    break;

                case 'N':
                    //Debug.Log("P: " + dictionary.ElementAt(index).Value);
                    DialogManager.modalPanel.DisplayPhrase(dictionary.ElementAt(index).Value, "sprite_" + gameObject.name);
                    index++;
                    wait = true; button = false;
                    if (dictionary.ElementAt(index).Key[0] == 'B' && !DialogManager.modalPanel.textRetard) wait = false;
                    break;

                case 'A':
                    //Debug.Log("A");
                    wait = false; button = false;
                    if (!alreadyAnswered) {
                        alreadyAnswered = true; wait = true;
                        if (dictionary.ContainsKey("A_" + res))
                        {
                            DialogManager.modalPanel.DisplayPhrase(dictionary["A_" + res], "sprite_Anxo_normal");
                        }
                        else wait = false;
                    }
                    index++;
                    break;

                case 'R':
                    //Debug.Log("R");
                    wait = false; button = false;
                    if (firstR)
                    {
                        wait = true;
                        firstR = false;
                    }
                    index++;
                    break;

                default:
                    //Debug.Log("Default");
                    index++;
                    wait = false; button = false;
                    break;


            }
            //Debug.Log("Empieza");
            yield return new WaitForSeconds(0.5f);
            //Debug.Log("Acaba");
            while (wait)
            {
                yield return null;
                if (!button) wait = !Input.GetKeyDown(KeyCode.E) || DialogManager.modalPanel.textRetard;
                //Debug.Log("Esperando " + index);
            }
            //Debug.Log("Continua " + index);

        }
        DialogManager.modalPanel.ClosePanel();
        alreadyInteracted = true; PlayerController.playerControllerInstance.isTalking = false;
        //Debug.Log("Fin de la conversación");
        GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
        if (gameObject.name == "SeñoraRamos")
        {
            if (PlayerController.playerControllerInstance.hasFood)
                if (SRFirstTime)
                {
                    this.transform.Translate(1.5f, 0, 0);
                    this.transform.Rotate(0, 180, 0);
                    DisplayManager.displayManagerInstance.DisplayMessage(GameStrings.gameStringsInstance.GetString("FoodRamos", null), 2.0f);
                    SRFirstTime = false;
                    StartCoroutine(PlayerController.playerControllerInstance.IncreaseByTime());
                }
                else DisplayManager.displayManagerInstance.DisplayMessage(dictionary["I_2"], 2.0f);
            else DisplayManager.displayManagerInstance.DisplayMessage(dictionary["I_1"], 2.0f);
        }
    }

    bool jaimeA=false, jaimeB=false, jaimeC=false;
    void DialogFunctionA()
    {
        DialogManager.modalPanel.DisplayPhrase(dictionary["R_A"], "sprite_" + gameObject.name);
        if(gameObject.name == "Jaime")
        {

            index -= 4;
            jaimeA = true;
            if (gameObject.name == "Jaime" && jaimeA && jaimeB && jaimeC) index += 4;

        }
        index++; wait = false; res = 'A'; //alreadyAnswered=true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

 
    void DialogFunctionB()
    {
        //Debug.Log("Button B");
        DialogManager.modalPanel.DisplayPhrase(dictionary["R_B"], "sprite_" + gameObject.name);
        if (gameObject.name == "Jaime")
        {

            index -= 4;
            jaimeB = true;
            if (gameObject.name == "Jaime" && jaimeA && jaimeB && jaimeC) index += 4;

        }
        index++; wait = false; res = 'B';
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void DialogFunctionC()
    {
        //Debug.Log("Button C");
        DialogManager.modalPanel.DisplayPhrase(dictionary["R_C"], "sprite_" + gameObject.name);
        if (gameObject.name == "Jaime")
        {

            index -= 4;
            jaimeC = true;
            if (gameObject.name == "Jaime" && jaimeA && jaimeB && jaimeC) index += 4;

        }
        index++; wait = false; res = 'C';
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}