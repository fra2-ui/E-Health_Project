using UnityEngine;

public static class GameSession
{
    public static bool playedBusCutscene = false;
    public static bool shownIntroDialogue = false;

    // Scelta personaggio (puoi usare int o string)
    public static int selectedCharacterId = 0;

    public static void ResetForNewRun()
    {
        playedBusCutscene = false;
        shownIntroDialogue = false;
    }
}