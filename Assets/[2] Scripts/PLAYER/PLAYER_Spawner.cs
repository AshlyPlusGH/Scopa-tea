using PurrNet;
using UnityEngine;

public class PLAYER_Spawner : NetworkBehaviour
{
    [SerializeField] private GameObject survivor;
    [SerializeField] private GameObject monster;

    protected override void OnSpawned(){ if (isOwner){ CALLRPC_Trigger(); }}
    public void CALLRPC_Trigger(){ RPC_UPDATESERVER_Trigger(); }
        [ServerRpc]
        void RPC_UPDATESERVER_Trigger(){ RPC_UPDATECLIENTS_Trigger(); }
        [ObserversRpc]
        void RPC_UPDATECLIENTS_Trigger(){ Trigger(); }
        void Trigger()
        {
            if (PLAYER_Data.playerRole == ENUM_playerRole.Monster){ SpawnAsMonster(); return; }
            SpawnAsSurvivor();
        }
            void SpawnAsSurvivor()
            {
                if (!survivor.activeInHierarchy){ survivor.SetActive(true); }
                survivor.transform.SetParent(null);

                RPC_SERVER_Cleanup();
            }
            void SpawnAsMonster()
            {
                if (!monster.activeInHierarchy){ monster.SetActive(true); }
                monster.transform.SetParent(null);
                
                RPC_SERVER_Cleanup();
            }
                [ServerRpc]
                void RPC_SERVER_Cleanup(){ Destroy(gameObject); }
}