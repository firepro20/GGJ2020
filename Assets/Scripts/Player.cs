using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    PlayerController controller;
    GameController gc;
    enum Meshes { BODY = 0, FARMOR = 1, LSPIKES = 2, RSPIKES = 3, THRUST = 4, ROOF = 5, HOOD = 6, TRUNK = 7, FTURRET = 8, BTURRET = 9 };
    bool[] activeComponents;
    public GameObject[] carComponents;
    public TextMeshProUGUI componentsText;

    private float componentTimer = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = PlayerController.Instance;
        gc = GameController.Instance;

        //Activate all components
        for (int i = 0; i < 10; i++)
          carComponents[i].SetActive(true);

        activeComponents = new bool[10];
        for (int i = 0; i < 10; i++)
            activeComponents[i] = true;

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        //Take damage and lose component over time
        componentTimer -= Time.deltaTime;
        if (componentTimer <= 0)
        {
            TakeDamage();
            componentTimer = 10.0f;
        }
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
        else if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            PickupLoot();
            gc.DecreaseBagCount();
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
            if (count >= 4)
                index = Random.Range(1, 8);
            else if (count == 3)
                index = (int)Meshes.BTURRET;
            else
                index = (int)Meshes.FTURRET;
            
            //index = Random.Range(1, 10);
        } while (!activeComponents[index]);

        //Debug.Log("Removed: " + ((Meshes)index).ToString());
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
        int index;
        int count = 0;
        for (int i = 0; i < 10; i++)
        {
            if (carComponents[i].activeInHierarchy)
                count++;
        }
        if (count == 10)
        {
            //Debug.Log("You already have all components");
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
