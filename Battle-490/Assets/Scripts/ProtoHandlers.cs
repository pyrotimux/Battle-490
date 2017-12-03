using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoHandlers : MonoBehaviour {
    public bool enddown = false, movedown = false, attackdown = false;


    public void Button_End_Turn()
    {
        enddown = true;
    }

    public void Button_Move()
    {
        movedown = true;
    }

    public void Button_Attack()
    {
        attackdown = true;
    }
}
