using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour, ITrap
{
    public bool isTriggerable = false;
    public GameObject arrowTrapHolePrefab;
    public GameObject arrowTrapHole;
    public GameObject arrowPrefab;
    public float arrowSpeed = 1000f;
    public float arrowInterval = 1f;
    public float arrowLifetime = 5f;

    /// <summary>
    /// Will create an arrow hole and invoke a repeating function of FireArrowTrap
    /// </summary>
    void Start()
    {
        arrowTrapHole = Instantiate(arrowTrapHolePrefab, 
            new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            transform.rotation);
        arrowTrapHole.transform.parent = transform;
        
        if(!isTriggerable)
            InvokeRepeating(nameof(FireArrowTrap), 0f, arrowInterval);
    }

    /// <summary>
    /// Instantiate an arrow, provide it a slightly random trajectory and apply force to the arrow. Destroy the arrow
    /// at the end of its lifetime
    /// This could be made as an object pool for efficiency
    /// </summary>
    private void FireArrowTrap()
    {
        // Make the arrow prefab an object pool

        GameObject arrow = Instantiate(arrowPrefab, arrowTrapHole.transform.position, arrowTrapHole.transform.rotation  * Quaternion.Euler(-2.5f, 0f, 0f));
        Vector3 direction = arrow.transform.forward + new Vector3(
                                                                0, 
                                                                Random.Range(-0.05f, 0.05f), 
                                                                Random.Range(-0.05f, 0.05f));
        
        arrow.GetComponent<Rigidbody>().AddForce(direction  * arrowSpeed);
        arrow.transform.parent = transform;
        Destroy(arrow, arrowLifetime);
    }

    /// <summary>
    /// Cancels the Invoke function FireArrowTrap
    /// </summary>
    private void CancelArrowInvoke()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CancelInvoke(nameof(FireArrowTrap));
        }
    }

    /// <summary>
    /// If the trap is triggered and is not active, this will invoke the function FireArrowTrap
    /// </summary>
    public void ActivateTrapTrigger()
    {
        if(isTriggerable)
            InvokeRepeating(nameof(FireArrowTrap), 0f, arrowInterval);
        
        isTriggerable = false;
    }
}