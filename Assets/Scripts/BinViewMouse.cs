using System.Collections;
using TMPro;
using UnityEngine;

public class BinViewMouse : MonoBehaviour
{
    public RectTransform indicatorRectTransform;
    public float sensitivity = 3000f;
    public Camera binCamera;
    public Camera mainCamera;
    private float originalPlayerMoveSpeed;

    public Vector2 minXMaxY = new Vector2(-350, 350); // Min and Max X values
    public Vector2 minYMaxY = new Vector2(-210, 210); // Min and Max Y values

    private Vector2 indicatorPosition;

    private PlayerPickupDrop playerPickupDrop;
    private PlayerMovement playerMovement;

    public GameObject infoText;
    public float delayBeforeDisabling = 2f;

    public GameObject UUIDHoverText;

    void Start()
    {
        if (!binCamera)
        {
            binCamera = GameObject.FindGameObjectWithTag("BinCamera").GetComponent<Camera>();
        }

        if (!mainCamera)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        indicatorRectTransform = this.GetComponent<RectTransform>();
        indicatorPosition = indicatorRectTransform.anchoredPosition;

        GameObject playerObject = GameObject.Find("Player");
        playerPickupDrop = playerObject.GetComponent<PlayerPickupDrop>();
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        originalPlayerMoveSpeed = playerMovement.GetMoveSpeed();
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        indicatorPosition += new Vector2(mouseX, mouseY);

        indicatorPosition.x = Mathf.Clamp(indicatorPosition.x, minXMaxY.x, minXMaxY.y);
        indicatorPosition.y = Mathf.Clamp(indicatorPosition.y, minYMaxY.x, minYMaxY.y);

        indicatorRectTransform.anchoredPosition = indicatorPosition;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(binCamera, indicatorRectTransform.position);

        // Cast a ray from indicator downwards
        if (binCamera.isActiveAndEnabled)
        {
            // Remove info text after delay
            StartCoroutine(HideInfoText());

            // Disable player movement
            playerMovement.moveSpeed = 0;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ExitBinView();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("2 was pressed");
            }

            Ray ray = binCamera.ScreenPointToRay(screenPoint);

            float maxDistance = 100f;
            float currentDistance = 0f;

            while (currentDistance < maxDistance)
            {
                if (Physics.Raycast(ray.origin + ray.direction * currentDistance, ray.direction, out RaycastHit hit, maxDistance - currentDistance))
                {
                    if (hit.collider.GetComponent<Renderer>()?.enabled == true) // Check if renderer is enabled
                    {
                        ProcessHit(hit);
                        break;
                    }
                    else
                    {
                        currentDistance += hit.distance + 0.01f; // Move the start point beyond the last hit object
                    }
                }
                else
                {
                    break; // No more objects hit
                }
            }
        }
    }

    private void ExitBinView()
    {
        StopCoroutine(HideInfoText());
        binCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        binCamera.enabled = false;
        mainCamera.enabled = true;
        playerMovement.moveSpeed = originalPlayerMoveSpeed;
    }

    private void PickupAndSwitchViews(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out ObjectGrabbable objectGrabbable))
        {
            playerPickupDrop.PickUpObject(objectGrabbable);
            ExitBinView();
        }
    }

    private IEnumerator HideInfoText()
    {
        yield return new WaitForSeconds(delayBeforeDisabling);
        infoText.SetActive(false);
    }

    private void DisplayHoverUUID(RaycastHit hit, UUIDGenerator uuidGenerator)
    {
        var packageBounds = hit.collider.GetComponent<Collider>().bounds;
        Vector3 targetPosition = new Vector3(packageBounds.center.x, packageBounds.max.y + 0.01f, packageBounds.center.z);
        UUIDHoverText.SetActive(true);
        UUIDHoverText.GetComponent<TextMeshProUGUI>().text = uuidGenerator.GetUUID();
        UUIDHoverText.transform.position = targetPosition;
    }

    void ProcessHit(RaycastHit hit)
    {
        Debug.Log(hit.collider.name);
        UUIDGenerator uuidGenerator = hit.collider.GetComponent<UUIDGenerator>();
        if (uuidGenerator != null)
        {
            DisplayHoverUUID(hit, uuidGenerator);

            if (!playerPickupDrop.isHolding && Input.GetMouseButtonDown(0))
            {
                PickupAndSwitchViews(hit);
            }
        }
        else
        {
            UUIDHoverText.SetActive(false);
        }
    }
}
