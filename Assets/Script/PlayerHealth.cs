using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class PlayerHealth : NetworkBehaviour
{
    //Make Synchroniable variable to store health
    //NetworkVariablePermission.OwnerOnly allows for OwnerGameobject to change server side value.
    [SerializeField]
    NetworkVariableInt health = new NetworkVariableInt( new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly}, 100);

    PlayerSpawner playerSpawnner;

    private void Start()
    {
        playerSpawnner = GetComponent<PlayerSpawner>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (health.Value <= 0 && IsLocalPlayer)
        {
            health.Value = 100;
            playerSpawnner.Respawn();
        }
    }

    //runs on server
    public void TakeDamage(int damage)
    {
        health.Value -= damage;
    }
}
