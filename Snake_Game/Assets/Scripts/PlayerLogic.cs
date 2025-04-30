using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float steerSpeed = 180;
    public int Gap = 2;
    public int score = 0;
    public GameObject snakeBodyPart;
    public Material snakeColor;
    public Material appleColor;
    public Material deathColor;
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PosHistory = new List<Vector3>();
    public bool isDead = false;
    public Text displayedScore;

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
        uiInteraction = FindObjectOfType<GameUIManager>();
        PosHistory.Add(transform.position);
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        float direction = Input.GetAxis("Horizontal") * Time.deltaTime * steerSpeed;
        transform.Rotate(0, direction, 0);

        PosHistory.Insert(0, transform.position);

        if (PosHistory.Count > Gap * (BodyParts.Count + 1))
        {
            PosHistory.RemoveAt(PosHistory.Count - 1);
        }
        
        if (PosHistory.Count == 0)
            return;

        int index = 0;
        foreach (var body in BodyParts)
        {
            int historyIndex = Mathf.Min((index + 1) * Gap, PosHistory.Count - 1);
            if ((index * Gap) < PosHistory.Count)
            {
                body.transform.position = PosHistory[historyIndex];
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
            Destroy(collision.gameObject);
            DestroySnake();
            uiInteraction.GameEnd();
            audioSystem.PlayOneShot(gameEnd);
        }
        else if (collision.gameObject.tag == "snakeBody")
        {
            Debug.Log("SnakeBody Collision");
            ChangeSnakeColor(deathColor);
            DestroySnake();
            uiInteraction.GameEnd();
            audioSystem.PlayOneShot(gameEnd);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Wall Collision");
            ChangeSnakeColor(deathColor);
            DestroySnake();
            uiInteraction.GameEnd();
            audioSystem.PlayOneShot(gameEnd);
        }
        displayedScore.text = $"{score}";
    }

    private void OnCollisionExit(Collision collision)
    {
        if (Time.time > 4)
        {
            ChangeSnakeColor(snakeColor);
        }
    }


    private void GrowSnake()
    {

        GameObject body = Instantiate(snakeBodyPart);
        if (BodyParts.Count < 2)
        {
            body.tag = "Untagged";
        }
        else
        {
            body.tag = "snakeBody";
        }

        // Position behind the last body part or head
        if (BodyParts.Count > 0)
        {
            GameObject lastBodyPart = BodyParts[BodyParts.Count - 1];
            body.transform.position = lastBodyPart.transform.position;
        }
        else if (PosHistory.Count > 2)
        {
            body.transform.position = PosHistory[2];
        }
        else
        {
            body.transform.position = transform.position - transform.forward * 0.5f;
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
        score = 0;
        foreach (var body in BodyParts)
        {
            Destroy(body);
        }
        BodyParts.Clear();
        Destroy(gameObject);
    }
} 