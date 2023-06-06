using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class Bookcase : MonoBehaviour, IInteractable
{
    public GameObject activatedBook;

    private Animator anim;
    private Animator bookAnim;
    private static readonly int bookTrigger = Animator.StringToHash("bookTrigger");

    public InteractableType InteractableType { get; set; }
    public bool IsActivated { get; set; }

    private void Awake()
    {
        InteractableType = InteractableType.Collect;
        activatedBook = transform.GetChild(0).gameObject;
        bookAnim = activatedBook.GetComponent<Animator>();
        IsActivated = false;
    }
    
    /// <summary>
    /// When interacted with, set the bookTrigger within the animator
    /// </summary>
    public void InteractOn(ref InteractableType _iType)
    {
        if (IsActivated)
            return;

        _iType = InteractableType;
        IsActivated = true;
        // Trigger book animation here
        bookAnim.SetTrigger(bookTrigger);
    }

    public void InteractOff() {}
}
