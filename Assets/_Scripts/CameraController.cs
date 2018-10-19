using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float smoothPositionFactor;
    public float smoothRotationFactor;

    private Transform playerHead;
    private Vector3 offsetPosition;



    // Use this for initialization
    void Start()
    {
        playerHead = GameObject.Find(Names.playerHead).transform;
        offsetPosition = this.transform.position - playerHead.position;


    }


    // Update is called once per frame
    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 idealPosition;
        idealPosition = playerHead.position + playerHead.rotation * offsetPosition;
        this.transform.position = Vector3.Lerp(this.transform.position, idealPosition, Time.deltaTime * smoothPositionFactor); //Con interpolación

        Quaternion idealRotation = Quaternion.LookRotation(playerHead.forward, playerHead.up);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, idealRotation, Time.deltaTime * smoothRotationFactor); //Con interpolación
    }

}
