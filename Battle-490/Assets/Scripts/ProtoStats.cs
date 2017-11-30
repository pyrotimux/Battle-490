using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class toonDetail
{
    //public string toonType; //data, virus or antivirus?
    public int health; //toon HP count
    public int atkpower; //toon attack power
    public int defpower; //toon defense power
}

public class ProtoStats : NetworkBehaviour {

    public toonDetail toon;

	// Use this for initialization
	void Start () {

        //initializing each toons stats below
        toon = new toonDetail
        {
            //toon.toonType = "",
            health = 3,
            atkpower = 2,
            defpower = 1
        };

    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "toons")
        {
            toon.health -= toon.atkpower;
            Debug.Log("I GOT HIT! HEALTH IS NOW " + toon.health);
        }
    }
	
    /*
	// Update is called once per frame
	void Update () {
		
	}
    */
}
