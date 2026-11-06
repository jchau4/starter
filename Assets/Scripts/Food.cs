using UnityEngine;

public class Food
{
    // Allows different food types to override how much they change cat's affection
    public virtual int GetAffectionChange()
    {
        return 0;
    }
}
