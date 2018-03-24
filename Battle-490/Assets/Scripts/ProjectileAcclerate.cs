using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileAcclerate : NetworkBehaviour
{
    [SyncVar]
    public string owner = "";

    [SyncVar]
    public int type = 1;

    private void OnCollisionEnter(Collision collision){

    }

    // Update is called once per frame
    void FixedUpdate () {
        GetComponent<Rigidbody>().velocity = transform.forward * 6;
    }
}
