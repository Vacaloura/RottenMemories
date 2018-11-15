using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour {

    private DisplayManager displayManager;

    public AudioClip ArrowNail;
    private AudioSource source = null;


    void OnTriggerEnter(Collider col) {
        if (transform.tag == "Head" || transform.tag == "Body") {
            if (col.gameObject.tag == "Arrow") {
                displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();

                try
                {
                    source = col.gameObject.GetComponent<AudioSource>();
                }
                catch (UnityException e) { Debug.Log("No hay AudioSource: " + e.ToString()); }
                source.PlayOneShot(ArrowNail);

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
                    GameObject.Find(Names.managers).GetComponent<DisplayManager>().interactText.SetActive(false);
                    Destroy(parent.gameObject);
                }
                Debug.Log("Vida de " + parent.gameObject.name + ": " + parent.gameObject.GetComponent<InteractPerson>().life);
                displayManager.DisplayMessage("Vida de " + parent.gameObject.name + ": " + parent.gameObject.GetComponent<InteractPerson>().life);
            }
        }
    }


}
