using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake_Move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float steerSpeed = 180;
    public int Gap = 15;
    public int score = 0;
    public GameObject snakeBodyPart;
    public Material snakeColor;
    public Material appleColor;
    public Material deathColor;
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PosHistory = new List<Vector3>();
    public bool isDead = false;
    public Text displayedScore;

    //public AudioSource gameStart;
    //public AudioSource appleCrunch;
    //public AudioSource bombCollision;
    //public AudioSource gameEnd;
    //public AudioSource gamePlay;

    public AudioSource audioSystem;
    public AudioClip gameStart;
    public AudioClip appleCrunch;
    public AudioClip bombCollision;
    public AudioClip gameEnd;
    public AudioClip gamePlay;

    private GameUIManager uiInteraction;

    
    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < 100; i++)
        //{
        //    PosHistory.Add(transform.position);
        //}

        uiInteraction = FindObjectOfType<GameUIManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        float direction = Input.GetAxis("Horizontal") * Time.deltaTime * steerSpeed;
        transform.Rotate(0, direction, 0);

        PosHistory.Insert(0, transform.position);

        //chat
        // Limit PosHistory size to avoid memory issues
        if (PosHistory.Count > Gap * (BodyParts.Count + 1))
        {
            PosHistory.RemoveAt(PosHistory.Count - 1);
        }
        //chat
        
        if (PosHistory.Count == 0)
            return;

        int index = 0;
        foreach (var body in BodyParts)
        {
            if ((index * Gap) < PosHistory.Count)
            {
                body.transform.position = PosHistory[index * Gap];
            }
            else
            {
                body.transform.position = PosHistory[PosHistory.Count - 1];
            }
            index++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.tag == "Apple")
        {
            Debug.Log("Apple collision");
            Destroy(collision.gameObject);
            GrowSnake();
            ChangeSnakeColor(appleColor);
            audioSystem.PlayOneShot(appleCrunch);
            score++;
        }
        else if (collision.gameObject.tag == "Bomb") {
            Debug.Log("Bomb collision");
            ChangeSnakeColor(deathColor);
            audioSystem.PlayOneShot(bombCollision);
            DestroySnake();
            uiInteraction.GameEnd();
            audioSystem.PlayOneShot(gameEnd);
        }
        else if (collision.gameObject.tag == "snakeBody" || collision.gameObject.tag == "Wall")
        {
            Debug.Log("SnakeBody or Wall Collision");
            ChangeSnakeColor(deathColor);
            //DestroySnake();
            uiInteraction.GameEnd();
            audioSystem.PlayOneShot(gameEnd);
        }

        displayedScore.text = $"{score}";
    }

    private void OnCollisionExit(Collision collision)
    {
        if (Time.time > 2)
        {
            ChangeSnakeColor(snakeColor);
        }
    }


    private void GrowSnake()
    {
        //GameObject body = Instantiate(snakeBodyPart);
        //body.transform.position = transform.position + Vector3.up * 0.5f;
        //BodyParts.Add(body);

        GameObject body = Instantiate(snakeBodyPart);
        // Use PosHistory to place the new body part at the correct position
        if (PosHistory.Count >= Gap)
        {
            body.transform.position = PosHistory[Gap - 1]; // Place at the position where the head was 'Gap' frames ago
        }
        else
        {
            // If not enough history, place it slightly behind the head
            body.transform.position = transform.position - transform.forward * 0.5f;
        }
        body.tag = "snakeBody";
        Collider bodyCollider = body.GetComponent<Collider>();
        if (bodyCollider != null)
        {
            bodyCollider.isTrigger = false;
        }
        BodyParts.Add(body);
        Debug.Log($"Snake grew. Body parts count: {BodyParts.Count}");
    }

    private void ChangeSnakeColor(Material material)
    {
        MeshRenderer headMesh = GetComponent<MeshRenderer>();
        if (headMesh != null)
        {
            Material[] headMaterials = headMesh.materials;
            headMaterials[0] = material;
            headMesh.materials = headMaterials;
        }

        foreach (var body in BodyParts)
        {
            MeshRenderer bodyMesh = body.GetComponent<MeshRenderer>();
            if (bodyMesh != null)
            {
                bodyMesh.material = material;
            }
        }
    }

    void DestroySnake()
    {
        isDead = true;
        //score = 0;
        foreach (var body in BodyParts)
        {
            Destroy(body);
        }
        BodyParts.Clear();
        Destroy(gameObject);
    }
} 