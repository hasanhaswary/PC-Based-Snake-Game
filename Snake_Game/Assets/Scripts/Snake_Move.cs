using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float steerSpeed = 180;
    public int Gap = 15;
    public int score = 0;
    public GameObject snakeBodyPart;
    public Material snakeColor;
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PosHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < 100; i++)
        //{
        //    PosHistory.Add(transform.position);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += transform.forward * moveSpeed *Time.deltaTime;


        
        float direction = Input.GetAxis("Horizontal") * Time.deltaTime * steerSpeed;
        transform.Rotate(0,direction, 0);


        PosHistory.Insert(0, transform.position);

        if (PosHistory.Count == 0)
            return;

        int index = 0;
        foreach (var body in BodyParts)
        {
            if ((index*Gap) < PosHistory.Count)
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
        if (collision.gameObject.tag == "Apple")
        {
            Destroy(collision.gameObject);
            //GetComponent<MeshRenderer>().material.color = Color.green;
            GrowSnake();
            score++;

        }
        
       

    }

    private void GrowSnake()
    {
        GameObject body = Instantiate(snakeBodyPart);
        body.transform.position = transform.position + Vector3.up * 0.5f;
        BodyParts.Add(body);
    }




}
