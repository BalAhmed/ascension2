using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform[] Position;
    [SerializeField] float ObjectSpeed;

    int NextPosIndex;
    Transform NextPos;
    
    void Start()
    {
        NextPos = Position[0];
    }

    // Update is called once per frame
    void Update()
    {
        MoveGameObject();
    }

    void MoveGameObject()
    {
        if(transform.position == NextPos.position)
        {
            NextPosIndex++;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            if (NextPosIndex >= Position.Length)
            {
                NextPosIndex = 0;
            }
            NextPos = Position[NextPosIndex];
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPos.position, ObjectSpeed * Time.deltaTime);
            
        }
    }

}
