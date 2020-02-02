using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private bool isAlive;
    public int hp;

    void Awake()
    {
        animator = GetComponent<Animator>();
        hp = 2;
    }

    // Update is called once per frame
    void Update()
    {

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
            StartCoroutine(DeathAnimation());
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
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        isAlive = false;
    }
}
