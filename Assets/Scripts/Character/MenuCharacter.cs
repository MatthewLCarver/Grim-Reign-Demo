using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCharacter : MonoBehaviour
{
    private Animator animator;
    private int animationPlays = 0;
    private Vector3 originalRotation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        originalRotation = transform.rotation.eulerAngles;
    }
    
    public void IncrementAnimationPlays()
    {
        animationPlays++;
        SetAnimationPlays();
    }

    public void ResetAnimationPlays()
    {
        animationPlays = 0;
        SetAnimationPlays();
    }

    private void SetAnimationPlays()
    {
        animator.SetInteger("AnimationPlays", animationPlays);
    }
    
    public void MenuButtonClicked()
    {
        bool inMenu = animator.GetBool("InMenu");
        animator.SetBool("InMenu", !inMenu);
        StartCoroutine(RotateCharacter());
    }

    private IEnumerator RotateCharacter()
    {
        float time = 0;
        // If the character is in the original rotation, rotate it to the menu rotation accounting for floating point errors
        if (Math.Abs(transform.rotation.eulerAngles.y - originalRotation.y) < 0.01f)
        {
            while (time < .5f)
            {
                time += Time.deltaTime;
                transform.rotation = 
                    Quaternion.Lerp(transform.rotation,
                        Quaternion.Euler(new Vector3(359,10,359)), 
                        time * 2);
                yield return null;
            }
        }
        else
        {   while (time < .5f)
            {
                time += Time.deltaTime;
                transform.rotation = 
                    Quaternion.Lerp(transform.rotation, 
                        Quaternion.Euler(originalRotation), 
                        time * 2);
                yield return null;
            }
        }
    }
}
