using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

    public Transform target;
    public float maxAttackDistance = 50f;
    public float minAttackDistance = 0.8f;
    public int life = 100;
    public bool zombieBeingAttacked = false;
    public bool isMoving = false;
    public GameObject dialogPanel;

    [HideInInspector] public bool firstAttackFlag = false;
    [HideInInspector] public bool playerBeingAttacked = false;
    public int zombieAttackValue = 10;
    public int zombieAttackTime = 3;

    //private NavMeshAgent myNavAgent;

    // Use this for initialization
    void Start () {
        //myNavAgent = transform.GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find(Names.player)) {
            if (Vector3.Distance(GameObject.Find(Names.player).transform.position, this.transform.position) < maxAttackDistance)
            {
                if (Vector3.Distance(GameObject.Find(Names.player).transform.position, this.transform.position) > minAttackDistance)
                {
                    //myNavAgent.isStopped = false;
                    //myNavAgent.destination = target.position;
                    playerBeingAttacked = false;
                } else
                {
                    //myNavAgent.isStopped = true;
                    playerBeingAttacked = true;
                }
            }
            else
            {
                firstAttackFlag = false;
                //myNavAgent.isStopped = true;
            }
        }
        if (PlayerController.playerControllerInstance.isTalking) {
            gameObject.GetComponent<ZombieHordeAgent>().myNavAgent.isStopped = true;
        } else {
            gameObject.GetComponent<ZombieHordeAgent>().myNavAgent.isStopped = false;
        }
    }
}
