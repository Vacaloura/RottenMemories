using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //TODO añadir con qué empieza el personaje en PlayerController

    #region SINGLETON INVENTORY
    public static Inventory inventoryInstance;
    public GameObject[] slots;
    private static int lastSlotIndex;
    // Use this for initialization
    void Awake()
    {
        if (inventoryInstance == null)
            inventoryInstance = this;
        else Debug.LogError("Tried to create a second inventory");
        slots = GameObject.FindGameObjectsWithTag("Slot");

    }

    void Start()
    {
        GameObject.Find(Names.inventoryPanel).SetActive(false);
        lastSlotIndex = 0;
    }
    #endregion

    //#region EVENTS

    //public delegate void OnItemChanged();
    //public static event OnItemChanged OnItemChangedEvent;

    //#endregion


    public List<Item> itemList = new List<Item>();

    public void AddItem(Item item)
    {
        if(itemList.Count == 0)
        {
            GameObject slot = slots[lastSlotIndex];
            slot.transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
            slot.transform.GetChild(1).GetComponent<Text>().text = item.itemName + " (" + item.itemAmount + ")";
            lastSlotIndex++;
        }
        else
            foreach (Item it in itemList)
            {
                if (item.itemType == it.itemType)
                {
                    it.Increase();
                    break;
                }
                else
                {
                    GameObject slot = slots[lastSlotIndex];
                    slot.transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
                    slot.transform.GetChild(1).GetComponent<Text>().text = item.itemName + " (" + item.itemAmount + ")";
                    lastSlotIndex++;

                    break;
                }
            }
        itemList.Add(item);
        //if (OnItemChangedEvent != null) OnItemChangedEvent();
        //else Debug.Log("No method subscribed to the event");
    }

    public void RemoveItem(Item item)
    {
        itemList.Remove(item);
        //OnItemChangedEvent();
    }
}
