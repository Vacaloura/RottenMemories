using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerController : MonoBehaviour {

    public float speed = 10.0F;
    private Camera player_camera;

    private int madness;
    public int timeIncreaseValue=5;
    public int timeIncrease=20;
    public int zombieAttackValue=10;
    public int zombieAttackTime=3;
    [HideInInspector] public bool playerControl = true;

    private DisplayManager displayManager;
    private int numberOfAtackingZombies=0;

    public AudioClip ShootSound;
    public AudioClip DamageDealt;
    public AudioClip MovingSound;
    public AudioClip AmbientSound;

    private AudioSource source = null;
    bool moving = false;

    /*Vector2 mouseLook;
    Vector2 smoothV;
    public float smoothing = 2.0f;*/
    public float sensitivity = 5.0f;
    private Transform playerHead;
    private Transform weapon;
    private GameObject endCamera;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
        player_camera = GameObject.Find(Names.playerCamera).GetComponent<Camera>();
        playerHead = GameObject.Find(Names.playerHead).transform;
        weapon = GameObject.Find(Names.harpoon).transform;
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
    }
	
	// Update is called once per frame
	void Update () {
        if (madness >= 100)
        {
            Debug.Log("GAME OVER: Te has transformado en zombie. Pulse ESC para salir");
            displayManager.DisplayMessage("GAME OVER: Te has transformado en zombie. Pulse ESC para salir");
            endCamera.SetActive(true);
            Destroy(playerHead.parent.parent.gameObject);
        }
        if (playerControl) {
            Movement();
            Shoot();
        }
        PlayerInteract();

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

        Vector3 temp;
        temp = this.transform.GetChild(0).position;
        this.transform.position = this.transform.GetChild(0).position;
        this.transform.GetChild(0).position = temp;

        if (translation != 0 || straffe != 0)
        {
            if (!moving)
            {
                moving = true;
                source.PlayOneShot(MovingSound);
            }
        }
        else
        {
            moving = false;
            source.Stop();
        }

        transform.Translate(straffe, 0, translation);

        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime);
        playerHead.Rotate(-Vector3.right * Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime);
        weapon.Rotate(-Vector3.right * Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime);


        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
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
                GameObject arrow = (GameObject)Instantiate(Resources.Load(Names.arrowPrefab), weapon.position, weapon.rotation * Quaternion.Euler(-90, 0, 0));
                arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * 17;
                source.PlayOneShot(ShootSound);
                harpoon.arrows--;
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
        if (col.gameObject.tag == "Zombie")
        {
            if (numberOfAtackingZombies == 0) {
                numberOfAtackingZombies++;
                StartCoroutine("ZombieAttack");
            } else   numberOfAtackingZombies++;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Zombie")
        {
            numberOfAtackingZombies--;
            if(numberOfAtackingZombies==0)  StopCoroutine("ZombieAttack");
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

    IEnumerator ZombieAttack()
    {
        while (true)
        {
            madness += numberOfAtackingZombies * zombieAttackValue;
            source.PlayOneShot(DamageDealt);
            Debug.Log("Player madness3: " + madness);
            displayManager.DisplayMessage("Player madness: " + madness);
            yield return new WaitForSeconds(zombieAttackTime);
        }
    }
}
