using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    PlayerController controller;
    GameController gc;
    enum Meshes { BODY = 0, FARMOR = 1, LSPIKES = 2, RSPIKES = 3, FTURRET = 4, BTURRET = 5, THRUST = 6, ROOF = 7, HOOD = 8, TRUNK = 9 };
    bool[] activeComponents;
    public GameObject[] carComponents;
    public TextMeshProUGUI componentsText;

    // Start is called before the first frame update
    void Start()
    {
        controller = PlayerController.Instance;
        gc = GameController.Instance;

        //Activate all components
        //for (int i = 0; i < 10; i++)
        //  carComponents[i].SetActive(true);
        //DEBUG
        carComponents[(int)Meshes.FARMOR].SetActive(false);

        activeComponents = new bool[10];
        for (int i = 0; i < 10; i++)
            activeComponents[i] = true;

        //DEBUG
        activeComponents[(int)Meshes.FARMOR] = false;

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            controller.speed = 0.0f;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 direction = (transform.position - collision.gameObject.transform.position).normalized;
            Ray ray = new Ray(collision.gameObject.transform.position, direction);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Vector3 normal = hit.normal;

            if (normal == transform.forward) //Front Collision
            {
                if (carComponents[(int)Meshes.FARMOR].activeInHierarchy)
                {
                    Debug.Log("Hit Front Armor");
                    GetComponent<Rigidbody>().AddForce(transform.forward * -90.0f, ForceMode.Impulse);
                    collision.gameObject.GetComponent<Enemy>().Death();
                }
                else
                    TakeDamage();
            }
            else if (normal == -transform.forward) //Back Collision
            {
                if (carComponents[(int)Meshes.TRUNK].activeInHierarchy)
                {
                    Debug.Log("Hit Trunk");
                    GetComponent<Rigidbody>().AddForce(-transform.forward * -100.0f, ForceMode.Impulse);
                    collision.gameObject.GetComponent<Enemy>().Death();
                }
                else
                    TakeDamage();
            }
            else if (normal == transform.right) //Right Collision
            {
                if (carComponents[(int)Meshes.RSPIKES].activeInHierarchy)
                {
                    Debug.Log("Hit Right Spikes");
                    GetComponent<Rigidbody>().AddForce(transform.right * -90.0f, ForceMode.Impulse);
                    collision.gameObject.GetComponent<Enemy>().Death();
                }
                else
                    TakeDamage();
            }
            else if (normal == -transform.right) //Left Collision
            {
                if (carComponents[(int)Meshes.LSPIKES].activeInHierarchy)
                {
                    Debug.Log("Hit Left Spikes");
                    GetComponent<Rigidbody>().AddForce(-transform.right * -90.0f, ForceMode.Impulse);
                    collision.gameObject.GetComponent<Enemy>().Death();
                }
                else
                    TakeDamage();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            PickupLoot();
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage()
    {
        int index;
        int count = 0;
        for (int i = 0; i < 10; i++)
        {
            if (carComponents[i].activeInHierarchy)
                count++;
        }
        if (count < 2)
        {
            gc.GameOver();
            componentsText.text = "0 / 10";
            return;
        }

        do //Look for a component to destroy
        {
            index = Random.Range(1, 10);
        } while (!activeComponents[index]);

        Debug.Log("Removed: " + ((Meshes)index).ToString());
        activeComponents[index] = false;
        carComponents[index].SetActive(false);
        UpdateText();
    }

    private void UpdateText()
    {
        int count = 0;
        for (int i = 0; i < 10; i++)
        {
            if (carComponents[i].activeInHierarchy)
                count++;
        }

        componentsText.text = count.ToString() + " / 10";
    }

    private void PickupLoot()
    {
        Debug.Log("Picked up");
        int index;
        int count = 0;
        for (int i = 0; i < 10; i++)
        {
            if (carComponents[i].activeInHierarchy)
                count++;
        }
        if (count == 10)
        {
            Debug.Log("You already have all components");
            return;
        }

        do //Look for a component to recover
        {
            index = Random.Range(1, 10);
        } while (activeComponents[index]);

        Debug.Log("Recovered: " + ((Meshes)index).ToString());
        activeComponents[index] = true;
        carComponents[index].SetActive(true);
        UpdateText();
    }
}
