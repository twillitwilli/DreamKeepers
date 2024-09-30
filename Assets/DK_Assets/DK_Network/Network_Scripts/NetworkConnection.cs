using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkConnection : MonoBehaviour
{
    enum ConnectionType
    {
        create,
        join,
        disconnect
    }

    [SerializeField]
    ConnectionType _connection;

    private void OnTriggerEnter(Collider other)
    {
        VRHandController hand;

        if (other.gameObject.TryGetComponent<VRHandController>(out hand))
        {
            switch (_connection)
            {
                case ConnectionType.create:
                    Create();
                    break;

                case ConnectionType.join:
                    Join();
                    break;

                case ConnectionType.disconnect:
                    Debug.Log("Disconnection Not Setup");
                    break;
            }
        }    
    }

    void Create()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Starting Host");
    }

    void Join()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Starting Client");
    }
}
