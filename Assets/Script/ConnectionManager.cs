using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;

public class ConnectionManager : MonoBehaviour
{
    public GameObject connectionButtonPanel;
    public string ipAddress = "127.0.0.1";
    UNetTransport transport;

    //Happen on server
    public void Host()
    {
        connectionButtonPanel.SetActive(false);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(GetRandomSpawn(), Quaternion.identity);
    }

    //Happen on server
    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        //Check Incoming Data
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "Password1234";
        callback(true, null, approve, GetRandomSpawn(), Quaternion.identity);
    }

    public void Join()
    {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;
        connectionButtonPanel.SetActive(false);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Password1234");
        NetworkManager.Singleton.StartClient();
    }

    Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);

        return new Vector3(x, y, z);
    }

    public void IPAddressChanged(string newAddress)
    {
        this.ipAddress = newAddress;
    }
}
