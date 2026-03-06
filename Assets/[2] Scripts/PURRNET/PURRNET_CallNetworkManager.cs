using PurrNet;
using UnityEngine;

public class PURRNET_CallNetworkManager : MonoBehaviour
{
    public void StopServer(){ if (NetworkManager.main != null){ NetworkManager.main.StopServer(); }}
}