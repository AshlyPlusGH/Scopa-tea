using PurrNet;
using PurrNet.Transports;
using PurrNet.Utils;
using UnityEngine;
using UnityEngine.Events;

public class PURRNET_UnityEventOnClientConnectionStateChanged : MonoBehaviour
{
    [SerializeField] private bool debug;

    [Space(10)]

    [SerializeField] private bool isDDoL = false;

    [Space(10)]

    [SerializeField] private UnityEvent onClientConnected;
    [SerializeField] private UnityEvent onClientConnecting;
    [SerializeField] private UnityEvent onClientDisconnected;
    [SerializeField] private UnityEvent onClientDisconnecting;

    private NetworkManager networkManager;

    void Awake(){ if (isDDoL){ DontDestroyOnLoad(gameObject); } NetworkManager foundNetworkManager = FindFirstObjectByType<NetworkManager>(); if (foundNetworkManager != null){ Setup(foundNetworkManager); }}
    void Setup(NetworkManager newNetworkManager){ networkManager = newNetworkManager; networkManager.onClientConnectionState += Trigger; }

    public void Trigger(ConnectionState connectionState)
    {
        // Prevents triggering on Local Close App Commands
        if (ApplicationContext.isQuitting){ return; }

        switch (connectionState)
        {
            case ConnectionState.Connecting:
                if (debug){ Debug.Log("Client Connecting caused Event Trigger at: " + name); }
                onClientConnecting.Invoke();
                break;
            case ConnectionState.Connected:
                if (debug){ Debug.Log("Client Connected caused Event Trigger at: " + name); }
                onClientConnected.Invoke();
                break;
            case ConnectionState.Disconnected:
                if (debug){ Debug.Log("Client Disconnected caused Event Trigger at: " + name); }
                onClientDisconnected.Invoke();
                break;
            case ConnectionState.Disconnecting:
                if (debug){ Debug.Log("Client Disconnecting caused Event Trigger at: " + name); }
                onClientDisconnecting.Invoke();
                break;
        }
    }
}