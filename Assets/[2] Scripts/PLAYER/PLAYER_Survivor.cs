using PurrNet;
using UnityEngine;

public class PLAYER_Survivor : NetworkBehaviour
{
    public PLAYER_Pointer pointer;

    public bool escaped {get; private set;}
    public bool alive {get; private set;} = true;

    public void Escaped(){ if (!isOwner){ return; } RPC_UPDATESERVER_Escaped(); }
        [ServerRpc]
        private void RPC_UPDATESERVER_Escaped(){ RPC_UPDATECLIENTS_Escaped(); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_Escaped(){ ACTION_Escaped(); }
        private void ACTION_Escaped()
        {
            escaped = true; GameStateManager.CheckGameState();

            pointer.gameObject.SetActive(false);
        }

    public void Death(){ RPC_UPDATESERVER_Death(); }
        protected override void OnDestroy(){ if (alive){ return; } GameStateManager.CheckGameState(); }

        [ServerRpc]
        private void RPC_UPDATESERVER_Death(){ RPC_UPDATECLIENTS_Death(); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_Death(){ ACTION_Death(); }
        private void ACTION_Death(){ alive = false; if (isOwner){ Destroy(gameObject); }}
}