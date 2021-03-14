using UnityEngine;

public interface IGameData { }

[System.Serializable]
public class GameData : IGameData
{
    public float distance;
    public int coin;

    public void CopyTo(GameData target)
    {
        target.distance = distance;
        target.coin = coin;
    }
}

[System.Serializable]
public class GameSetting : IGameData
{
    public float musicVol;
    public float soundVol;
    public bool musicMute;
    public bool soundMute;

    public void CopyTo(GameSetting target)
    {
        target.musicVol = musicVol;
        target.soundVol = soundVol;
        target.musicMute = musicMute;
        target.soundMute = soundMute;
    }
}