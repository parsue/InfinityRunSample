using UnityEngine;

[CreateAssetMenu]
public class DataProfile : ScriptableObject
{
    public GameData gameData;
    public GameSetting gameSetting;

    public void CopyTo(DataProfile target)
    {
        gameData.CopyTo(target.gameData);
        gameSetting.CopyTo(target.gameSetting);
    }

    public void CopyGameDataTo(GameData target)
    {
        gameData.CopyTo(target);
    }

    public void CopyGameSettingTo(GameSetting target)
    {
        gameSetting.CopyTo(target);
    }
}
