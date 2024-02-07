using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  
class for managing the motion of the scanner from its default 
position in the player's hand to being help up in front of the
player's face
*/
public class ScannerMovement : MonoBehaviour
{

    public bool isScannerMode = false; // if the scanner is up or down (false is down)
    private bool isMoving = false; // if the scanner is moving between positions

    public Animator anim;
    private float animSpeed;

    // Start is called on startup
    public void Start(){

        anim = GetComponent<Animator>();
        animSpeed = anim.speed;

        anim.speed = 0f; // stopping the animation while disabled
        anim.Play("RaiseLowerScanner");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Trigger the animation if not already moving
            if (!isMoving)
            {
                // Set the scanner mode
                if(isScannerMode){
                    isScannerMode = false;
                }
                else{
                    isScannerMode = true;
                }
                isMoving = true;
                anim.speed = animSpeed;

                // Set the trigger parameter in the Animator Controller
                isMoving = true; // Set flag to indicate animation is in progress
            }
        }
    }

    // Sets the speed back to zero once the animation phase is complete
    public void AnimPhaseComplete(){

        isMoving = false;
        anim.speed = 0f;
    }

    // Returns if scanner is in scanner mode
    public bool getScannerMode(){
        return isScannerMode;
    }
}
