using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public static PlayerController playerControllerInstance;

    public float thrust = 20.0f;
    private float rotX = 0.0f;
    private float rotY = 0.0f;

    private Camera player_camera;
    private CharacterController movementController;

    [HideInInspector] public int madness;
    public int timeDamage = 5;
    public int timeIncrease = 20;
    /*[HideInInspector]*/
    public bool playerControl = true, allowInteract = true, isTalking = false, isMadeUp = false, hasWine = false, hasLadder = false, hasCat = false, hasFood = false;
    public bool[] foodTaken = new bool[6] { false, false, false, false, false, false };
    public bool[] diaryPageTaken = new bool[4] { false, false, false, false };

    private DisplayManager displayManager;

    public AudioClip ShootSound;
    public AudioClip DamageDealt;
    public AudioClip MovingSound;
    public AudioClip AmbientSound;
    public AudioClip EatingSound;
    private float audioCrossfade = 2.0f;


    [HideInInspector] public AudioSource source = null;
    [HideInInspector] public AudioSource sourceAmbient = null;
    [HideInInspector] public bool moving = false;

    private Image madnessBar;
    public Image avatar;
    private PlayableDirector cinematic;
    public GameObject aimSight;

    /*Vector2 mouseLook;
    Vector2 smoothV;
    public float smoothing = 2.0f;*/
    private Transform playerHead;
    private Transform weapon;
    private float weaponRotX = 0.0f;
    private float weaponRotY = 0.0f;
    private float weaponRotZ = 0.0f;
    private Transform quiver;
    private GameObject endCamera;
    public GameObject myLight;
    public float lightChange;

    private void Awake()
    {
        if (playerControllerInstance == null)
            playerControllerInstance = this;
        else Debug.LogError("Tried to create a second PlayerController");

        //GameObject.DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
        player_camera = GameObject.Find(Names.playerCamera).GetComponent<Camera>();
        playerHead = GameObject.Find(Names.playerHead).transform;
        weapon = GameObject.Find(Names.harpoon).transform;
        quiver = GameObject.Find(Names.quiverObject).transform;
        endCamera = GameObject.Find(Names.endCamera);
        endCamera.SetActive(false);
        cinematic = GameObject.Find(Names.cinematicCamera).GetComponent<PlayableDirector>();
        movementController = gameObject.GetComponent<CharacterController>();
        //weapon.Rotate(Vector3.left * 15);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        DisplayManager.displayManagerInstance.intext.GetComponent<Text>().text = GameStrings.gameStringsInstance.GetString("Skip", null);
        //StartCoroutine("IncreaseByTime");
        try
        {
            sourceAmbient = GameObject.Find(Names.playerCamera).GetComponent<AudioSource>();
            sourceAmbient.clip = AmbientSound;
            sourceAmbient.Play();
        }
        catch (UnityException e) { Debug.Log("No hay AudioSource en PlayerCamera: " + e.ToString()); }

        //playerHead.GetComponent<AudioSource>().PlayOneShot(AmbientSound);
        try
        {
            source = GetComponent<AudioSource>();
        }
        catch (UnityException e) { Debug.Log("No hay AudioSource: " + e.ToString()); }

        madnessBar = GameObject.FindGameObjectWithTag("MadnessBar").GetComponent<Image>();
        if (GameController.gameControllerInstance.loadData)
        {
            GameController.gameControllerInstance.LoadPlayerData();
            Inventory.inventoryInstance.UpdateSlots();
        }
        timeDamage = GameController.gameControllerInstance.currentGameData.timeDamage;
        timeIncrease = GameController.gameControllerInstance.currentGameData.timeIncrease;
        weaponRotX = weapon.localRotation.x;
        weaponRotY = weapon.localRotation.y;
        weaponRotZ = weapon.localRotation.z;

        if (isMadeUp) avatar.sprite = Resources.Load<Sprite>("sprite_Anxo_normal");
        else avatar.sprite = Resources.Load<Sprite>("sprite_Anxo_locura");

        Screen.fullScreen = GameController.gameControllerInstance.gameWindowed;
        if (!GameController.gameControllerInstance.gameWindowed)
        {
            aimSight.GetComponent<RectTransform>().localPosition += Vector3.up * 13;
        }
    }

    public void PlayerDeath(String message)
    {
        Debug.Log("GAME OVER:\n" + message + "\nPulse ESC para salir o R para volver al último checkpoint.");
        displayManager.DisplayMessage(GameStrings.gameStringsInstance.GetString("PlayerDeath", message), 7.0f);
        endCamera.SetActive(true);
        source.Stop();
        Time.timeScale = 0;
        Destroy(playerHead.parent.parent.gameObject);
    }

    private bool cinematicOver = false;
    // Update is called once per frame
    void Update()
    {
        madnessBar.fillAmount = madness / 100f;
        if (madness >= 100)
        {
            PlayerDeath(GameStrings.gameStringsInstance.GetString("PlayerDeathByMadness", null));
        }

        if (!cinematicOver)
        {
            if (cinematic.state != PlayState.Playing || Input.GetKeyDown(KeyCode.E))
            {
                cinematic.gameObject.SetActive(false);
                cinematicOver = true;
                DisplayManager.displayManagerInstance.intext.GetComponent<Text>().text = GameStrings.gameStringsInstance.GetString("Interact", null);
                DisplayManager.displayManagerInstance.interactText.SetActive(false);
            }

        }
        if (playerControl && cinematicOver)
        {
            Movement();
            Shoot();
        }
        else
        {
            source.loop = false;
        }

        if (allowInteract && cinematicOver)
        {
            PlayerInteract();
        }
        if (myLight != null)
        {
            myLight.GetComponent<Light>().intensity -= Time.deltaTime / lightChange;
            if (myLight.GetComponent<Light>().intensity <= 0 || myLight.GetComponent<Light>().intensity >= 1.33)
                lightChange = -1 * lightChange;
            if (!sourceAmbient.isPlaying)
            {
                audioCrossfade -= Time.deltaTime;
                if (audioCrossfade < 0)
                {
                    sourceAmbient.clip = AmbientSound;
                    sourceAmbient.Play();
                }
            }
            else
            {
                audioCrossfade = 2.0f;
            }
        }
        if (Screen.fullScreen != GameController.gameControllerInstance.gameWindowed)
        {
            Screen.fullScreen = GameController.gameControllerInstance.gameWindowed;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Para evitar problemas con el collider de player
        /*Vector3 temp;
        temp = this.transform.GetChild(0).position;
        this.transform.position = this.transform.GetChild(0).position;
        this.transform.GetChild(0).position = temp;*/

    }


    void PlayerInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            source.loop = false;
            source.Stop();
            Ray myRay = player_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(myRay, out hit, 100))
            {
                Interactable myInteract;
                try
                {
                    myInteract = hit.collider.transform.GetComponent<Interactable>();
                    if (myInteract.onRange) myInteract.Interact();
                }
                catch (Exception e)
                {
                    Debug.Log("Error: El objeto no tiene Interactable --> " + e.ToString()); //El objeto no tiene Interactable
                }
            }
        }
    }

    private Vector3 moveDirection = Vector3.zero;
    private float gravity = 20.0f;
    private float speed = 10.0f;
    public float walkSpeed = 7.0f;
    public float runSpeed = 14.0f;
    public float jumpForce = 6.0f;
    public float sensitivity = 50.0f;
    void Movement()
    {
        if (movementController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;
            if (Input.GetKeyDown("space"))
            {
                moveDirection.y = jumpForce;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
            if ((moveDirection.x != 0 || moveDirection.z != 0) && movementController.isGrounded)
            {
                if (!moving)
                {
                    moving = true;
                    source.loop = true;
                    source.clip = MovingSound;
                    source.Play();
                }
            }
            else
            {
                moving = false;
                source.loop = false;
                source.Stop();
            }
        }
        else
        {
            moving = false;
            source.loop = false;
            source.Stop();
        }

        //// Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        ////Move the controller
        movementController.Move(moveDirection * Time.deltaTime);

        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -90f, 90f);
        transform.rotation = Quaternion.Euler(0f, rotX, 0f);
        playerHead.transform.localRotation = Quaternion.Euler(-rotY, 0f, 0f);
        weapon.transform.localRotation = Quaternion.Euler(-rotY + 83.329f, 0f, 0f);

        // Old rotation
        /*transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime);
        playerHead.Rotate(-Vector3.right * Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime);
        if (playerHead.eulerAngles.x > 85 && playerHead.eulerAngles.x < 200)
            playerHead.transform.localRotation = Quaternion.Euler(85f, 0f, 0f);
        else if (playerHead.eulerAngles.x < 275 && playerHead.eulerAngles.x > 200)
            playerHead.transform.localRotation = Quaternion.Euler(275f, 0f, 0f);
        else weapon.Rotate(-Vector3.right * Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime);*/
    }

    void Shoot()
    {
        /*Ray myRay = player_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        weapon.rotation = playerHead.rotation * Quaternion.Euler(90, 0, 0);
        if (Physics.Raycast(myRay, out hit)) {
            weapon.Rotate(Vector3.right * Mathf.Atan(Vector3.Distance(playerHead.position, weapon.position) / hit.distance));
        }*/
        if (Input.GetMouseButtonDown(1))
        {
            Harpoon harpoon = (Harpoon)Inventory.inventoryInstance.itemList[0];
            if (harpoon.arrows > 0)
            {
                //GameObject arrow = (GameObject)Instantiate(Resources.Load(Names.arrowPrefab), weapon.position, weapon.rotation);
                Transform arrow = quiver.GetChild(0);
                arrow.gameObject.SetActive(true);
                arrow.position = weapon.position;
                arrow.rotation = weapon.rotation;
                //Debug.Log("Arrow rotation: " + arrow.rotation);
                arrow.GetComponent<Rigidbody>().isKinematic = false;
                arrow.gameObject.GetComponent<Rigidbody>().AddForce(arrow.up * thrust);
                source.PlayOneShot(ShootSound);
                Debug.Log(ShootSound.name + source.name);
                harpoon.arrows--;
                arrow.parent = null;
            }
            else
            {
                displayManager.DisplayMessage(GameStrings.gameStringsInstance.GetString("EmptyMunition", null), 2.0f);
            }
            //Inventory.inventoryInstance.slots[0].transform.GetChild(1).GetComponent<Text>().text = harpoon.itemName + " (" + harpoon.arrows + ")";
        }
    }


    public void Eat(int value)
    {
        source.PlayOneShot(EatingSound);
        madness -= value;
        if (madness < 0) madness = 0;
        Debug.Log("Player madness1: " + madness);
        //displayManager.DisplayMessage("Player madness: " + madness);
    }

    public void PlayerWin(string message)
    {
        Debug.Log("YOU WIN!: " + message + " \nCargando siguiente nivel....");
        StartCoroutine("LoadLevel");
        displayManager.DisplayMessage(GameStrings.gameStringsInstance.GetString("PlayerWin", message), 7.0f);
        DontDestroyOnLoad(this.gameObject);
        //player_camera.gameObject.SetActive(false);
        //playerControl = false; allowInteract = false; source.Stop();
        //endCamera.SetActive(true);
        //Time.timeScale = 0;

    }

    public IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(8.0f);
        Debug.Log("Loading");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Closed")
        {
            DisplayManager.displayManagerInstance.DisplayMessage(GameStrings.gameStringsInstance.GetString("LockedDoor", null), 2.0f);
        }
        if (col.gameObject.tag == "Goal" && hasCat)
        {
            PlayerWin(GameStrings.gameStringsInstance.GetString("CatRescued", null));
        }
    }



    public IEnumerator IncreaseByTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIncrease);
            if (!isTalking)
            {
                madness += timeDamage;
                damagePanel.GetComponent<Image>().color = new Color(255, 190, 0);
                StopCoroutine("RedFlash");
                StartCoroutine("RedFlash");
                Debug.Log("Player madness2: " + madness);
                //displayManager.DisplayMessage("Player madness: " + madness);
            }
        }
    }

    float triggerTime;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            if (!other.gameObject.GetComponent<ZombieController>().firstAttackFlag)
            {
                Debug.Log("firstAttackFlag: " + other.gameObject.GetComponent<ZombieController>().firstAttackFlag);
                triggerTime = 0;
                madness += other.gameObject.GetComponent<ZombieController>().zombieAttackValue;
                damagePanel.GetComponent<Image>().color = Color.red;
                StopCoroutine("RedFlash");
                StartCoroutine("RedFlash");
                source.PlayOneShot(DamageDealt);
                //Debug.Log("Player madness3: " + madness);
                //displayManager.DisplayMessage("Player madness: " + madness);
                other.gameObject.GetComponent<ZombieController>().firstAttackFlag = true;
            }
            else
            {
                triggerTime += Time.deltaTime;
                //Debug.Log("triggerTime: " + triggerTime);
                if (triggerTime >= other.gameObject.GetComponent<ZombieController>().zombieAttackTime)
                {
                    triggerTime = 0;
                    madness += other.gameObject.GetComponent<ZombieController>().zombieAttackValue;
                    damagePanel.GetComponent<Image>().color = Color.red;
                    StopCoroutine("RedFlash");
                    StartCoroutine("RedFlash");
                    source.PlayOneShot(DamageDealt);
                    //Debug.Log("Player madness3: " + madness);
                    //displayManager.DisplayMessage("Player madness: " + madness);
                }
            }
        }
    }

    public GameObject damagePanel;
    public float flashTime = 0.01f, fadeTime = 0.5f;
    IEnumerator RedFlash()
    {
        Color resetPanelColor = damagePanel.GetComponent<Image>().color;
        resetPanelColor.a = 0.4f;
        damagePanel.GetComponent<Image>().color = resetPanelColor;

        yield return new WaitForSeconds(flashTime);

        while (resetPanelColor.a > 0)
        {
            resetPanelColor.a -= Time.deltaTime * 0.4f / fadeTime;
            damagePanel.GetComponent<Image>().color = resetPanelColor;
            yield return null;
        }
        yield return null;
    }
}
