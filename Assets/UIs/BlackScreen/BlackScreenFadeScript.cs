using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackScreenFadeScript : MonoBehaviour
{
    public GameObject panel;
    public Animator animator;

    public void Fade()
    {
        StartCoroutine(Fade2());
    }
    private IEnumerator Fade2()
    {
        panel.SetActive(true);

        animator.SetBool("SetFadeBool", true);

        yield return new WaitForSeconds(4);

        panel.SetActive(false);
    }

    public void Unfade()
    {
        StartCoroutine(Unfade2());
    }

    private IEnumerator Unfade2()
    {
        panel.SetActive(true);

        animator.SetBool("SetFadeBool", false);

        yield return new WaitForSeconds(1.5f);

        panel.SetActive(false);

    }



}
