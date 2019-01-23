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
    private float FadeValue;

    // Use this for initialization
    public virtual void Start () {
        FadeValue = 30.0f;
        player = GameObject.Find(Names.player).transform;
        interactText = DisplayManager.displayManagerInstance.interactText;
        try
        {
            objectLight = transform.Find("ObjectLight").GetComponent<Light>();
            StartCoroutine("LightBlink");
        }catch(Exception)
        {
            Debug.Log(transform.name + " doesn't have an object light");
        }
    }
    public void StopFlashing()
    {
        StopCoroutine("LightBlink");
    }

    IEnumerator LightBlink()
    {
        while (true)
        {
            while (objectLight.intensity < 20.0f)
            {
                objectLight.intensity += Time.deltaTime * FadeValue;
                yield return null;

            }

            while (objectLight.intensity > 0.0f)
            {
                objectLight.intensity -= Time.deltaTime * FadeValue;
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
