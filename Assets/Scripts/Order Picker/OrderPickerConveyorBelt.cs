using UnityEngine;

public class OrderPickerConveyorBelt : MonoBehaviour
{
    public float conveyorSpeed = 1f;

    private void OnCollisionStay(Collision collision)
    {
        MovePackage(collision.gameObject);
    }

    private void MovePackage(GameObject package)
    {
        Vector3 displacement = Vector3.back * conveyorSpeed * Time.deltaTime;
        package.transform.position += displacement;
    }
}