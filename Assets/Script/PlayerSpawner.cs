using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerSpawner : NetworkBehaviour
{

    CharacterController cc;
    Renderer[] renderers;
    public Behaviour[] scripts;

    public ParticleSystem deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
      if (IsLocalPlayer && Input.GetKeyDown(KeyCode.Y))
        {
            Respawn();
        }   
    }

    public void Respawn()
    {
        RespawnServerRPC();
    }

    [ServerRpc]
    void RespawnServerRPC()
    {
        //on the server - so its synced 
        RespawnClientRPC(GetRandomSpawn());
    }

    [ClientRpc]
    void RespawnClientRPC(Vector3 spawnPos)
    {
        StartCoroutine(RespawnCoroutine(spawnPos));
    }

    Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-10f, 10f);
        float y = 2f;
        float z = Random.Range(-10f, 10f);

        return new Vector3(x, y, z);
    }

    IEnumerator RespawnCoroutine(Vector3 spawnPos)
    {
        Instantiate(deathParticles, transform.position, transform.rotation);
        cc.enabled = false;
        SetPlayerState(false);
        yield return new WaitForSeconds(3f); 
        transform.position = spawnPos;
        cc.enabled = true;
        SetPlayerState(true);
    }

    void SetPlayerState(bool state)
    {
        foreach (var script in scripts)
        {
            script.enabled = state;
        }
        foreach (var renderer in renderers)
        {
            renderer.enabled = state;
        }
    }
}
