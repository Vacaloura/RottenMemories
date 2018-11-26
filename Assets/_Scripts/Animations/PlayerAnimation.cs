using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update () {
        Harpoon harpoon = (Harpoon)Inventory.inventoryInstance.itemList[0];
        if (gameObject.GetComponent<PlayerController>().moving) {
            anim.SetTrigger("IsMoving");
            anim.ResetTrigger("IsStopped");
        } else {
            anim.SetTrigger("IsStopped");
            anim.ResetTrigger("IsMoving");
        }
        if (Input.GetMouseButtonDown(1) && harpoon.arrows > 0) {
            anim.SetTrigger("IsShooting");
        }
    }
}
