using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Transition2
{
    public MonoBehaviour monoBehaviour;
    public Animator animator;

    public Transition2(MonoBehaviour monoBehaviour, Animator animator)
    {
        this.monoBehaviour = monoBehaviour;
        this.animator = animator;
    }

    public void PlayTransition(
        String animation,
        int before,
        int after,
        Action callback)
    {
        monoBehaviour.StartCoroutine(Transition(animation, before, after, callback));
    }

    private IEnumerator Transition(
        String animation,
        int before, 
        int after, 
        Action onFinish)
    {
        yield return new WaitForSeconds(before);

        animator.Play(animation);
        while (true)
        {
            AnimatorStateInfo anim = animator.GetCurrentAnimatorStateInfo(0);
            if (anim.IsName(animation) && anim.normalizedTime >= 1.0f) break;
            yield return null;
        }

        yield return new WaitForSeconds(after);
        onFinish();
    }
}
