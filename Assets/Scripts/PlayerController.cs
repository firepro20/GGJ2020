using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Singleton*/
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }

    // Class References

    // Public Variables
    public float turnSpeed = 30.0f;
    public float acceleration = 10.0f;
    public float brake = 5.0f;
    public float maxSpeed = 10.0f;

    public RectTransform crosshair;
    public Turret frontTurret;
    public Turret rearTurret;
    public GameObject TireBL;
    public GameObject TireBR;
    public GameObject TireFL;
    public GameObject TireFR;

    // Private Variables
    public float speed = 0.0f;

    void Awake()
    {
        /* Singleton */
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        /* End of Singleton */
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //The vertical axis controls the acceleration
        float forward = Input.GetAxis("Vertical");
        //The horizontal axis controls the turn
        float turn = Input.GetAxis("Horizontal");

        //Increase or decrease the player movement speed
        if (forward > 0)
        {
            //Turn the car in the movement direction
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            //Increase the car speed
            speed += acceleration * Time.deltaTime;
        }
        else if (forward < 0)
        {
            //Turn the car in the movement direction
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            //Decrease the car speed
            speed -= acceleration * Time.deltaTime;
        }
        else //Braking
        {
            if (speed > 0.1f)
                speed -= brake * Time.deltaTime;
            else if (speed < -0.1f)
                speed += brake * Time.deltaTime;
            else
                speed = 0.0f;
        }
        
        //Clamp the speed
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        //Compute a vector in the up direction of length speed
        Vector3 velocity = Vector3.forward * speed;

        //Move the player object
        transform.Translate(velocity * Time.deltaTime, Space.Self);
        RotateTires();

        //Press LShift to activate the turbo boost
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //  ActivateBoost();

        MoveCrosshair();
    }

    private void RotateTires()
    {
        Quaternion newRotation = Quaternion.AngleAxis(speed * 0.2f, Vector3.right);
        TireBL.transform.rotation *= newRotation;

        newRotation = Quaternion.AngleAxis(-speed * 0.2f, -Vector3.right);
        TireBR.transform.rotation *= newRotation;

        TireFL.transform.rotation *= newRotation;
        TireFR.transform.rotation *= newRotation;
    }

    // Moves crosshair and rotates turrets
    private void MoveCrosshair()
    {
        crosshair.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // x z

        // Turret rotates towards mouse pointer
        frontTurret.transform.rotation = Quaternion.Euler(0, 90, 0);
        frontTurret.transform.rotation = Quaternion.Euler(0f, 90 - GetTurretRotation(frontTurret.transform), 0f);

        float angle = frontTurret.transform.rotation.y;

        rearTurret.transform.rotation = Quaternion.Euler(0, 90, 0);
        rearTurret.transform.rotation = Quaternion.Euler(0f, 90 - GetTurretRotation(rearTurret.transform), 0f);
    }

    private float GetTurretRotation(Transform turretTransform)
    {
        // Calculate difference between mouse and turret
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float rotationY = 0.0f;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Vector3 crossHairWorldPoint = hit.point;
                crossHairWorldPoint.y = 2.5f;
                Vector3 difference = crossHairWorldPoint - turretTransform.position;
                rotationY = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg;
                return rotationY;
            }
        }

        return rotationY;
    }
}
