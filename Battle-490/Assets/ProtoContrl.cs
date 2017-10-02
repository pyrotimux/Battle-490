using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoContrl : MonoBehaviour {
    private Vector3 hitpos;
    private ProtoMove sltobj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.StartsWith("p1"))
                {
                    if (sltobj) sltobj.selected = false;
                    sltobj = hit.transform.GetComponent<ProtoMove>();
                    sltobj.selected = true;
                }
                else if (sltobj) {
                    sltobj.moveto = hit.point;
                }

            }

        }
    }
}
