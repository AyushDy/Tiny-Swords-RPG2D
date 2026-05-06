using System.Collections;
using UnityEngine;

public class EnemyHitFlash : MonoBehaviour
{
    private SpriteRenderer sr;
    private MaterialPropertyBlock mpb;

    private Animator anim;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        anim = GetComponent<Animator>();
    }


    public void PlayHitEffect()
    {
        StopAllCoroutines();
        StartCoroutine(HitEffectRoutine());
    }

    IEnumerator HitEffectRoutine()
    {
        float duration = 0.1f;
        float time = 0f;

        float originalAnimSpeed = 1f;
        Vector2 originalVelocity = Vector2.zero;

        if(anim != null)
        {
            originalAnimSpeed = anim.speed;
            anim.speed = 0f;
        }

        while (time < duration)
        {
            float t = 1f - (time / duration);
            sr.GetPropertyBlock(mpb);
            mpb.SetFloat("_FlashAmount", t);
            sr.SetPropertyBlock(mpb);

            time += Time.deltaTime;
            yield return null;
        }

        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_FlashAmount", 0f);
        sr.SetPropertyBlock(mpb);

        if(anim != null)
        {
            anim.speed = originalAnimSpeed;
        }
    }
}
