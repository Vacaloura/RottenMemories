using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour {

    public float maxInteractionDistance = 3f;
    private Transform player;
    [HideInInspector] public bool onRange = false;

    private GameObject interactText;

    // Use this for initialization
    void Start () {
        player = GameObject.Find(Names.player).transform;
        DisplayManager displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
        interactText = displayManager.interactText;
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
            onRange = true;
        }
        else
        {
            interactText.SetActive(false);
        }
    }

    private void OnMouseExit() {
        interactText.SetActive(false);
        onRange = false;
    }
}
