using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    public BoardManager bM;

    private Vector3 originalPosition;
    private Quaternion origianlRotation;

    private Rigidbody rgdB;
    //private Vector3 rgdBVelocityMagnitude;
    //private float rgdBAngularVelocityMagnitude;
    private bool diceThrowed = false;

	// Use this for initialization
	void Start () {

        originalPosition = this.transform.position;
        origianlRotation = this.transform.rotation;

        rgdB = this.GetComponent<Rigidbody>();
        //rgdBVelocityMagnitude = rgdB.velocity*100;

        //StartCoroutine(Launch());
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(rgdB.velocity*10);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LaunchCoroutine());
        }

        Debug.DrawLine(this.transform.position, rgdB.velocity*10, Color.black);

        /*
        Debug.DrawRay(transform.position, transform.forward, Color.blue);//4
        Debug.DrawRay(transform.position, transform.up, Color.black);//5
        Debug.DrawRay(transform.position, transform.right, Color.red);//1
        */

        if(rgdB.velocity * 10 == Vector3.zero && diceThrowed)
        {
            GetNumb();
        }
    }

    private void GetNumb()
    {
        Debug.Log("getting numb");

        diceThrowed = false;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward,out hitInfo) && hitInfo.collider.name=="RayWall")
        {
            Debug.Log("4");
            bM.ThrowDice(4);
        }
        else
            if (Physics.Raycast(transform.position, -transform.forward, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("3");
            bM.ThrowDice(3);
        }
        else
            if(Physics.Raycast(transform.position, transform.up, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("5");
            bM.ThrowDice(5);
        }
        else
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("2");
            bM.ThrowDice(2);
        }
        else
            if (Physics.Raycast(transform.position, transform.right, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("1");
            bM.ThrowDice(1);
        }
        else
            if (Physics.Raycast(transform.position, -transform.right, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("6");
            bM.ThrowDice(6);
        }
    }

    public void Launch ()
    {
        StartCoroutine(LaunchCoroutine());
    }

    public IEnumerator LaunchCoroutine()
    {
        rgdB.useGravity = true;

        float randomF = UnityEngine.Random.Range(0.3f, 0.7f) * 800f;
        rgdB.AddForce(Vector3.up * randomF);

        float randomT = UnityEngine.Random.Range(0.1f, 0.9f) * 10f;
        rgdB.AddTorque(Vector3.right * randomF);

        yield return new WaitForSeconds(0.25f);
        diceThrowed = true;
    }

    public void ResetDice()
    {
        this.transform.position = originalPosition;
        this.transform.rotation = origianlRotation;

        rgdB.useGravity = false;
    }
    
}
