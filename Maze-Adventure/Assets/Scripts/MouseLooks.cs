using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour{
    [Range(50,500)]
    public float sens;
    public Transform body;//Head move

    float xRot= 0f;

    public Transform leaner;
    public float zRot;
    bool isRotating;

    private void Start()
    {
        Cursor.visible = false; //Mouse gözükmesin
    }
    private void Update(){
        #region Camera Mov
        float rotX = Input.GetAxisRaw("Mouse X")* sens * Time.deltaTime;
        float rotY = Input.GetAxisRaw("Mouse Y")* sens * Time.deltaTime;
        
        xRot-=rotY;
        xRot = Mathf.Clamp(xRot, -80f, 80f);//Allows the camera to rotate up to a certain point

        
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        body.Rotate(Vector3.up * rotX); //Body move
        #endregion
        #region Leaner
        if(Input.GetKey(KeyCode.E))
        {
            zRot = Mathf.Lerp(zRot, -20.0f, 5f* Time.deltaTime);
            isRotating = true;
        }

        if(Input.GetKey(KeyCode.Q))
        {
            zRot = Mathf.Lerp(zRot, 20.0f, 5f* Time.deltaTime);
            isRotating = true;
        }

        if(Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            isRotating = false;
        }
        if(!isRotating)
        {
            zRot = Mathf.Lerp(zRot, 0.0f, 5f * Time.deltaTime);
        }
        leaner.localRotation = Quaternion.Euler(0, 0, zRot);
        #endregion
    }
}