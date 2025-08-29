using UnityEngine;

public class EasyDifficulty : Difficulty
{
    public override int[] GetDungeonLength()
    {
        int minLength = 10;
        int maxLength = 20;
        int[] dungeonLenght = { minLength, maxLength };

        return dungeonLenght;
    }

    public override int GetEnemyLevel()
    {
        return UnityEngine.Random.Range(1, 6);
    }
}
