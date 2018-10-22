using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour {

    private float maxInteractionDistance = 3f;
    private Transform player;
    [HideInInspector] public bool alreadyInteracted = false;

    public GameObject interactText;

    // Use this for initialization
    void Start () {
        player = GameObject.Find(Names.player).transform;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public virtual void Interact() {
        
    }

    private void OnMouseOver() {
        float distance = Vector3.Distance(player.position, this.transform.position);
        if (distance < maxInteractionDistance) {
            Debug.Log("Suficientemente cerca");
            interactText.SetActive(true);
            alreadyInteracted = true;
        }
    }

    private void OnMouseExit() {
        interactText.SetActive(false);
        alreadyInteracted = false;
    }
}
