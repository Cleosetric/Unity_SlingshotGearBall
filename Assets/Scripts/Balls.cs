using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Balls : MonoBehaviour
{
    public float normalSpeed = 5f;

    private void Start() {
        behaviour();
    }

    public virtual void behaviour(){
        
    }
}
