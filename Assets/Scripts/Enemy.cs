using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private bool isAlive;
    public int hp;

    
    public Transform shootPoint;
    public GameObject enemyShell;
    public float moveSpeed = 5f;
    public float rotateSpeed = 2f;
    public float bulletOffset = 4f;
    public float rateOfFire = 0;
    public float maxPlayerDistance = 30f;

    private float fireDelay = 2f;
    private GameObject player;
    private float minSpeed = 5f;
    private float maxSpeed = 10f;
    private float maxDistance = 120f;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
        hp = 2;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            moveSpeed = minSpeed + ((distance * maxSpeed) / maxDistance);
            transform.LookAt(player.transform.position);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            if (distance > maxPlayerDistance)
            {
                float oldY = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, oldY, transform.position.z);
            }
            fireDelay -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && isAlive)
        {
            if (fireDelay <= 0)
            {
                AudioController.Instance.PlayCharge();
                StartCoroutine(ShootAnimation());
                fireDelay = 2f;
            }
        }
    }

    private void OnEnable()
    {
        isAlive = true;
        hp = 2;
        animator = GetComponent<Animator>();
        animator.SetBool("Die", false);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void Death()
    {
        if (isAlive)
        {
            isAlive = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(DeathAnimation());
        }
    }

    public void BulletHit()
    {
        hp--;
        if (hp == 0)
            Death();
    }

    private IEnumerator DeathAnimation()
    {
        if (!gameObject.activeInHierarchy)
            yield break;

        
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(3);
        gameObject.transform.parent.gameObject.SetActive(false);
        
    }

    private IEnumerator ShootAnimation()
    {
        if (!gameObject.activeInHierarchy)
            yield break;

        animator.SetBool("Shoot", true);
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("Shoot", false);
        if (isAlive)
        {
            Instantiate(enemyShell, shootPoint.transform.position + (bulletOffset * shootPoint.transform.forward), shootPoint.transform.rotation);
            AudioController.Instance.PlayEnemyShot();
        }
        
    }
}
