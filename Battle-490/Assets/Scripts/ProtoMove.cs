using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProtoMove : NetworkBehaviour {
    public bool selected;
    public Vector3 moveto;
    public int startpos = 0, owner = 0;

    private Vector3 movingto;

	// Use this for initialization
	void Start () {
        moveto = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (selected)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else {
            GetComponent<Renderer>().material.color = Color.white;
        }

        movingto = new Vector3(moveto.x, moveto.y, moveto.z);
        if (Vector3.Distance(transform.position, movingto) > 0.5f) {
            transform.position = Vector3.Lerp(transform.position, movingto, Time.deltaTime);
        }
        else {
            transform.position = movingto;
        }
        
    }
}
