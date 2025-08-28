using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IGainExperienceModel
    {

        void GainExp(float amount);
        void LevelUp();
        float CalculateExpToNextlevel();
    }
}
