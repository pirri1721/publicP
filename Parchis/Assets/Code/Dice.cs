using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    public GameObject walls;
    public BoardManager bM;

    private Vector3 originalPosition;
    private Quaternion origianlRotation;

    private Rigidbody rgdB;
    private bool diceInMovement = false;

    void Start () {

        originalPosition = this.transform.position;
        origianlRotation = this.transform.rotation;

        rgdB = this.GetComponent<Rigidbody>();
    }

    public void GetNumb(int numb)
    {
        bM.ThrowDice(numb);
    }

    private void GetNumb()
    {
        Debug.Log("getting numb");
        diceInMovement = false;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo))
        {
            Debug.Log(hitInfo.collider.name);
        }

        if (Physics.Raycast(transform.position, transform.forward,out hitInfo) && hitInfo.collider.name=="RayWall")
        {
            Debug.Log("4");
            bM.ThrowDice(4);
            walls.SetActive(false);
        }
        else
            if (Physics.Raycast(transform.position, -transform.forward, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("3");
            bM.ThrowDice(3);
            walls.SetActive(false);
        }
        else
            if(Physics.Raycast(transform.position, transform.up, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("5");
            bM.ThrowDice(5);
            walls.SetActive(false);
        }
        else
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("2");
            bM.ThrowDice(2);
            walls.SetActive(false);
        }
        else
            if (Physics.Raycast(transform.position, transform.right, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("1");
            bM.ThrowDice(1);
            walls.SetActive(false);
        }
        else
            if (Physics.Raycast(transform.position, -transform.right, out hitInfo) && hitInfo.collider.name == "RayWall")
        {
            Debug.Log("6");
            bM.ThrowDice(6);
            walls.SetActive(false);
        }
        else
        {
            Debug.Log("Rerolling");
            bM.ui.ActiveRerolling();
            //ResetDice();
            Launch();
        }
    }

    public void Launch ()
    {
        if (!diceInMovement)
        {

            StartCoroutine(LaunchCoroutine());
        }
    }

    public IEnumerator LaunchCoroutine()
    {
        yield return new WaitForEndOfFrame();

        diceInMovement = true;
        rgdB.useGravity = true;

        walls.SetActive(true);

        float randomF = UnityEngine.Random.Range(0.3f, 0.7f) * 800f;
        rgdB.AddForce(Vector3.up * randomF);

        float randomT = UnityEngine.Random.Range(0.1f, 0.9f) * 10f;
        rgdB.AddTorque(Vector3.right * randomT);

        yield return new WaitForSeconds(0.25f);

        yield return new WaitUntil(() => rgdB.velocity.magnitude < 0.01f);

        yield return new WaitForSeconds(0.85f);
        GetNumb();
    }

    public void ResetDice()
    {
        this.transform.position = originalPosition;
        this.transform.rotation = origianlRotation;

        rgdB.useGravity = false;
    }
    
}
