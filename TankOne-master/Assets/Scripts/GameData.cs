using UnityEngine;

public class GameData : MonoBehaviour
{
    public static string winnerName;
    public static int player1Id = 1, player2Id = 1, winnerId;
    public static int rewarded;
    public static bool isPlayWithAI;



    public static PlayerState playerState = new PlayerState();



    public static bool isFirstOpen = true;

    private void Awake()
    {

        LoadData();
        //playerState.score = 1000;
        //playerState.gem = 1000;
        //playerState.powerMultiplier = 1;
        //playerState.speedMultiplier = 1;
        //SaveData();
    }

    public static void Sound(bool isOn)
    {
        playerState.isSound = isOn;
        SaveData();
    }
    public static void Music(bool isOn)
    {
        playerState.isMusic = isOn;
        SaveData();
    }
    public static void Vibration(bool isOn)
    {
        playerState.isVibrate = isOn;
        SaveData();
    }
    public static bool Sound()
    {
        return playerState.isSound;
    }
    public static bool Music()
    {
        return playerState.isMusic;
    }

    private void LoadData()
    {

        if (PlayerPrefs.HasKey(Constants.STATE))
        {
            string data = PlayerPrefs.GetString(Constants.STATE);
            playerState = JsonUtility.FromJson<PlayerState>(data);



        }
        else
        {
            playerState.score = 100;
            playerState.speedMultiplier = 1;
            playerState.powerMultiplier = 1;
        }


    }


    public static void SaveData()
    {
        string data = JsonUtility.ToJson(playerState);
        PlayerPrefs.SetString(Constants.STATE, data);
        PlayerPrefs.Save();

    }
}
