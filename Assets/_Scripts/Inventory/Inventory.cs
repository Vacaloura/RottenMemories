using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using System;

public class Inventory : MonoBehaviour
{
    //TODO añadir con qué empieza el personaje en PlayerController

    #region SINGLETON INVENTORY BEHAVIOUR
    public static Inventory inventoryInstance;

    public GameObject[] slots;
    private static int lastSlotIndex;
    private static int actualSlot;

    private GameObject inventoryPanel;
    private bool inventoryState;

    private GameObject infoPanel;

    GraphicRaycaster raycaster;



    // Use this for initialization
    void Awake()
    {
        if (inventoryInstance == null)
            inventoryInstance = this;
        else Debug.LogError("Tried to create a second inventory");
        slots = GameObject.FindGameObjectsWithTag("Slot").OrderBy(go => go.name).ToArray();
        inventoryPanel = GameObject.Find(Names.inventoryPanel);
        infoPanel = GameObject.Find(Names.infoPanel);
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
        infoPanel.SetActive(false);

        lastSlotIndex = 0;

        this.raycaster = GameObject.Find(Names.canvas).GetComponent<GraphicRaycaster>();

        InitializeInventory();
        CreateAvailableItems();

    }

    public void Update()
    {
        inventoryState = inventoryPanel.activeSelf;

        ToggleInventoryPanel();
        SelectItem();
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
            itemList.Add(item);

        } else
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
                    itemList.Add(item);

                    break;
                }
            }
        //if (OnItemChangedEvent != null) OnItemChangedEvent();
        //else Debug.Log("No method subscribed to the event");
        UpdateSlots();
    }

    public void RemoveItem()
    {
        itemList.RemoveAt(actualSlot);

        GameObject slot = slots[actualSlot];
        slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        slot.transform.GetChild(1).GetComponent<Text>().text = "Empty Slot";
        infoPanel.transform.GetChild(0).GetComponent<Text>().text = null;
        UpdateSlots();
    }

    void InitializeInventory()
    {
        Inventory.inventoryInstance.AddItem(new Harpoon());
        Inventory.inventoryInstance.AddItem(new Diary());
    }

    void ToggleInventoryPanel()
    { //Desactiviar movimiento de camera?

        if (Input.GetKeyDown("tab"))
        {
            inventoryPanel.SetActive(!inventoryState);
            if (!inventoryState)
            {
                Cursor.lockState = CursorLockMode.None; //Debería ser confined pero no funciona
            }
            else Cursor.lockState = CursorLockMode.Locked;

            infoPanel.SetActive(false);

        }

    }

    void SelectItem()
    {
        if (inventoryState)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                raycaster.Raycast(pointerData, results);
                foreach (RaycastResult result in results)
                {
                    if(result.gameObject.tag == Names.slotTag)
                    {
                        infoPanel.SetActive(true);
                        actualSlot = (int)Char.GetNumericValue(result.gameObject.name[result.gameObject.name.Length - 1]);
                        infoPanel.transform.GetChild(0).GetComponent<Text>().text = itemList[actualSlot].itemDescription;
                    }
                }
            }
        }
    }

    void UpdateSlots() {
        int itemNum = 0;
        foreach (GameObject slot in slots) {
            if(itemNum < itemList.Count) {
                slot.transform.GetChild(0).GetComponent<Image>().sprite = itemList[itemNum].itemSprite;
                slot.transform.GetChild(1).GetComponent<Text>().text = itemList[itemNum].itemName + " (" + itemList[itemNum].itemAmount + ")";
            } else {
                slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
                slot.transform.GetChild(1).GetComponent<Text>().text = "Empty Slot";
            }
            itemNum++;
        }
    }

    void CreateAvailableItems()
    {

    }
}
