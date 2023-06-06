using System;
using UnityEngine;

public class SecretBook : MonoBehaviour
{
    private Animator parentAnim;
    private static readonly int slide = Animator.StringToHash("Slide");

    private void Awake()
    {
        parentAnim = transform.parent.GetComponent<Animator>();
    }
    
    /// <summary>
    /// Sets the parent animations "Slide" trigger on
    /// </summary>
    public void SlideBookcase()
    {
        parentAnim.SetTrigger(slide);
    }
}
