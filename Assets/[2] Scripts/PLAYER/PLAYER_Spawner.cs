using PurrNet;
using UnityEngine;

public class PLAYER_Spawner : NetworkBehaviour
{
    [SerializeField] private GameObject survivor;
    [SerializeField] private GameObject monster;

    protected override void OnSpawned(){ if (isOwner){ CALLRPC_Trigger(PLAYER_Data.playerRole); }}
    public void CALLRPC_Trigger(ENUM_playerRole role){ RPC_UPDATESERVER_Trigger(role); }
        [ServerRpc]
        void RPC_UPDATESERVER_Trigger(ENUM_playerRole role){ RPC_UPDATECLIENTS_Trigger(role); }
        [ObserversRpc]
        void RPC_UPDATECLIENTS_Trigger(ENUM_playerRole role){ Trigger(role); }
        void Trigger(ENUM_playerRole role)
        {
            if (role == ENUM_playerRole.Monster){ SpawnAsMonster(); return; }
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