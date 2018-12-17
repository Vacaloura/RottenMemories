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
            Inventory.inventoryInstance.AddItem(new Food(GameStrings.gameStringsInstance.GetString("FoodName",null), GameStrings.gameStringsInstance.GetString("FoodDescription", null), Item.ItemType.Food, Food.FoodType.PreCooked)); //TODO tipo de comida dinámico
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
            Inventory.inventoryInstance.AddItem(new Item(GameStrings.gameStringsInstance.GetString("MakeUpName", null), "Icono_Maquillaje", GameStrings.gameStringsInstance.GetString("MakeUpDescription", null), Item.ItemType.MakeUp)); 
            source.PlayOneShot(InteractSound);
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DestroyObject");
        }
        else if (transform.tag == "Wine")
        {
            Inventory.inventoryInstance.AddItem(new Item(GameStrings.gameStringsInstance.GetString("WineName", null), GameStrings.gameStringsInstance.GetString("WineDescription", null), Item.ItemType.WineBottle)); 
            source.PlayOneShot(InteractSound);
            PlayerController.playerControllerInstance.hasWine = true;
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DestroyObject");
        }
        else if (transform.tag == "Ladder")
        {
            Inventory.inventoryInstance.AddItem(new Item(GameStrings.gameStringsInstance.GetString("LadderName", null), "Icono_Escalera", GameStrings.gameStringsInstance.GetString("LadderDescription", null), Item.ItemType.Ladder)); 
            source.PlayOneShot(InteractSound);
            PlayerController.playerControllerInstance.hasLadder = true;
            this.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DestroyObject");
        }
        else if (transform.tag == "Cat")
        {
            if (PlayerController.playerControllerInstance.hasLadder) {
                Inventory.inventoryInstance.AddItem(new Food(GameStrings.gameStringsInstance.GetString("CatName", null), GameStrings.gameStringsInstance.GetString("CatDescription", null), Item.ItemType.Food, Food.FoodType.Cat));
                PlayerController.playerControllerInstance.hasCat = true;
                source.PlayOneShot(InteractSound);
                this.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false;
                this.transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                StartCoroutine("DestroyObject");
            } else {
                DisplayManager.displayManagerInstance.DisplayMessage(GameStrings.gameStringsInstance.GetString("CantPick", null), 2.5f);
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
        Destroy(this.gameObject.transform.GetChild(0).gameObject);
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.collider.gameObject.name);
    }
}
