using UnityEngine;

public class CatDemo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cat myCat = new Cat();
        Food fish = new Fish();  // <----- Polymorph here (≧▽≦) Fish and lemon treated as Food objects 
        Food lemon = new Lemon(); // <----- Polymorph here too (⌒▽⌒)☆ but override GetAffectionChange w/ different value

        // ChangeAffection() receives int returned by GetAffectionChange()
        myCat.ChangeAffection(fish.GetAffectionChange());
        Debug.Log(@"Cat's affection has increased to " + myCat.Affection + @"!
        Cat happy 
         /\_/\
        (=^.^=)");
        myCat.ChangeAffection(lemon.GetAffectionChange());
        Debug.Log("Cat's affection has decreased to " + myCat.Affection + @". . .
            Cat disappointed 
            /\_/\
            (>.<)");
    }
}
