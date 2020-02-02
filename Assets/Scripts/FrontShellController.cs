using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontShellController : MonoBehaviour
{
    public GameObject shellExplosion;
    public AudioClip shellExplosionSound;
    public float shellSpeed = 5f;
    public int shellDamage = 25;
    public float shellTTL = 5f;
    private Vector3 turretPos;

    private Rigidbody shellRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        turretPos = PlayerController.Instance.transform.position + new Vector3(0.13f, 1.29f, 1.8f);
        shellRigidbody = GetComponent<Rigidbody>();
        Vector3 direction = EvaluateDirection();
        if (direction == Vector3.zero)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            shellRigidbody.velocity = direction * (shellSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("ShellController - " + turretRenderer.gameObject.name);

        Destroy(gameObject, shellTTL); // guarantees destruction of object after its lifetime expires
    }

    private void FixedUpdate()
    {
        // This is not a permanent solution as there may be times this will not work. To improve later
        if (!OnScreenCheck()) // we put this here as there was a bug were this was destroying object before instantiating, because
        {
            DestroyShell();
            //Destroy(gameObject, shellTTL);
        }   // render on camera is off before initiating.


    }

    private Vector3 EvaluateDirection()
    {
        // Calculate difference between mouse and turret
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 difference = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Vector3 result = hit.point;
                result.y = 2.5f;
                difference = result - turretPos;
                difference = difference.normalized;
            }
        }
        if (Vector3.Distance(hit.point, PlayerController.Instance.transform.position) < 1f)
        {
            difference = Vector3.zero;
        }
        return difference;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>(), true);
            return;
        }

        DestroyShell();


    }

    private bool OnScreenCheck()
    {
        // checks if visible by any camera, even Editor.
        if (!gameObject.GetComponent<MeshRenderer>().isVisible)
        {
            Destroy(gameObject);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DestroyShell()
    {
        // Destroy the bullet, instantiate bulletExplosion prefab, before destroying the component
        GameObject explosion = Instantiate(shellExplosion, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        AudioController.Instance.PlayShellExplosion();
        float totalPlayback = explosion.GetComponent<ParticleSystem>().main.duration + explosion.GetComponent<ParticleSystem>().main.startLifetime.constant;
        Destroy(explosion, totalPlayback);
        Destroy(gameObject);
        //Debug.Log("Destroy shell called");
    }
}
