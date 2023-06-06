using UnityEngine;


public class PlayerFX : MonoBehaviour
{
    [SerializeField] private GameObject dustFX;


    public void PlayDustFX()
    {
        GameObject dust = Instantiate(dustFX, transform.position, Quaternion.Euler(-90,0,0));
        Destroy(dust, 1f);
    }
}
