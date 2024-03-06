using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    private Collider dropZoneCollider;
    public bool canPickupObjectAgain;

    // Start is called before the first frame update
    void Start()
    {
        dropZoneCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectPlaced(){
        if (dropZoneCollider != null)
        {
            dropZoneCollider.enabled = false;
        }
    }

    public void ObjectPickedUp(){
        if (dropZoneCollider != null)
        {
            dropZoneCollider.enabled = true;
        }
    }

    public bool CanPickupObjectAgain(){
        return canPickupObjectAgain;
    }
}
