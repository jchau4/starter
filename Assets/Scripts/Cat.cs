using UnityEngine;

public class Cat
{
    private int affection = 50;
    public int Affection
    {
        get 
        { 
            return affection; // allows other classes to access value
        }
    }

    public void ChangeAffection(int amount)
    {
        affection += amount; // modifies private affection field
    }
}
