using Unity.Mathematics;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public KeyCode castKeybind, directionKeybind;
    public float range;
    public GameObject iceWallPreview, iceWallOBJ;
    public LayerMask layerMask;
    private bool direction, casting;

    public float speed = 10.0f;
    private float translation;
    private float straffe;

    public float lookSpeed = 3;
    public GameObject bullet;
    private Vector2 rotation = Vector2.zero;

    private Transform cam;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        rb.WakeUp();
    }

    // Update is called once per frame
    void Update()
    {
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(straffe, 0, translation);

        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -40f, 40f);
        transform.eulerAngles = new Vector2(0, rotation.y) * lookSpeed;
        cam.localRotation = Quaternion.Euler(rotation.x * lookSpeed, 0, 0);

        // Fire bullet using the right mouse button (mouse button 1)
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(bullet, cam.position + cam.forward, cam.rotation);
        }

        // Toggle casting with custom keybind
        if (Input.GetKeyDown(castKeybind))
        {
            casting = !casting;
            if (!casting) { iceWallPreview.SetActive(false); }
        }

        // If casting, initiate wall placement
        if (casting) { CastingWall(); }
    }

    // Wall casting method
    void CastingWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, range, layerMask))
        {
            if (!iceWallPreview.activeSelf) { iceWallPreview.SetActive(true); }

            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            if (direction) { rotation.y = 1; }
            else { rotation.y = 0; }

            iceWallPreview.transform.localRotation = rotation;
            iceWallPreview.transform.position = hit.point;

            // Cast Ice Wall with left mouse button (mouse button 0)
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(iceWallOBJ, hit.point, iceWallPreview.transform.rotation);
                casting = false;
                iceWallPreview.SetActive(false);
            }
        }
        else { iceWallPreview.SetActive(false); }

        // Toggle direction with custom keybind
        if (Input.GetKeyDown(directionKeybind)) { direction = !direction; }
    }
}
