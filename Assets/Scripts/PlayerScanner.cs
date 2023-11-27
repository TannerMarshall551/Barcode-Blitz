using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScanner : MonoBehaviour
{

    public bool isScannerMode = false;
    private bool isMoving = false;

    public Animator anim;
    private float animSpeed;

    public void Start(){

        anim = GetComponent<Animator>();
        animSpeed = anim.speed;

        anim.speed = 0f;
        anim.Play("RaiseLowerScanner");
    }

    public void AnimPhaseComplete(){

        isMoving = false;
        anim.speed = 0f;
    }

    public void Toggle(){
        if(!isMoving){
            if(isScannerMode){
                isScannerMode = false;
            }
            else{
                isScannerMode = true;
            }
            isMoving = true;
            anim.speed = animSpeed;
        }
    }

    public bool getScannerMode(){
        return isScannerMode;
    }
}
