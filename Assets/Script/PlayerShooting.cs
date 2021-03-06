using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerShooting : NetworkBehaviour
{

    public TrailRenderer bulletTrail;
    public Transform gunBarrel;
    public float Bulletdistance = 200f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            //shoot
            if (Input.GetButtonDown("Fire1"))
            {
                //actually shoot - tell server we have shoot.
                ShootServerRpc();
            }
        }
        
    }

    //These run on server, called by clients . Clients => server,
    [ServerRpc]
    void ShootServerRpc()
    {
        //do raycast on server to see if we hit an enemy and take damage
        if (Physics.Raycast(gunBarrel.position,gunBarrel.forward,out RaycastHit hit, Bulletdistance))
        {
            //we are hitting something, isit a player?
            var enemyHealth = hit.transform.GetComponent<PlayerHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
            }
        }

        //to show to All Client the animation.
        ShootClientRpc();
    }

    //Run on server, Sent by Server to all client , Server => client
    [ClientRpc]
    void ShootClientRpc()
    {
        var bullet = Instantiate(bulletTrail, gunBarrel.position, Quaternion.identity);
        bullet.AddPosition(gunBarrel.position);
        if (Physics.Raycast(gunBarrel.position,gunBarrel.forward, out RaycastHit hit, Bulletdistance))
        {
            bullet.transform.position = hit.point;
        }
        else
        {
            bullet.transform.position = gunBarrel.position + (gunBarrel.forward * Bulletdistance);
        }
    }
}

