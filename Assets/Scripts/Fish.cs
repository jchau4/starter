using UnityEngine;

public class Fish : Food
{
    // Polymorphism here overrides Food class method so that Fish increases affection by 20
    public override int GetAffectionChange()
    {
        return 20;
    }
}
