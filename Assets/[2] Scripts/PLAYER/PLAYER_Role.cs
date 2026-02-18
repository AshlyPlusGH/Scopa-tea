using UnityEngine;
using PurrNet;

public class PLAYER_Role : NetworkBehaviour
{
    public Role role {get; private set;} = Role.Survivor;

    protected override void OnSpawned(){ Setup(); }

    void Setup()
    {
        if (role == Role.Survivor){}
        else if (role == Role.Monster){ GetComponent<PLAYER_MONSTER_Attack>().enabled = true; }
    }
}

public enum Role
{
    Survivor,
    Monster
}