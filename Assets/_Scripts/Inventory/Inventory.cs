using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using System;

public class Inventory : MonoBehaviour
{
    public AudioClip SelectedSound;
    private AudioSource source = null;

    #region SINGLETON INVENTORY BEHAVIOUR
    public static Inventory inventoryInstance;

    public GameObject[] slots;
    public List<Item> itemList = new List<Item>();

    private bool inventoryState, firstClick;
    public float doubleClickDelta;
    private float firstClickTime, secondClickTime;
    private static int lastSlotIndex, actualSlot;
    private static int diaryPage = 0;

    private GameObject infoPanel, diaryPanel, inventoryPanel, player;

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
        diaryPanel = GameObject.Find(Names.diaryPanel);
        player = GameObject.Find(Names.player);
        try
        {
            source = player.GetComponent<AudioSource>();
        }
        catch (UnityException e) { Debug.Log("No hay AudioSource: " + e.ToString()); }

        //diaryPanel = GameObject.Find(Names.diaryPanel);
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
        infoPanel.SetActive(false);
        diaryPanel.SetActive(false);

        lastSlotIndex = 0;
        doubleClickDelta = 0.20f;

        this.raycaster = GameObject.Find(Names.canvas).GetComponent<GraphicRaycaster>();

        InitializeInventory();

    }

    public void Update()
    {
        inventoryState = inventoryPanel.activeSelf;

        ToggleInventoryPanel();
        SelectItem();
        UseItem();
    }
    #endregion

    //#region EVENTS

    //public delegate void OnItemChanged();
    //public static event OnItemChanged OnItemChangedEvent;

    //#endregion



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


    void InitializeInventory()
    {
        Inventory.inventoryInstance.AddItem(new Harpoon());
        /*Vector3 harpoonPosition= body.transform.position;
        harpoonPosition.z += 0.5f;
        harpoonPosition.x += 0.3f;
        harpoonPosition.y += 0.5f;
        GameObject harpoon = (GameObject)Instantiate(Resources.Load(Names.harpoonPrefab), harpoonPosition, Quaternion.Euler(90, 0, 0));
        harpoon.transform.parent = player.transform;*/

        Inventory.inventoryInstance.AddItem(new Diary());
    }

    void ToggleInventoryPanel()
    { //Desactiviar movimiento de camera?

        if (Input.GetKeyDown("tab"))
        {
            GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = inventoryState;

            inventoryPanel.SetActive(!inventoryState);
            if (!inventoryState)
            {
                Cursor.lockState = CursorLockMode.None; //Debería ser confined pero no funciona
            }
            else Cursor.lockState = CursorLockMode.Locked;

            infoPanel.SetActive(false);
            diaryPanel.SetActive(false);

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
                        source.PlayOneShot(SelectedSound);
                        actualSlot = (int)Char.GetNumericValue(result.gameObject.name[result.gameObject.name.Length - 1]);
                        if (itemList[actualSlot].itemType == Item.ItemType.Diary)
                        {
                            infoPanel.SetActive(false);
                            diaryPanel.SetActive(true);
                            diaryPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Diary)itemList[actualSlot]).ReadPage(diaryPage);
                        }
                        else
                        {
                            diaryPanel.SetActive(false);
                            infoPanel.SetActive(true);
                            infoPanel.transform.GetChild(0).GetComponent<Text>().text = itemList[actualSlot].itemDescription;
                        }
                    }
                }
            }
        }
    }

    void UseItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!firstClick)
            {
                firstClick = true;
                firstClickTime = Time.time;
            }
            else
            {
                secondClickTime = Time.time; 
                if ((secondClickTime - firstClickTime) < doubleClickDelta)
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    List<RaycastResult> results = new List<RaycastResult>();

                    //Raycast using the Graphics Raycaster and mouse click position
                    pointerData.position = Input.mousePosition;
                    raycaster.Raycast(pointerData, results);
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.tag == Names.slotTag)
                        {
                            actualSlot = (int)Char.GetNumericValue(result.gameObject.name[result.gameObject.name.Length - 1]);
                            bool flag=itemList[actualSlot].Consume(player.GetComponent<PlayerController>());
                            if (flag)
                            {
                                Debug.Log("Has consumido: " + itemList[actualSlot].itemName);
                                RemoveItem();
                            }
                        }
                    }
                    firstClick = false;
                }
                else
                {
                    firstClickTime = secondClickTime;
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

    public void RemoveItem()
    {
        itemList.RemoveAt(actualSlot);

        GameObject slot = slots[actualSlot];
        slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        slot.transform.GetChild(1).GetComponent<Text>().text = "Empty Slot";
        infoPanel.transform.GetChild(0).GetComponent<Text>().text = null;
        UpdateSlots();
    }

    public void ReadNextPage()
    {
        if(((Diary)itemList[1]).hasNext(diaryPage)) diaryPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Diary)itemList[1]).ReadPage(++diaryPage);
    }

    public void ReadPreviousPage()
    {
        if (((Diary)itemList[1]).hasPrevious(diaryPage)) diaryPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Diary)itemList[1]).ReadPage(--diaryPage);
    }
}
