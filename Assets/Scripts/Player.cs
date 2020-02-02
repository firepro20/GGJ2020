using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerController controller;
    enum Meshes { BODY = 0, FARMOR = 1 };

    //Functional Components
    public GameObject thruster;
    public GameObject frontArmor;
    public GameObject leftSpikes;
    public GameObject rightSpikes;
    public GameObject frontTurret;
    public GameObject backTurret;

    //Aesthetic Components
    GameObject roof;
    GameObject hood;
    GameObject trunk;
    GameObject glass;

    // Start is called before the first frame update
    void Start()
    {
        controller = PlayerController.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
            controller.speed = 0.0f;
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 direction = (transform.position - collision.gameObject.transform.position).normalized;
            Ray ray = new Ray(collision.gameObject.transform.position, direction);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Vector3 normal = hit.normal;

            if (normal == transform.forward && frontArmor.activeInHierarchy) //Front
                collision.gameObject.GetComponent<Enemy>().Death();
            else if (normal == -transform.forward) //Back
                Debug.Log("Hit Trunk");
            else if (normal == transform.right) //Right
                Debug.Log("Hit Right Spikes");
            else if (normal == -transform.right) //Left
                Debug.Log("Hit Left Spikes");
        }
    }
}
