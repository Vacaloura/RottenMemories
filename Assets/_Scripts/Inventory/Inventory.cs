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
    [HideInInspector] public static Inventory inventoryInstance;

    public GameObject[] slots;
    public List<Item> itemList = new List<Item>();

    [HideInInspector] public bool inventoryPreviousState;
    private bool firstClick;
    public float doubleClickDelta;
    private float firstClickTime, secondClickTime;
    private static int lastSlotIndex, actualSlot;
    private static int diaryPage = 0;

    [HideInInspector] public GameObject infoPanel, diaryPanel, inventoryPanel, player;
    private Button delete;
    GraphicRaycaster raycaster;

    public int indice;


    // Use this for initialization
    void Awake()
    {
        if (inventoryInstance == null)
            inventoryInstance = this;
        else Debug.LogError("Tried to create a second inventory");
        slots = GameObject.FindGameObjectsWithTag("Slot").OrderBy(go => go.name).ToArray();
        inventoryPanel = GameObject.Find(Names.inventoryPanel);
        infoPanel = GameObject.Find(Names.infoPanel);
        delete = infoPanel.transform.GetChild(1).GetComponent<Button>();
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
        delete.onClick.AddListener(delegate { RemoveItem(actualSlot); });
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
        inventoryPreviousState = inventoryPanel.activeSelf;

        ToggleInventoryPanel();
        SelectItem();
        UseItem();
        indice = lastSlotIndex;
    }
    #endregion

    //#region EVENTS

    //public delegate void OnItemChanged();
    //public static event OnItemChanged OnItemChangedEvent;

    //#endregion



    public void AddItem(Item item)
    {
        bool alreadyInvented=false;
        if (itemList.Count != 0)
            foreach (Item it in itemList)
            {
                if (item.itemType == it.itemType)
                {
                    if (item.itemType == Item.ItemType.Food)
                    {
                        if (((Food)item).foodType == ((Food)it).foodType)
                        {
                            it.Increase();
                            alreadyInvented = true;
                            break;
                        }
                    }
                    else
                    {
                        it.Increase();
                        alreadyInvented = true;
                        break;
                    }
                }
            }
        if(!alreadyInvented)
        {
            GameObject slot = slots[lastSlotIndex];
            Image icon = slot.transform.GetChild(0).GetComponent<Image>();
            Color tempColor = icon.color; tempColor.a = 1.0f;
            icon.color = tempColor;
            icon.sprite = Resources.Load<Sprite>(item.itemSpriteName);
            //slot.transform.GetChild(1).GetComponent<Text>().text = item.itemName + " (" + item.itemAmount + ")";
            lastSlotIndex++;
            itemList.Add(item);
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

        if (Input.GetKeyDown("tab") && !PlayerController.playerControllerInstance.isTalking)
        {
            PlayerController.playerControllerInstance.source.loop = false;
            PlayerController.playerControllerInstance.source.Stop();
            PlayerController.playerControllerInstance.playerControl = PlayerController.playerControllerInstance.allowInteract = inventoryPreviousState;
; 
            inventoryPanel.SetActive(!inventoryPreviousState);
            if (!inventoryPreviousState)
            {
                DisplayManager.displayManagerInstance.interactText.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined; //Debería ser confined pero no funciona
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            infoPanel.SetActive(false);
            diaryPanel.SetActive(false);

        }
    }

    void SelectItem()
    {
        if (inventoryPreviousState)
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
                        if (actualSlot < itemList.Count)
                        {
                            if (itemList[actualSlot].itemType == Item.ItemType.Diary)
                            {
                                infoPanel.SetActive(true);
                                diaryPanel.SetActive(true);
                                //diaryPanel.transform.GetChild(0).GetComponent<Text>().text = ((Diary)itemList[actualSlot]).ReadPage(diaryPage);
                                diaryPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(((Diary)itemList[actualSlot]).ReadPage(diaryPage));
                                infoPanel.transform.GetChild(0).GetComponent<Text>().text = itemList[actualSlot].itemName + "\n" +  itemList[actualSlot].itemDescription;
                            }
                            else
                            {
                                diaryPanel.SetActive(false);
                                infoPanel.SetActive(true);
                                if (itemList[actualSlot].itemType == Item.ItemType.Harpoon) infoPanel.transform.GetChild(0).GetComponent<Text>().text =
                                        itemList[actualSlot].itemName + " (" + ((Harpoon)itemList[actualSlot]).arrows + GameStrings.gameStringsInstance.GetString("Virotes", null) + itemList[actualSlot].itemDescription;
                                else infoPanel.transform.GetChild(0).GetComponent<Text>().text = itemList[actualSlot].itemName + " (" + itemList[actualSlot].itemAmount + GameStrings.gameStringsInstance.GetString("Unidades", null) + itemList[actualSlot].itemDescription;
                            }
                        }
                        else Debug.Log("Empty slot");
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
                    //Debug.Log(secondClickTime - firstClickTime);
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
                            bool flag = itemList[actualSlot].Consume();
                            if (flag)
                            {
                                Debug.Log("Has consumido: " + itemList[actualSlot].itemName);
                                RemoveItem(actualSlot);
                                infoPanel.SetActive(false);
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

    public void UpdateSlots() {
        int itemNum = 0;
        foreach (GameObject slot in slots) {
            if(itemNum < itemList.Count) {
                Image icon = slot.transform.GetChild(0).GetComponent<Image>();
                Color tempColor = icon.color; tempColor.a = 1.0f;
                icon.color = tempColor;
                icon.sprite = Resources.Load<Sprite>(itemList[itemNum].itemSpriteName);
                //if (itemList[itemNum].itemType == Item.ItemType.Harpoon) slot.transform.GetChild(1).GetComponent<Text>().text = itemList[itemNum].itemName + " (" + ((Harpoon)itemList[itemNum]).arrows + ")";
                //else slot.transform.GetChild(1).GetComponent<Text>().text = itemList[itemNum].itemName + " (" + itemList[itemNum].itemAmount + ")";
            } else {
                Image icon = slot.transform.GetChild(0).GetComponent<Image>();
                Color tempColor = icon.color; tempColor.a = 0.0f;
                icon.color = tempColor;
                icon.sprite = null;                
                //slot.transform.GetChild(1).GetComponent<Text>().text = "Empty Slot";
            }
            itemNum++;
        }
    }

    public void RemoveItem(int number)
    {
        Debug.Log("Number: " + number);
        Debug.Log("ActualSlot: " + actualSlot);
        Debug.Log("Name: " + itemList[number].itemName);

        if (itemList[number].itemType == Item.ItemType.Harpoon || (itemList[number].itemType==Item.ItemType.Food && ((Food)itemList[number]).foodType == Food.FoodType.Cat) || itemList[number].itemType == Item.ItemType.Diary)
            DisplayManager.displayManagerInstance.DisplayMessage(GameStrings.gameStringsInstance.GetString("NonRemovable", null), 2.0f);
        else 
        {
            GameObject slot = slots[number];
            if (itemList[number].itemAmount <= 1) {
                itemList.RemoveAt(number);
                lastSlotIndex--;
                Image icon = slot.transform.GetChild(0).GetComponent<Image>();
                Color tempColor = icon.color; tempColor.a = 0.0f;
                icon.color = tempColor;
                icon.sprite = null;                
                //slot.transform.GetChild(1).GetComponent<Text>().text = "Empty Slot";
                infoPanel.transform.GetChild(0).GetComponent<Text>().text = null;
            } else {
                itemList[number].itemAmount--;
            }

            UpdateSlots();
        }

    }

    public void ReadNextPage()
    {
        if (((Diary)itemList[1]).hasNext(diaryPage)) {
            //ScrollRect scrollRect = diaryPanel.GetComponentInChildren<ScrollRect>();
            //scrollRect.verticalScrollbar.value = 1;
            diaryPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(((Diary)itemList[1]).ReadPage(++diaryPage));
        }
    }

    public void ReadPreviousPage()
    {
        if (((Diary)itemList[1]).hasPrevious(diaryPage))
        {
            //ScrollRect scrollRect = diaryPanel.GetComponentInChildren<ScrollRect>();
            //scrollRect.verticalScrollbar.value = 1;
            diaryPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(((Diary)itemList[1]).ReadPage(--diaryPage));
        }
    }
}
