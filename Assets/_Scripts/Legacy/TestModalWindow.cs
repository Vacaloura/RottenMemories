using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TestModalWindow : MonoBehaviour {

    private DialogManager modalPanel;
    private DisplayManager displayManager;

    private UnityAction myYesAction;
    private UnityAction myNoAction;
    private UnityAction myCancelAction;


    private void Awake() {
        modalPanel = DialogManager.Instance();
        displayManager = DisplayManager.Instance();

        myYesAction = new UnityAction(TestYesFunction);
        myNoAction = new UnityAction(TestNoFunction);
        myCancelAction = new UnityAction(TestCancelFunction);
    }

    public void TestYNC() {
        modalPanel.Choice("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction, false, "falta una string");
    }

    //Send to the Modal Panel to set up the Buttons and Functions to call
    //These are wrapped into UnityActions

    void TestYesFunction() {
        displayManager.DisplayMessage("Heck yeah! Yup!");
    }

    void TestNoFunction() {
        displayManager.DisplayMessage("No way, José!");
    }

    void TestCancelFunction() {
        displayManager.DisplayMessage("I give up!");
    }

}
