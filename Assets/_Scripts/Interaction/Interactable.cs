using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour {

    public float maxInteractionDistance = 5f;
    private Transform player;
    [HideInInspector] public bool onRange = false;

    private GameObject interactText;
    private Light objectLight;
    private float lightFadeTime;

    // Use this for initialization
    public virtual void Start () {
        lightFadeTime = 1.0f;
        player = GameObject.Find(Names.player).transform;
        interactText = DisplayManager.displayManagerInstance.interactText;
        try
        {
            objectLight = transform.Find("ObjectLight1").GetComponent<Light>();
            StartCoroutine("LightBlink");
        }catch(Exception)
        {
            Debug.Log(transform.name + " doesn't have an object light");
        }
    }

    IEnumerator LightBlink()
    {
        while (true)
        {
            Boolean flag = objectLight.intensity > 4.0f;
            while (objectLight.intensity < 4.0f)
            {
                objectLight.intensity += Time.deltaTime / lightFadeTime;
                yield return null;

            }

            while (objectLight.intensity > 0.3f)
            {
                objectLight.intensity -= Time.deltaTime / lightFadeTime;
                yield return null;

            }

            yield return null;
        }
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
                if (gameObject.name == "SeñoraRamos" && !PlayerController.playerControllerInstance.isMadeUp) {
                    DisplayManager.displayManagerInstance.DisplayMessage(GameStrings.gameStringsInstance.GetString("ShouldNotTalk", null), 2.0f);
                }
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
