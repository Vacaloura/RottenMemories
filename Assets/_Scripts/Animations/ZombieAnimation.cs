using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour {

    public Animator anim;

    // Use this for initialization
    void Start () {
        anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update () {
        //if (gameObject.GetComponent<ZombieHordeAgent>().isMoving) { //TODO revisar
        //    anim.SetTrigger("IsMoving");
        //    anim.ResetTrigger("IsStopped");
        //} else {
        //    anim.SetTrigger("IsStopped");
        //    anim.ResetTrigger("IsMoving");
        //}
        if (PlayerController.playerControllerInstance.playerBeingAttacked) {
            if (Random.Range(1, 2) == 1) {
                anim.SetTrigger("IsAttacking1");
                //anim.ResetTrigger("IsAttacking1");
            } else {
                anim.SetTrigger("IsAttacking2");
                //anim.ResetTrigger("IsAttacking2");
            }
        }
        //if (gameObject.GetComponent<ZombieHordeAgent>().zombieBeingAttacked) {
        //    if (Random.Range(1, 2) == 1) {
        //        anim.SetTrigger("IsHitted1");
        //        //anim.ResetTrigger("IsHitted1");
        //    } else {
        //        anim.SetTrigger("IsHitted2");
        //        //anim.ResetTrigger("IsHitted2");
        //    }
        //}
        PlayerController.playerControllerInstance.playerBeingAttacked = false;
        //gameObject.GetComponent<ZombieHordeAgent>().zombieBeingAttacked = false;
    }
}
