using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    private bool isFirstPerson = true;

    void Start()
    {
        ActivateFirstPerson(); // Başlangıçta bir tanesi aktif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isFirstPerson = !isFirstPerson;

            if (isFirstPerson)
                ActivateFirstPerson();
            else
                ActivateThirdPerson();
        }
    }

    void ActivateFirstPerson()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
    }

    void ActivateThirdPerson()
    {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.enabled = true;
    }
}