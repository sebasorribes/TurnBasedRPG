using TMPro;
using UnityEngine;

public class SkeletonKnightView : EntityView
{
    //ver de hacer general despues
    [SerializeField] private GameObject levelText;

    public void SetLevel(int level)
    {
        levelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + level.ToString();
    }
}
