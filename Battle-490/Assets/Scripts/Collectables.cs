using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Collectables : NetworkBehaviour {

    public GameObject collectablesEffect; // effect for when any toons hit it
                                          // note : collectablesEffect = EffectExamples/FireExplosionEffect/Prefabs/SmallExplodeEdited.prefab
    public GameObject bombEffect;
    public int score; // score that each collectables carry (10, 20, 30, 40 or 50)
    public bool randomizeScore;
    public bool vintagePC; // vintagePC also use the same script but it won't ever be a bomb 
    private int bombChance; // random chances (collectables is a bomb or not)
    ProtoContrl[] plyrs;

    void Start()
    {
        // delay to 3 to wait for GameMgr (deyaled 2) to prepare all the player first
        StartCoroutine(DelayStart(3)); // delay init.

        // if the collectable object is not VintagePC, get random number from 0 to 9
        // if it is VintagePC, then we just gonna set the score to 25 in Unity
        if (randomizeScore && !vintagePC) {
            bombChance = UnityEngine.Random.Range(0, 10);
            int rand = UnityEngine.Random.Range(1, 5);
            score = rand * 10;
        }
        
    }

    public IEnumerator DelayStart(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // get all players from game
        int j = 0; // counter so we can assign them 
        plyrs = new ProtoContrl[players.Length]; // init array with length of total player
        foreach (GameObject p in players)
        {
            plyrs[j++] = p.GetComponent<ProtoContrl>(); // get all the proto control and save it in array.
        }

    }

    // destroying game object on the network (not just locally)
    [Command]
    void CmdDestroyGameObject(GameObject go)
    {
        NetworkServer.Destroy(go);
    }

    // when other (the toon in this case) collided with the collectables
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("toons"))
        {
            if (bombChance == 8 || bombChance == 9) // 20% chances that collectables is a bomb
            {
                // collectables is a bomb, destroy toons
                KillToons(other);
            }
            else AddScore(other); // collectables is not a bomb
        }
    }

    void KillToons(Collider toon)
    {
        // step 1 : spawn effect when collided
        Instantiate(bombEffect, transform.position, transform.rotation);

        // step 2 : decrease score, destroy toons
        Debug.Log("Collectables is a bomb..");
        String name = toon.GetComponent<ProtoMove>().owner; // get the owner of the toon
        int j = 0;
        ProtoContrl ply = null;

        foreach (ProtoContrl p in plyrs)
        {
            Debug.Log("Bomb collected by : " + plyrs[j].pname + " = " + name);
            if (plyrs[j].pname == name)
            { // checking the name
                ply = plyrs[j];
            }
            j++;

        }

        // subtracting playerScore in ProtoContrl (total score stored there)
        ply.playerScore -= score; 
        if (ply.playerScore < 0)
        { // we don't want any player got negative score, so we reset it back to zero
            ply.playerScore = 0;
        }
        Debug.Log(ply.pname + " new score : " + ply.playerScore);

        // decrease the health of the toon that collected that bomb
        toon.GetComponent<ProtoMove>().health -= score;
        Debug.Log("Toon new health: " + toon.GetComponent<ProtoMove>().health);

        // step 3 : remove collectables
        CmdDestroyGameObject(gameObject);
    }

    void AddScore(Collider toon)
    {
        // step 1 : spawn effect when collide
        Instantiate(collectablesEffect, transform.position, transform.rotation);

        // step 2 : add score
        Debug.Log("Crashing into collectables..");
        String name = toon.GetComponent<ProtoMove>().owner; // get the owner of the toon
        int j = 0;
        ProtoContrl ply = null;

        foreach (ProtoContrl p in plyrs)
        {
            Debug.Log(plyrs[j].pname + " " + name);
            if (plyrs[j].pname == name) { // checking the name
                ply = plyrs[j];
            } 
            j++;
                
        }

        ply.playerScore += score; // adding score to playerScore in ProtoContrl (total score stored there)
        Debug.Log(ply.pname + " new score : " + ply.playerScore);

        // step 3 : remove collectables from the game
        CmdDestroyGameObject(gameObject);
    }
}
