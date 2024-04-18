using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
   public float speed = 1.0f; // Adjust this speed as needed
   

    void FixedUpdate()
    {
        // Move items on the conveyor belt
        // Adjusting the half-extents to match the desired box size
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale * 9 );
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Move the objects to the right (east)
                Vector3 movement = Vector3.right * speed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + movement);

                // If object reaches the end, you can remove it or perform other actions
                if (rb.position.x > transform.position.x + transform.localScale.x * 9)
                {
                    Destroy(rb.gameObject);
                }
            }
        }
    }
}




