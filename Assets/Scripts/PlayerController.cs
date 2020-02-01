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

    // Private Variables
    private float speed = 0.0f;

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
            if (speed > 0)
                speed -= brake * Time.deltaTime;
            else
                speed += brake * Time.deltaTime;
        }

        //Clamp the speed
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        //Compute a vector in the up direction of length speed
        Vector3 velocity = Vector3.forward * speed;

        //Move the player object
        transform.Translate(velocity * Time.deltaTime, Space.Self);

        //Press LShift to activate the turbo boost
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //  ActivateBoost();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
            speed = 0;
    }
}
