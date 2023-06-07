using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public struct TransformData
{
    public Transform transform;
    public Vector3 position;
    public Quaternion rotation;

    public TransformData(Transform _transform)
    {
        transform = _transform;
        position = _transform.position;
        rotation = _transform.rotation;
    }
}

public class Respawner : MonoBehaviour
{
    public Vector3 respawnPosition;
    public Quaternion respawnRotation;
    
    public Transform rootTransform;
    public List<TransformData> transformDataArray;
    private PlayerInputHandler playerInputHandler;

    private void Awake()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCheckPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.L))
        {
            RespawnCharacter();
        }
    }

    /// <summary>
    /// Respawns the Player back to their respawn position/checkpoint position
    /// </summary>
    private void RespawnCharacter()
    {
        playerInputHandler.CharacterRagdoll();
        transform.position = respawnPosition;
        transform.rotation = respawnRotation;
        Respawn(rootTransform);
        playerInputHandler.CharacterRespawn();
        SaveRespawnTransform(rootTransform);
        /*Quaternion temp = Quaternion.Euler(70, 0, 0);
        GetComponent<Player>().GetWeaponSocket().transform.rotation = temp;*/
    }

    /// <summary>
    /// Captures the initial position of the player and sets the new respawn position and rotation
    /// </summary>
    public void SetCheckPoint()
    {
        if(transformDataArray != null)
            transformDataArray.Clear();
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
        SaveRespawnTransform(rootTransform);
        
        //xText.text = respawnPosition.ToString();
    }
    
    /// <summary>
    /// Captures the checkpoint position and rotation of the player to the respawn position and rotation
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_rotation"></param>
    public void SetCheckPoint(Vector3 _position, Quaternion _rotation)
    {
        if(transformDataArray != null)
            transformDataArray.Clear();
        respawnPosition = _position;
        respawnRotation = _rotation;
        SaveRespawnTransform(rootTransform);
        
        //xText.text = respawnPosition.ToString() + " " + respawnRotation.eulerAngles.ToString();
    }
    
    /// <summary>
    /// Captures the transform of the Player parts, recursively, from the Root
    /// </summary>
    /// <param name="_transform"></param>
    public void SaveRespawnTransform(Transform _transform)
    {
        if (transformDataArray == null || transformDataArray.Count == 0)
            transformDataArray = new List<TransformData>();
        transformDataArray.Add(new TransformData(_transform));
        foreach(Transform child in _transform)
        {
            SaveRespawnTransform(child);
        }
    }
    
    /// <summary>
    /// Sets the transforms of the Player parts, recursively, from the transformDataArray
    /// </summary>
    /// <param name="_transform"></param>
    public void Respawn(Transform _transform)
    {
        _transform.position = transformDataArray.Find(x => x.transform == _transform).position;
        _transform.rotation = transformDataArray.Find(x => x.transform == _transform).rotation;
        transformDataArray.Remove(transformDataArray.Find(x => x.transform == _transform));
        
        foreach (Transform child in _transform)
        {
            Respawn(child);
        }
    }
}
