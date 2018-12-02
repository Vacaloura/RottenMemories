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
                this.transform.parent.GetComponent<ZombieController>().zombieBeingAttacked = true;
                try
                {
                    source = col.gameObject.GetComponent<AudioSource>();
                }
                catch (UnityException e) { Debug.Log("No hay AudioSource: " + e.ToString()); }
                //Lo que puse aquí es para que no suene varias veces lo mismo al dispararle varias veces, pero como
                //cada flecha es una fuente de sonido esta condición no hace nada. Tendría que estar en cada personaje
                //el audio source. No lo cambié por si lo habías puesto así por algún motivo
                if (!source.isPlaying) {
                    source.PlayOneShot(ArrowNail);
                }
                col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Transform parent = this.transform.parent;
                col.gameObject.transform.parent = parent;
                if (transform.tag == "Head") {
                    parent.gameObject.GetComponent<ZombieController>().life = 0;
                } else {
                    parent.gameObject.GetComponent<ZombieController>().life -= 25;
                }
                if (parent.gameObject.GetComponent<ZombieController>().life == 0) {
                    foreach (Transform child in parent) {
                        if (child.tag == "Head" || child.tag == "Body" || child.tag == "Avatar") {
                            Destroy(child.gameObject);
                        } else {
                            Debug.Log(child.name);
                            child.GetComponent<Rigidbody>().isKinematic = false;
                       }
                    }
                   parent.DetachChildren();
                    //DisplayManager.displayManagerInstance.interactText.SetActive(false);
                    Destroy(parent.gameObject);
                }
                Debug.Log("Vida de " + parent.gameObject.name + ": " + parent.gameObject.GetComponent<ZombieController>().life);
                //DisplayManager.displayManagerInstance.DisplayMessage("Vida de " + parent.gameObject.name + ": " + parent.gameObject.GetComponent<ZombieController>().life);
            }
        }
    }


}
