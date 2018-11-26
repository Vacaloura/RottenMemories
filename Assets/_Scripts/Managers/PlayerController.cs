using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour {

    [HideInInspector] public static PlayerController playerControllerInstance;

    
    public float speed = 10.0f;
    public float thrust = 20.0f;
    public float jumpForce = 50.0f;
    private Camera player_camera;

    private int madness;
    public int timeIncreaseValue=5;
    public int timeIncrease=20;
    [HideInInspector] public bool playerControl = true, allowInteract = true, isTalking = false, isMadeUp = false, hasWine = false, hasLadder = false, hasCat = false, playerBeingAttacked = false;

    private DisplayManager displayManager;

    public AudioClip ShootSound;
    public AudioClip DamageDealt;
    public AudioClip MovingSound;
    public AudioClip AmbientSound;

    private AudioSource source = null;
    [HideInInspector] public bool moving = false;

    private Image madnessBar;

    /*Vector2 mouseLook;
    Vector2 smoothV;
    public float smoothing = 2.0f;*/
    public float sensitivity = 5.0f;
    private Transform playerHead;
    private Transform weapon;
    private Transform quiver;
    private GameObject endCamera;

    private void Awake()
    {
        if (playerControllerInstance == null)
            playerControllerInstance = this;
        else Debug.LogError("Tried to create a second PlayerController");

        //GameObject.DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
        player_camera = GameObject.Find(Names.playerCamera).GetComponent<Camera>();
        playerHead = GameObject.Find(Names.playerHead).transform;
        weapon = GameObject.Find(Names.harpoon).transform;
        quiver = GameObject.Find(Names.quiverObject).transform;
        endCamera = GameObject.Find(Names.endCamera);
        endCamera.SetActive(false);
        weapon.Rotate(Vector3.left * 15);
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine("IncreaseByTime");
        playerHead.GetComponent<AudioSource>().PlayOneShot(AmbientSound);
        try
        {
            source = GetComponent<AudioSource>();
        }
        catch (UnityException e) { Debug.Log("No hay AudioSource: " + e.ToString()); }

        madnessBar = GameObject.FindGameObjectWithTag("MadnessBar").GetComponent<Image>();
        
    }
	
    public void PlayerDeath(String message)
    {
        Debug.Log("GAME OVER:\n" + message + "\nPulse ESC para salir");
        displayManager.DisplayMessage("GAME OVER:\n" + message + "\nPulse ESC para salir");
        endCamera.SetActive(true);
        source.Stop();
        Destroy(playerHead.parent.parent.gameObject);
    }
	// Update is called once per frame
	void Update () {
        madnessBar.fillAmount = madness/100f;
        if (madness >= 100)
        {
            PlayerDeath("Tu locura ha aumentado demasiado. Te has transformado en zombie.");
        }

        if (playerControl) {
            Movement();
            Shoot();
        } else {
            source.loop = false;
        }
        if (allowInteract)
        {
            PlayerInteract();
        }

        //Para evitar problemas con el collider de player
        /*Vector3 temp;
        temp = this.transform.GetChild(0).position;
        this.transform.position = this.transform.GetChild(0).position;
        this.transform.GetChild(0).position = temp;*/

        if (Input.GetKeyDown("escape")) {
            playerControl = false;
        }
    }


    void PlayerInteract() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Ray myRay = player_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(myRay, out hit, 100)) {
                Interactable myInteract;
                try {
                    myInteract = hit.collider.transform.GetComponent<Interactable>();
                    if(myInteract.onRange)    myInteract.Interact();
                } catch (Exception e) {
                    Debug.Log("Error: El objeto no tiene Interactable --> " + e.ToString()); //El objeto no tiene Interactable
                }
            }
        }
    }

    void Movement() {
        float translation = Input.GetAxis("Vertical") * speed;
        float straffe = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;

        

        if (translation != 0 || straffe != 0)
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
        }

        transform.Translate(straffe, 0, translation);

        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime);
        playerHead.Rotate(-Vector3.right * Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime);
        weapon.Rotate(-Vector3.right * Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime);

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("jump");
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1f, 0) * jumpForce, ForceMode.Impulse);
        }

    }

    void Shoot() {
        /*Ray myRay = player_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        weapon.rotation = playerHead.rotation * Quaternion.Euler(90, 0, 0);
        if (Physics.Raycast(myRay, out hit)) {
            weapon.Rotate(Vector3.right * Mathf.Atan(Vector3.Distance(playerHead.position, weapon.position) / hit.distance));
        }*/
        if (Input.GetMouseButtonDown(1)) {
                Harpoon harpoon = (Harpoon)Inventory.inventoryInstance.itemList[0];
            if (harpoon.arrows > 0) {
                //GameObject arrow = (GameObject)Instantiate(Resources.Load(Names.arrowPrefab), weapon.position, weapon.rotation);
                Transform arrow = quiver.GetChild(0);   arrow.gameObject.SetActive(true);
                arrow.position = weapon.position;   arrow.rotation = weapon.rotation;
                arrow.gameObject.GetComponent<Rigidbody>().AddForce(arrow.up*thrust);
                source.PlayOneShot(ShootSound);
                Debug.Log(ShootSound.name + source.name);
                harpoon.arrows--;
                arrow.parent = null;
            }
            else {
                displayManager.DisplayMessage("¡Te has quedado sin virotes!");
            }
        }
    }


    public void Eat(int value)
    {
        madness -= value;
        if (madness < 0) madness = 0;
        Debug.Log("Player madness1: " + madness);
        displayManager.DisplayMessage("Player madness: " + madness);
    }

    void OnTriggerEnter(Collider col)
     {

        if (col.gameObject.tag == "Goal" && hasCat)
        {

            Debug.Log("YOU WIN!: Has encontrado a Lúculo y lo has llevado a casa a salvo. Pulse ESC para salir");
            displayManager.DisplayMessage("YOU WIN!: Has encontrado a Lúculo y lo has llevado a casa a salvo. Pulse ESC para salir");
            player_camera.gameObject.SetActive(false);
            playerControl = false; allowInteract = false; source.Stop();

            endCamera.SetActive(true);
        }
    }



        IEnumerator IncreaseByTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIncrease);
            madness += timeIncreaseValue;
            Debug.Log("Player madness2: " + madness);
            displayManager.DisplayMessage("Player madness: " + madness);
        }
    }

    float triggerTime;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            playerBeingAttacked = true;
            //if (!other.gameObject.GetComponent<ZombieHordeAgent>().firstAttackFlag)
            //{
            //    triggerTime = 0;
            //    madness += other.gameObject.GetComponent<ZombieControllerIA>().zombieAttackValue;
            //    source.PlayOneShot(DamageDealt);
            //    Debug.Log("Player madness3: " + madness);
            //    displayManager.DisplayMessage("Player madness: " + madness);
            //    other.gameObject.GetComponent<ZombieControllerIA>().firstAttackFlag = true;
            //}
            //else
            //{
            //    triggerTime += Time.deltaTime;
            //    if (triggerTime >= other.gameObject.GetComponent<ZombieControllerIA>().zombieAttackTime)
            //    {
            //        triggerTime = 0;
            //        madness += other.gameObject.GetComponent<ZombieControllerIA>().zombieAttackValue;
            //        source.PlayOneShot(DamageDealt);
            //        Debug.Log("Player madness3: " + madness);
            //        displayManager.DisplayMessage("Player madness: " + madness);
            //    }
            //}
        }
    }
}
