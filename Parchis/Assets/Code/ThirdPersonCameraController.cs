using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public float mTurnSpeed;
    public Transform mTarget;
    public float mMaxAngle = 45.0f;

    private float mOffsetX = 0.0f;
    private float mOffsetY = 0.0f;
    private Vector3 mOffset;
    private bool drag = true;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        mOffset = transform.position - mTarget.transform.position;
        mMaxAngle -= transform.eulerAngles.x;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            drag = !drag;
        }

        if (this.gameObject.activeInHierarchy && drag)//!Input.GetKeyDown(KeyCode.LeftControl))
        {
            mOffsetX += Input.GetAxis("Mouse X") * mTurnSpeed;
            mOffsetY += -Input.GetAxis("Mouse Y") * mTurnSpeed;

            mOffsetY = Mathf.Clamp(mOffsetY, -mMaxAngle, mMaxAngle);
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(mOffsetY, mOffsetX, 0);
        transform.position = mTarget.position + rotation * mOffset;
        if(transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        transform.LookAt(mTarget.position);
    }
}
