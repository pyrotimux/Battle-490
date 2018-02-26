using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Collectables : NetworkBehaviour {

    public GameObject collectablesEffect;
    public int increaseScore = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("toons"))
        {
            AddScore(other);
        }
    }

    void AddScore(Collider player)
    {
        // spawn effect when collide
        // (install and use Unity Particle Pack)
        //Instantiate(collectablesEffect, transform.position, transform.rotation);

        // add score
        // error: line 32
        // fix: how to get from toons to ProtoContrl to change the score??? 
        ProtoContrl score = player.GetComponent<ProtoContrl>();
        //Debug.Log("Player score : " + score.playerScore);
        score.playerScore += increaseScore;
        Debug.Log("Player new score : " + score.playerScore);

        // remove collectables
        Destroy(gameObject);
    }
}
