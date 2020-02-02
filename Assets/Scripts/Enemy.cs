using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private bool isAlive;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        isAlive = true;
        animator = GetComponent<Animator>();
        animator.SetBool("Die", false);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void Death()
    {
        if (isAlive)
            StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation()
    {
        if (!gameObject.activeInHierarchy)
            yield break;

        animator.SetBool("Die", true);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
