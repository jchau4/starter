using System.Runtime.CompilerServices;
using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    // static field: tracks successful amount of times cat was fed
    public static int bigBackCount = 0;
    private Cat myCat = new Cat();
    private Transform selectedObject;
    private Vector3 originalPosition;
    // attributes: serializefield attribute tells Unity to display and serialize private view in inspector
    // allowing assigning references/adjust values without making field public
    [SerializeField] private GameObject catObject;
    [SerializeField] private Texture2D idleTexture;
    [SerializeField] private Texture2D happyTexture;
    [SerializeField] private Texture2D angryTexture;
    [SerializeField] private Texture2D cryingTexture;
    [SerializeField] private SkinnedMeshRenderer catBodyRenderer;
    private float expressionTimer = 0f;
    private float angryTimer = 10f;
    [SerializeField] private float expressionDuration = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectFood();
        }
        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            DragFood();

        }
        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            ReleaseFood();
        }
        if (expressionTimer > 0)
        {
            expressionTimer -= Time.deltaTime;
            if (expressionTimer <= 0)
            {
                catBodyRenderer.material.SetTexture("_BaseMap", idleTexture);
            }
        }
    }
    private void SelectFood()
    {
        // out parameter: Raycast fills "hit" with info about
        // object clicked so we can identify and interact with it
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Food"))
            {
                selectedObject = hit.collider.transform;
                originalPosition = selectedObject.position;
                Debug.Log("Selected " + selectedObject.name);
            }
        }
    }
    private void DragFood()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(selectedObject.position);
        mousePosition.z = screenPosition.z;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        selectedObject.position = worldPosition;
    }
    private void ReleaseFood()
    {
        Collider catCollider = catObject.GetComponent<Collider>();
        Collider foodCollider = selectedObject.GetComponent<Collider>();
        if (catCollider != null && foodCollider != null)
        {
            if (catCollider.bounds.Intersects(foodCollider.bounds))
            {
                bigBackCount++;
                Debug.Log("Cat has been fed " + bigBackCount + " times!");
                feedCat();
                if (bigBackCount == 10)
                {
                    Debug.Log("Don't you think the cat has had enough?");
                }
            }
        }
        selectedObject.position = originalPosition;
        selectedObject = null;
    }
    private void feedCat()
    {
        if (selectedObject.gameObject.name == "Herring")
        {
            Food fish = new Fish();
            myCat.ChangeAffection(fish.GetAffectionChange());
            Animator catAnimator = catObject.GetComponent<Animator>();
            if (catAnimator != null)
            {
                catAnimator.SetTrigger("Jump");
            }
            Debug.Log(@"Cat's affection has increased to " + myCat.Affection + @"!
                        Cat happy 
                        /\_/\
                        (=^.^=)");
            catBodyRenderer.material.SetTexture("_BaseMap", happyTexture);
            expressionTimer = expressionDuration;
        }
        else if (selectedObject.gameObject.name == "lemon")
        {
            Food lemon = new Lemon();
            myCat.ChangeAffection(lemon.GetAffectionChange());
            Debug.Log("Cat's affection has decreased to " + myCat.Affection + @" . . .
                        Cat disappointed 
                        /\_/\
                        (>.<)");
            catBodyRenderer.material.SetTexture("_BaseMap", cryingTexture);
            expressionTimer = expressionDuration;
            if (myCat.Affection <= 0)
            {
                catBodyRenderer.material.SetTexture("_BaseMap", angryTexture);
                expressionTimer = angryTimer;
            }
        }
    }
}
