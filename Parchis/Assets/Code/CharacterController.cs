using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameObject weapon;

    public Transform boneToAttach;
    Transform weaponOriginalBone;
    Vector3 weaponOriginalLocalposition;
    Quaternion weaponOriginalRotation;

    // Start is called before the first frame update
    void Start()
    {
        weaponOriginalBone = weapon.transform.parent;

        weaponOriginalLocalposition = weapon.transform.localPosition;
        weaponOriginalRotation = weapon.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RootWeapon()
    {
        weapon.transform.SetParent(weaponOriginalBone);
        weapon.transform.localPosition = weaponOriginalLocalposition;
        weapon.transform.localRotation = weaponOriginalRotation;
    }

    public void UnRootWeapon()
    {
        weapon.transform.SetParent(boneToAttach);
    }
}
