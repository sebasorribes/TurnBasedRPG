using Assets.Scripts.Interfaces;
using UnityEngine;

public class SkeletonKnightController : EntityController, IGiveExperience, ISetLevel
{
    public float OnGetExperiencePoints() { return 50f * model.GetLevel(); }

    public void OnSetLevel(int level)
    {
        GetComponent<SkeletonKnightModel>().OnSetLevel(level);
        GetComponent<SkeletonKnightView>().SetLevel(level);
        view.SetHealthText((int) model.GetCurrentHealth());
    }
}
