using PurrNet;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static void CheckGameState()
    {
        PLAYER_Survivor[] survivors = FindObjectsByType<PLAYER_Survivor>(0);
        if (survivors.Length == 0){ if (PLAYER_Data.playerRole == ENUM_playerRole.Monster){ MonsterWin(); } else { SurvivorLose(); } return;}
        
        bool survivorVictory = true;
        foreach (PLAYER_Survivor survivor in survivors)
        {
            if (survivor.escaped == false){ survivorVictory = false; }
        }

        if (survivorVictory){ if (PLAYER_Data.playerRole == ENUM_playerRole.Survivor){ SurvivorWin(); } else { MonsterLose(); }}
    }

    static void MonsterWin(){ TEMP_Win(); }
    static void MonsterLose(){ TEMP_Lose(); }
    static void SurvivorWin(){ TEMP_Win(); }
    static void SurvivorLose(){ TEMP_Lose(); }
    static void TEMP_Win()
    {
        NetworkManager.main.StopClient();
        SCENE.LoadScene("[TESTING] win");
        SCENE.UnloadScene(SceneManager.GetActiveScene().name);
    }
    static void TEMP_Lose()
    {
        NetworkManager.main.StopClient();
        SCENE.LoadScene("[TESTING] lose");
        SCENE.UnloadScene(SceneManager.GetActiveScene().name);
    }
}