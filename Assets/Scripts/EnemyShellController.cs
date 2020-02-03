using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShellController : MonoBehaviour
{
    public GameObject shellExplosion;
    public AudioClip shellExplosionSound;
    public float shellSpeed = 40f;
    public int shellDamage = 1;
    public float shellTTL = 5f;

    private Rigidbody shellRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        shellRigidbody = GetComponent<Rigidbody>();
        shellRigidbody.velocity = transform.forward * shellSpeed;       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("ShellController - " + turretRenderer.gameObject.name);

        //Destroy(gameObject, shellTTL); // guarantees destruction of object after its lifetime expires
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.collider, true);
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
        //AudioController.Instance.PlayShellExplosion();
        float totalPlayback = explosion.GetComponent<ParticleSystem>().main.duration + explosion.GetComponent<ParticleSystem>().main.startLifetime.constant;
        Destroy(explosion, totalPlayback);
        Destroy(gameObject);
        //Debug.Log("Destroy shell called");
    }
}
