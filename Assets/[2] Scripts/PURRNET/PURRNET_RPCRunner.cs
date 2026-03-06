using PurrNet;
using UnityEngine.Events;

public class PURRNET_RPCRunner : NetworkBehaviour
{
    public UnityEvent unityEvent;

    public void Trigger(){ RPC_UPDATESERVER_Trigger(); }
        private void Triggering(){ unityEvent.Invoke(); }
        [ServerRpc]
        private void RPC_UPDATESERVER_Trigger(){ RPC_UPDATECLIENTS_Trigger(); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_Trigger(){ Triggering(); }
}