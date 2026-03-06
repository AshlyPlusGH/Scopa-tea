using PurrNet;
using UnityEngine.Events;

public class PURRNET_RPCRunner : NetworkBehaviour
{
    public UnityEvent unityEvent;

    public void Trigger(){ RPC_UPDATESERVER_Trigger(); }
            [ServerRpc]
            private void RPC_UPDATESERVER_Trigger(){ RPC_UPDATECLIENTS_Trigger(); }
            [ObserversRpc]
            private void RPC_UPDATECLIENTS_Trigger(){ Triggering(); }
    public void TriggerServerside(){ RPC_SERVER_Trigger(); }
            [ServerRpc]
            private void RPC_SERVER_Trigger(){ Triggering(); }
        private void Triggering(){ unityEvent.Invoke(); }
}