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
            playerMovement.moveSpeed = 0;

            Ray ray = binCamera.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                // Check if hit object has UUID
                UUIDGenerator uuidGenerator = hit.collider.GetComponent<UUIDGenerator>();
                if (uuidGenerator != null)
                {
                    //Debug.Log("Hovering over: " + uuidGenerator.GetUUID());
                    if (!playerPickupDrop.isHolding && Input.GetMouseButtonDown(0))
                    {
                        if (hit.transform.TryGetComponent(out ObjectGrabbable objectGrabbable))
                        {
                            playerPickupDrop.PickUpObject(objectGrabbable);
                            binCamera.gameObject.SetActive(false);
                            mainCamera.gameObject.SetActive(true);
                            binCamera.enabled = false;
                            mainCamera.enabled = true;
                            playerMovement.moveSpeed = originalPlayerMoveSpeed;
                        }
                    }
                }
            }
        }
    }
}
