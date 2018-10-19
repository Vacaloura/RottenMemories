using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    private float maxInteractionDistance = 3f;
    private Transform player;
    [HideInInspector] public bool alreadyInteracted = false;

    // Use this for initialization
    void Start () {
        player = GameObject.Find(Names.player).transform;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public virtual void Interact() {
        float distance = Vector3.Distance(player.position, this.transform.position);
        if (distance < maxInteractionDistance && !alreadyInteracted) {
            Debug.Log("Suficientemente cerca");
            alreadyInteracted = true;
        }
    }

}
