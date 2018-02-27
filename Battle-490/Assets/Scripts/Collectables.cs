using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Collectables : NetworkBehaviour {

    public GameObject collectablesEffect;
    public int increaseScore = 50;
    ProtoContrl[] plyrs;

    void Start()
    {
        StartCoroutine(DelayStart(3)); // delay init.
    }

    public IEnumerator DelayStart(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // get all players from game
        int j = 0; // counter so we can assign them 
        plyrs = new ProtoContrl[players.Length]; // init array with lenght of total player
        foreach (GameObject p in players)
        {
            plyrs[j++] = p.GetComponent<ProtoContrl>(); // get all the proto control and save it in array.
        }

    }

    [Command]
    void CmdDestroyGameObject(GameObject go)
    {
        NetworkServer.Destroy(go);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("toons"))
        {
            AddScore(other);
        }
    }

    void AddScore(Collider toon)
    {
        // spawn effect when collide
        // (install and use Unity Particle Pack and refer it to collectablesEffect)
        //Instantiate(collectablesEffect, transform.position, transform.rotation);

        // add score
        Debug.Log("Crashing into collectables..");
        String name = toon.GetComponent<ProtoMove>().owner;
        int j = 0;
        ProtoContrl ply = null;

        foreach (ProtoContrl p in plyrs)
        {
            Debug.Log(plyrs[j].pname + " " + name);
            if (plyrs[j].pname == name) {
                ply = plyrs[j];
            }
            j++;
                
        }

        //Debug.Log("Player score : " + score.playerScore);
        ply.playerScore += increaseScore;
        Debug.Log("Player new score : " + ply.playerScore);

        // remove collectables
        CmdDestroyGameObject(gameObject);
    }
}
