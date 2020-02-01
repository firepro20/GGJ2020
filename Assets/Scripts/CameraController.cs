using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /* Singleton*/
    private static CameraController instance;
    public static CameraController Instance { get { return instance; } }

    // Class References

    // Public Variables
    public GameObject player;

    // Private Variables

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
    void LateUpdate()
    {
        //Save the camera current position/rotation
        Quaternion currentRotation = transform.rotation;
        //Reset the player's rotation
        transform.rotation = Quaternion.identity;

        //Reposition the camera on top of the player
        transform.position = player.transform.position;
        transform.Translate(0, 30, -17);

        //Rotate the camera back
        transform.rotation = currentRotation;
    }
}
