using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutAnimator : MonoBehaviour
{
    public static BlackoutAnimator instance;
    private void Awake()
    {
        instance = this;
    }

    public Animator anim;

    public void FadeToBlack()
    {
        anim.SetBool("Blackout", true);
    }

    public void FadeFromBlack()
    {
        anim.SetBool("Blackout", false);
    }
}
