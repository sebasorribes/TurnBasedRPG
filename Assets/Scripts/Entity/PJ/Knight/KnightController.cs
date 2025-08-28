using Assets.Scripts.Interfaces;
using UnityEngine;

public class KnightController : EntityController, IGainExperience
{
    public void GainExp(float amount)
    {
        GetComponent<IGainExperienceModel>().GainExp(amount);
    }
}