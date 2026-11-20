using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CatInteraction : MonoBehaviour
{
    // static field: tracks successful amount of times cat was fed
    public static int bigBackCount = 0;
    private Cat myCat = new Cat();
    private Transform selectedObject;
    private Vector3 originalPosition;
    private bool gameOver = false;
    // attributes: serializefield attribute tells Unity to display and serialize private view in inspector
    // allowing assigning references/adjust values without making field public
    [SerializeField] private GameObject catObject;
    [SerializeField] private Texture2D idleTexture;
    [SerializeField] private Texture2D happyTexture;
    [SerializeField] private Texture2D angryTexture;
    [SerializeField] private Texture2D cryingTexture;
    [SerializeField] private SkinnedMeshRenderer catBodyRenderer;
    [SerializeField] private TMP_Text endingTitle;
    [SerializeField] private TMP_Text endingDesc;
    [SerializeField] private TMP_Text restartText;
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private Image endingBackground;

    private float expressionTimer = 0f;
    private float angryTimer = 10f;
    [SerializeField] private float expressionDuration = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartText.text = "Press R to Restart";
    }

    // Update is called once per frame
    // mouse movement here as a custom component that reads input and uses Update() to change over time
    void Update()
    {
        if (!gameOver && Input.GetMouseButtonDown(0))
        {
            SelectFood();
        }
        if (!gameOver && Input.GetMouseButton(0) && selectedObject != null)
        {
            DragFood();

        }
        if (!gameOver && Input.GetMouseButtonUp(0) && selectedObject != null)
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
                FeedCat();
                if (bigBackCount == 10)
                {
                    Debug.Log("Don't you think the cat has had enough?");
                }
            }
        }
        selectedObject.position = originalPosition;
        selectedObject = null;
    }
    private void FeedCat()
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
            CheckEnding();
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
            CheckEnding();
        }
    }
    // Message displayed for winning/losing game
    private void CheckEnding()
    {
        if (myCat.Affection >= 100)
        {
            endingTitle.text = "GOOD ENDING 1/2";
            endingDesc.text = "Cat loves you!";
            gameOver = true;
            endingBackground.color = new Color32(23, 20, 33, 255);
            endingPanel.SetActive(true);
        }
        else if (myCat.Affection <= -100)
        {
            endingTitle.text = "BAD ENDING 2/2";
            endingDesc.text = "That's unfortunate";
            gameOver = true;
            endingBackground.color = new Color32(9, 11, 18, 255);
            endingPanel.SetActive(true);
        }
    }
}
