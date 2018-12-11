using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem : Interactable {

    public AudioClip InteractSound;

    private AudioSource source = null;

    public override void Interact() {
        base.Interact();
        try
        {
            source = GetComponent<AudioSource>();
        }   catch (UnityException e) { Debug.Log("No hay AudioSource: " + e.ToString()); }

        if (transform.tag == "Arrow")
        {
            ((Harpoon)Inventory.inventoryInstance.itemList[0]).arrows++;
            source.PlayOneShot(InteractSound, 1.0f);
            /*this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine("DestroyObject");*/
            transform.parent = GameObject.Find(Names.quiverObject).transform;
            transform.gameObject.SetActive(false);
        }
        else if(transform.tag == "Food")
        {
            Inventory.inventoryInstance.AddItem(new Food(transform.name.Substring(0,(transform.name.Length-1)), "Comida que te ayudará a mantenerte cuerdo.", Item.ItemType.Food, Food.FoodType.PreCooked)); //TODO tipo de comida dinámico
            source.PlayOneShot(InteractSound);
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<SphereCollider>().enabled = false;
            StartCoroutine("DestroyObject");
            PlayerController.playerControllerInstance.hasFood = true;

            int foodNum = (int)char.GetNumericValue(gameObject.name[transform.name.Length]);
            GameObject.Find("Player").GetComponent<PlayerController>().foodTaken[foodNum] = true;
        }
        else if (transform.tag == "MakeUp")
        {
            Inventory.inventoryInstance.AddItem(new Item(transform.name, "Icono_Maquillaje", "Maquillaje que algún vecino dejó abandonado en la huída.", Item.ItemType.MakeUp)); 
            source.PlayOneShot(InteractSound);
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DestroyObject");
        }
        else if (transform.tag == "Wine")
        {
            Inventory.inventoryInstance.AddItem(new Item(transform.name, "Botella de vino que alguien perdió mientras escapaba. CASUALMENTE es un Ribeira.", Item.ItemType.WineBottle)); 
            source.PlayOneShot(InteractSound);
            PlayerController.playerControllerInstance.hasWine = true;
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DestroyObject");
        }
        else if (transform.tag == "Ladder")
        {
            Inventory.inventoryInstance.AddItem(new Item(transform.name, "Icono_Escalera", "Una escalera. Es útil para alcanzar sitios elevados.", Item.ItemType.Ladder)); 
            source.PlayOneShot(InteractSound);
            PlayerController.playerControllerInstance.hasLadder = true;
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DestroyObject");
        }
        else if (transform.tag == "Cat")
        {
            if (PlayerController.playerControllerInstance.hasLadder) {
                Inventory.inventoryInstance.AddItem(new Food(transform.name, "Tu única compañía desde que murió tu esposa.", Item.ItemType.Food, Food.FoodType.Cat));
                PlayerController.playerControllerInstance.hasCat = true;
                source.PlayOneShot(InteractSound);
                this.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false;
                this.transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                StartCoroutine("DestroyObject");
            } else {
                DisplayManager.displayManagerInstance.DisplayMessage("Tengo que ver como llego hasta ahí arriba.", 2.5f);
            }
        }
        else if (transform.tag == "DiaryPage")
        {
            ((Diary)Inventory.inventoryInstance.itemList[1]).AddPage((int)System.Char.GetNumericValue(this.transform.name[this.transform.name.Length-1]));
            source.PlayOneShot(InteractSound);
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DestroyObject");

            int pageNum = (int)char.GetNumericValue(gameObject.name[9]);
            GameObject.Find("Player").GetComponent<PlayerController>().diaryPageTaken[pageNum - 1] = true;
        }
        else if (transform.tag == "Furniture")
        {
            source.PlayOneShot(InteractSound);
        }
        else
        {
            Inventory.inventoryInstance.AddItem(new Item(transform.name, "TODO", Item.ItemType.NotDefined));
            source.PlayOneShot(InteractSound);
            gameObject.SetActive(false);
        }
        GameObject.Find(Names.managers).GetComponent<DisplayManager>().interactText.SetActive(false);
    }

    IEnumerator DestroyObject()
    {
       
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.collider.gameObject.name);
    }
}
