using UnityEngine;

public class BinViewMouse : MonoBehaviour
{
    public RectTransform indicatorRectTransform; // Assign your UI indicator's RectTransform
    public float sensitivity = 3000f; // Increase sensitivity as needed
    public Camera binCamera;
    public Camera mainCamera;

    public Vector2 minXMaxY = new Vector2(-350, 350); // Min and Max X values
    public Vector2 minYMaxY = new Vector2(-210, 210); // Min and Max Y values

    private Vector2 indicatorPosition;

    void Start()
    {
        // Find BinCamera by tag
        if (!binCamera)
        {
            binCamera = GameObject.FindGameObjectWithTag("BinCamera").GetComponent<Camera>();
        }

        if (!mainCamera)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        // Initialize indicatorPosition based on the current indicator position
        indicatorRectTransform = this.GetComponent<RectTransform>();
        indicatorPosition = indicatorRectTransform.anchoredPosition;
    }

    void Update()
    {
        // Get raw mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        // Update indicatorPosition based on mouse input
        indicatorPosition += new Vector2(mouseX, mouseY);

        // Clamp the indicatorPosition to ensure it stays within the bounding box
        indicatorPosition.x = Mathf.Clamp(indicatorPosition.x, minXMaxY.x, minXMaxY.y);
        indicatorPosition.y = Mathf.Clamp(indicatorPosition.y, minYMaxY.x, minYMaxY.y);

        // Apply the clamped position to the indicator
        indicatorRectTransform.anchoredPosition = indicatorPosition;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(binCamera, indicatorRectTransform.position);

        // Cast a ray from indicator downwards
        Ray ray = binCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            // Check if hit object has UUID
            UUIDGenerator uuidGenerator = hit.collider.GetComponent<UUIDGenerator>();
            if (uuidGenerator != null)
            {
                Debug.Log("Hovering over: " + uuidGenerator.GetUUID());

                if (Input.GetMouseButtonDown(0))
                {
                    hit.collider.gameObject.SetActive(false);
                    binCamera.gameObject.SetActive(false);
                    mainCamera.gameObject.SetActive(true);
                }
            }
        }
    }
}
