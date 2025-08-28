using Assets.Scripts.Interfaces;
using UnityEngine;

public class SkeletonKnightController : EntityController, IGiveExperience
{
    public float OnGetExperiencePoints() { return 50f; }


}
