using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMove : MonoBehaviour {
    public bool selected;
    public Vector3 moveto;

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
        if(moveto.x != transform.position.x && moveto.z != transform.position.z)
            transform.position = Vector3.Lerp(transform.position, new Vector3(moveto.x, 0.0f, moveto.z), Time.deltaTime);
    }
}
