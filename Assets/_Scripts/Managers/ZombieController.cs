using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

    public Transform target;
    public float maxAttackDistance = 50f;
    public float minAttackDistance = 1.5f;

    private NavMeshAgent myNavAgent;

    // Use this for initialization
    void Start () {
        myNavAgent = transform.GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find(Names.player)) {
            if (Vector3.Distance(GameObject.Find(Names.player).transform.position, this.transform.position) < maxAttackDistance 
                && Vector3.Distance(GameObject.Find(Names.player).transform.position, this.transform.position) > minAttackDistance) {
                myNavAgent.isStopped = false;
                myNavAgent.destination = target.position;

            } else {
                myNavAgent.isStopped = true;
            }
        }
	}
}
