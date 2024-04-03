using UnityEngine;
using System.Collections;

public class ConveyorPackageDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Package '" + other.gameObject.name + "' detected!");

        // Start the coroutine to remove the box after a delay
        StartCoroutine(RemoveBoxAfterDelay(other.gameObject));
    }

    IEnumerator RemoveBoxAfterDelay(GameObject box)
    {
        yield return new WaitForSeconds(2);
        Destroy(box);
    }
}
