using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {

    public float speed = 10.0F;
    private Camera player_camera;

    private int madness;
    public int timeIncreaseValue=5;
    public int timeIncrease=20;
    public int zombieAttackValue=10;
    public int zombieAttackTime=3;

    private int numberOfAtackingZombies=0;

    /*Vector2 mouseLook;
    Vector2 smoothV;
    public float smoothing = 2.0f;*/
    public float sensitivity = 5.0f;
    private Transform playerHead;
    private Transform weapon;

    public GameObject arrowSpawn;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        player_camera = GameObject.Find(Names.playerCamera).GetComponent<Camera>();
        playerHead = GameObject.Find(Names.playerHead).transform;
        weapon = GameObject.Find(Names.harpoon).transform;
        weapon.Rotate(Vector3.left * 10);
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine("IncreaseByTime");
    }
	
	// Update is called once per frame
	void Update () {
        if (madness >= 100)
        {
            Debug.Log("GAME OVER: Te has trasnformado en zombie");
            Application.Quit();
        }
        Movement();
        PlayerInteract();
        ChangeScene();
        Shoot();
    }

    void ChangeScene()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

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
                harpoon.arrows--;
            }
            else {
                DisplayManager displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
                displayManager.DisplayMessage("¡Te has quedado sin virotes!");
            }
        }
    }
    /*void Vision()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        player_camera.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        this.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, this.transform.up);
    }*/

    public void Eat(int value)
    {
        madness -= value;
        if (madness < 0) madness = 0;
        Debug.Log("Player madness: " + madness);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Zombie")
        {
            if (numberOfAtackingZombies == 0)   StartCoroutine("ZombieAttack");
            numberOfAtackingZombies++;

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
            Debug.Log("Player madness: " + madness);
        }
    }

    IEnumerator ZombieAttack()
    {
        while (true)
        {
            madness += zombieAttackValue;
            Debug.Log("Player madness: " + madness);
            yield return new WaitForSeconds(zombieAttackTime);

        }
    }

}
