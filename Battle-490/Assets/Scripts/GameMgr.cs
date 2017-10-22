using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMgr : NetworkBehaviour {
    public int i = 1;

    public ProtoContrl[] plyrs;
    public bool delayed = true;


    public IEnumerator DelayStart(float time)
    {
        yield return new WaitForSeconds(time);
        delayed = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int j = 0;
        plyrs = new ProtoContrl[players.Length];
        foreach(GameObject p in players) {
            plyrs[j++] = p.GetComponent<ProtoContrl>();
        }

    }
    // Use this for initialization
    void Start () {
        StartCoroutine(DelayStart(2));
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (delayed) return;

        if (plyrs[i].myturn == false) {
            if (i + 1 == plyrs.Length) i = 0;
            else i++;

            plyrs[i].myturn = true;
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
