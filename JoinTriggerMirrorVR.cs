using UnityEngine;
using Mirror;
using Mirror.VR;

public class JoinTriggerMirrorVR : MonoBehaviour
{
    [SerializeField] private string handTag = "HandTag";
    [SerializeField] private bool hasTriggered = false;
    
    public bool HasTriggered => hasTriggered;
    
    private NetworkManager networkManager;

    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag) && !hasTriggered && !IsConnected())
        {
            hasTriggered = true;
            JoinRandomLobby();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            hasTriggered = false;
            
            if (NetworkClient.isConnected)
                NetworkClient.Disconnect();
        }
    }

    private void JoinRandomLobby()
    {
        if (!IsConnected() && networkManager != null)
        {
            MirrorVRManager.JoinRandomLobby();
        }
    }

    private bool IsConnected()
    {
        return NetworkClient.isConnected || NetworkServer.active;
    }
}
