using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// this only exists on the server. it handles all server calculations.
/// </summary>
/// 
public class GameMgr : NetworkBehaviour {
    public int i = 1; // defined which player turn it is.

    public ProtoContrl[] plyrs;
    public bool delayed = true;
    

    public void GameOver() {
        foreach (ProtoContrl player in plyrs)
        {
            player.gameover = true;

        }
    }



    /// <summary>delayed start by number of sec then init stuff.</summary>
    /// <param name="time"> wait till this time and run the method </param>
    public IEnumerator DelayStart(float time)
    {
        yield return new WaitForSeconds(time);
        delayed = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // get all players from game
        int j = 0; // counter so we can assign them 
        plyrs = new ProtoContrl[players.Length]; // init array with lenght of total player
        foreach(GameObject p in players) {
            plyrs[j++] = p.GetComponent<ProtoContrl>(); // get all the proto control and save it in array.
        }

    }
    // Use this for initialization
    void Start () {
        StartCoroutine(DelayStart(2)); // delay init.
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (delayed) return;

        foreach (ProtoContrl player in plyrs)
        {
            if (player.winning)
            {
                GameOver();
                return;
            }

        }

        GameObject[] go = GameObject.FindGameObjectsWithTag("score");

        foreach (GameObject g in go)
        {
            if (g.name == "p1score")
            {
                g.GetComponent<Slider>().value = plyrs[0].playerScore;
                Debug.Log(g.name + ": " + plyrs[0].playerScore);
                /*
                GameObject textscore = GameObject.FindGameObjectWithTag("scorevalue");
                if (textscore.name == "p1scoreText")
                {
                    textscore.GetComponent<Text>().text = "" + plyrs[0].playerScore;
                }
                */
            }
            else
            {
                g.GetComponent<Slider>().value = plyrs[1].playerScore;
                Debug.Log(g.name + ": " + plyrs[1].playerScore);
                //g.GetComponent<Text>().text = "" + plyrs[1].playerScore;
            }
        }

        

        if (plyrs[i].myturn == false) { // if player finish the turn then we will let other player start.
            if (i + 1 == plyrs.Length) i = 0;  // circle back to first player
            else i++; // or go to next player

            plyrs[i].myturn = true; // set next player turn to true

            // now set all toons can move and can attack bool to true so they can move again.
            GameObject[] toons = GameObject.FindGameObjectsWithTag("toons"); 
            foreach (GameObject toon in toons) {
                ProtoMove t = toon.GetComponent<ProtoMove>();
                if (t.owner == plyrs[i].pname) {
                    t.canattack = true; t.canmove = true;
                    
                }
                
            }
        }



    }
}
