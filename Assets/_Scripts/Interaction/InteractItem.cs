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
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine("DestroyObject");
        }
        else if(transform.tag == "Food")
        {
            Inventory.inventoryInstance.AddItem(new Food(transform.name, "Comida que te ayudará a mantenerte cuerdo.", Item.ItemType.Food, Food.FoodType.Roe)); //TODO tipo de comida dinámico
            source.PlayOneShot(InteractSound);
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<SphereCollider>().enabled = false;
            StartCoroutine("DestroyObject");
        }
        else if (transform.tag == "DiaryPage")
        {
            ((Diary)Inventory.inventoryInstance.itemList[1]).AddPage((int)System.Char.GetNumericValue(this.transform.name[this.transform.name.Length-1]));
            source.PlayOneShot(InteractSound);
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DestroyObject");
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
}
