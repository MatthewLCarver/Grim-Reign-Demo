using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{

    public float movementSpeed = 5f;

    private CharacterController cc;
    private Animator anim;
    private AvatarMask mask;
    private Vector2 moveInput;

    public Vector3 velocity = Vector3.zero;
    public bool jumpInput;
    public float jumpVelocity = 10f;
    public float jumpHeight = 4f;

    public bool isGrounded = true;
    public bool isDiving;
    public bool isPushing;
    public bool isCollecting;

    private Camera cam;

    private Ragdoll ragdoll;

    // Cached properties string lookups without the overhead
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Dive = Animator.StringToHash("Dive");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int collecting = Animator.StringToHash("IsCollecting");
    private static readonly int pushing = Animator.StringToHash("IsPushing");
    private static readonly int isLeft = Animator.StringToHash("IsLeft");

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        cam = Camera.main;
        
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            CharacterRagdoll();
        }

        SetMovementAnimation();
        ProcessActionAnimations();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GetComponent<Player>().ResetColorScheme();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    /// <summary>
    /// Smoothly transition between animation layers based on boolean values
    /// </summary>
    private void ProcessActionAnimations()
    {
        if (isPushing || isCollecting)
        {
            SetAnimationActiveLayer(anim, 1, 1, Time.fixedDeltaTime, 10);
            //SetAnimationActiveLayer(anim, 2, 1, Time.fixedDeltaTime, 10);
            //m_virtualCamera.Priority = 11;
        }
        else
        {
            //CasualMovement(Time.fixedDeltaTime);
            SetAnimationActiveLayer(anim, 1, 0, Time.fixedDeltaTime, 10);
            //SetAnimationActiveLayer(anim, 2, 0, Time.fixedDeltaTime, 10);
            //m_virtualCamera.Priority = 9;
        }
    }


    void FixedUpdate()
    {
        // find the horizontal unit vector facing forward from the camera
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        // use our camera's right vector, which is always horizontal
        Vector3 camRight = cam.transform.right;
        
        Vector3 delta = (moveInput.x * camRight + moveInput.y * camForward) * movementSpeed;

        if (isGrounded || moveInput.x != 0 || moveInput.y != 0)
        {
            velocity.x = delta.x;
            velocity.z = delta.z;
        }

        // calculate jump acceleration required to reach jump height
        float time = (jumpHeight * 2) / (0 + jumpVelocity);
        float acceleration = (0 - jumpVelocity) / time;
        acceleration = -acceleration;

        if (isGrounded)
        {
            // check for jumping
            if(jumpInput)
            {
                velocity.y = jumpHeight;
                jumpInput = false;
            }
            
            // check if we've hit ground from falling. If so, remove our velocity
            if (velocity.y <= -0.1635f)
            {
                velocity.y = 0;
            }
        }


        // apply gravity after zeroing velocity so we register as grounded still
        velocity += Physics.gravity * Time.fixedDeltaTime;

        cc.Move(velocity * Time.deltaTime);
        isGrounded = cc.isGrounded;
        anim.SetBool(IsGrounded, isGrounded);
    }


    /// <summary>
    /// When colliding with an ITrap object, activate the trap
    /// </summary>
    /// <param name="_other"></param>
    private void OnCollisionEnter(Collision _other)
    {
        ITrap trap = _other.collider.GetComponent<ITrap>();

        if (trap == null)
            return;
        trap.ActivateTrapTrigger();
    }
    
    /// <summary>
    /// Used to trigger Checkpoints
    /// </summary>
    /// <param name="_other"></param>
    private void OnTriggerEnter(Collider _other)
    {
        ITrap trap = _other.GetComponent<ITrap>();

        if (trap == null)
            return;
        trap.ActivateTrapTrigger();
    }


    /// <summary>
    /// Called through the Player Input component event and converts the input context into a Vector2 to register
    /// the player's movement 
    /// </summary>
    /// <param name="_context"></param>
    public void OnMove(InputAction.CallbackContext _context)
    {
        if (isDiving || isPushing || isCollecting)
            return;
        
        if(_context.started || _context.performed)
            moveInput = _context.ReadValue<Vector2>();
        else if(_context.canceled)
            moveInput = Vector2.zero;
    }
    
    /// <summary>
    /// Called through the Player Input component event and converts the input context into a Vector2 to register
    /// the player's control of the camera 
    /// </summary>
    /// <param name="_context"></param>
    public void OnLook(InputAction.CallbackContext _context)
    {
        if(cc.enabled == false)
            return;
        float newX = _context.ReadValue<Vector2>().x;
        newX /= 15;


        transform.Rotate(0,  newX, 0);
    }
    
    /// <summary>
    /// Called through the Player Input component event and checks the input context and activates the player's jump
    /// boolean 
    /// </summary>
    /// <param name="_context"></param>
    public void OnJump(InputAction.CallbackContext _context)
    {
        if (isPushing || isCollecting)
            return;
        
        if (_context.started)
        {
            jumpInput = true;
            anim.SetTrigger(IsJumping);
        }
        else if(_context.canceled)
            jumpInput = false;
    }
    
    /// <summary>
    /// Called through the Player Input component event and thecks the input context and activates the player's dive
    /// boolean and creates the dive coroutine 
    /// </summary>
    /// <param name="_context"></param>
    public void OnDive(InputAction.CallbackContext _context)
    {
        if (isPushing || isCollecting)
            return;
        
        if (_context.started &&
            !isDiving)
        {
            anim.SetTrigger(Dive);
            StartCoroutine(DiveMovementInhibitor());
        }
    }
    
    
    /// <summary>
    /// Will set the appropriate floats in the Animator to control animations for movement
    /// </summary>
    private void SetMovementAnimation()
    {
        // set the animator's movement values
        anim.SetFloat(Horizontal, moveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat(Vertical, moveInput.y, 0.1f, Time.deltaTime);
    }

    
    /// <summary>
    /// Turns off the Ragdoll and re-activates the Character Controller
    /// </summary>
    public void CharacterRespawn()
    {
        cc.enabled = true;
        ragdoll.ragdollOn = false;
    }
    
    /// <summary>
    /// Turns on the Ragdoll and turns off the Character Controller
    /// </summary>
    public void CharacterRagdoll()
    {
        cc.enabled = false;
        ragdoll.ragdollOn = true;
    }
    
    /// <summary>
    /// Pauses movement for the duration of the roll and resets the moveInput Vector2 and resets animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator DiveMovementInhibitor()
    {
        isDiving = true;
        yield return new WaitForSeconds(1f);
        isDiving = false;
        
        moveInput = Vector2.zero;
        SetMovementAnimation();
    }
    
    /// <summary>
    /// Sets the layer weight over time when performing actions that require animations on a different layer to the base
    /// </summary>
    /// <param name="_animator"></param>
    /// <param name="_layer"></param>
    /// <param name="_transitionValue"></param>
    /// <param name="_dt"></param>
    /// <param name="_rateOfChange"></param>
    private void SetAnimationActiveLayer(Animator _animator, int _layer, int _transitionValue, float _dt, float _rateOfChange)
    {
        _animator.SetLayerWeight(_layer, Mathf.Lerp(_animator.GetLayerWeight(_layer), _transitionValue, _dt * _rateOfChange));
    }

    /// <summary>
    /// Sets the appropriate animation boolean for the different animation layer to play
    /// </summary>
    /// <param name="_type"></param>
    public void SetInteractableTypeAnimation(InteractableType _type)
    {
        switch (_type)
        {
            case InteractableType.Collect:
                isCollecting = true;
                anim.SetBool(isLeft, false);
                anim.SetBool(collecting, isCollecting);
                break;
            
            case InteractableType.CollectLeft:
                isCollecting = true;
                anim.SetBool(isLeft, true);
                anim.SetBool(collecting, isCollecting);
                break;
            
            case InteractableType.Push:
                isPushing = true;
                anim.SetBool(pushing, isPushing);
                break;
        }
    }

    public void ResetAnimationLayerBooleans()
    {
        isCollecting = false;
        isPushing = false;
        
        anim.SetBool(collecting, isCollecting);
        anim.SetBool(pushing, isPushing);
    }
}
