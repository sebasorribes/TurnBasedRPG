using UnityEngine;

public class Stairs : Interactable
{

    public override void Interaction()
    {
        GameManager.Instance.EndExploring();
    }
}
