using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour {

    private DisplayManager displayManager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) {
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();

        if (transform.tag == "Head" || transform.tag == "Body") {
            if (col.gameObject.tag == "Arrow") {
                //displayManager.DisplayMessage("Collided with" + col.gameObject.name);
                col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Transform parent = this.transform.parent;
                col.gameObject.transform.parent = parent;
                if (transform.tag == "Head") {
                    parent.gameObject.GetComponent<InteractPerson>().life = 0;
                } else {
                    parent.gameObject.GetComponent<InteractPerson>().life -= 25;
                }
                if (parent.gameObject.GetComponent<InteractPerson>().life == 0) {
                    foreach (Transform child in parent) {
                        if (child.tag == "Head" || child.tag == "Body") {
                            Destroy(child.gameObject);
                        } else {
                            child.GetComponent<Rigidbody>().isKinematic = false;
                        }
                    }
                    parent.DetachChildren();
                    Destroy(parent.gameObject);
                }
                Debug.Log("Vida de " + parent.gameObject.name + ": " + parent.gameObject.GetComponent<InteractPerson>().life);
                displayManager.DisplayMessage("Vida de " + parent.gameObject.name + ": " + parent.gameObject.GetComponent<InteractPerson>().life);
            }
        }
    }
}
