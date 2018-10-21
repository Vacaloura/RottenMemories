using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPerson : Interactable {

    public override void Interact() {
        base.Interact();
        if (alreadyInteracted) {
            GameObject.Find(Names.managers).GetComponent<TestModalWindow>().TestYNC();
            alreadyInteracted = false;
        }
    }
}
