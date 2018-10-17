using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    private float maxInteractionDistance = 3f;
    private Transform player;
    private bool alreadyInteracted = false;

    // Use this for initialization
    void Start () {
        player = GameObject.Find(Names.player).transform;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void Interact() {
        float distance = Vector3.Distance(player.position, this.transform.position);
        if (distance < 3f && !alreadyInteracted) {

        }
    }

}
