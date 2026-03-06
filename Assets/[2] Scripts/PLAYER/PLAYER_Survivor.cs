using UnityEngine;

public class PLAYER_Survivor : MonoBehaviour
{
    public bool escaped {get; private set;}
    public bool alive {get; private set;} = true;

    public void Escaped(){ escaped = true; GameStateManager.CheckGameState(); }

    public void Death(){ alive = false; Destroy(gameObject); }
        void OnDestroy(){ if (alive){ return; } GameStateManager.CheckGameState(); }
}