using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour {

    public float maxInteractionDistance = 5f;
    private Transform player;
    [HideInInspector] public bool onRange = false;

    private GameObject interactText;

    // Use this for initialization
    public virtual void Start () {
        player = GameObject.Find(Names.player).transform;
        interactText = DisplayManager.displayManagerInstance.interactText;
    }
	
	// Update is called once per frame
	void Update () {
       // if (this.transform.tag == "Zombie") return; //?????????
        }

    public virtual void Interact() {

    }

    private void OnMouseOver() {
        if (this.transform.tag != "Zombie") {
            float distance = Vector3.Distance(player.position, this.transform.position);
            if (distance < maxInteractionDistance && !onRange && !Inventory.inventoryInstance.inventoryPreviousState) {
                interactText.SetActive(true);
                onRange = true;
            } else if (distance > maxInteractionDistance && onRange) {
                interactText.SetActive(false);
                onRange = false;
            }
        }
    }

    private void OnMouseExit() {
        if (onRange) {
            interactText.SetActive(false);
            onRange = false;
        }
    }
}
