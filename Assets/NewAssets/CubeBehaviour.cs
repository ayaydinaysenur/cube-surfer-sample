using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    private Rigidbody rigidbody;
    private BoxCollider boxCollider;
    private Vector3 topCubePosition;
    private Transform topCube;//it will always under player foot
    public bool canMove;
    //new tags: Cube, NewCube, Coin= Point, SkippedFinish = FinishStep, TopCube
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        topCube = GameObject.FindGameObjectWithTag(Constants.TAG_TOP_CUBE).transform;
        SwipeController.OnSwipe += MoveHorizontal;
        if (transform.tag == Constants.TAG_TOP_CUBE)
        {
            //canMove = true;
            GameController.OnGameStart += StartMovement;
            boxCollider.isTrigger = false;
            rigidbody.useGravity = true;
        }
        else
        {
            canMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            rigidbody.velocity = new Vector3(0, 0, GameController.gameSpeed);
        }
    }

    private void MoveHorizontal(float horizontalMovementAmount)
    {
        if (canMove)
        {
            transform.position += transform.right * horizontalMovementAmount;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((transform.tag == Constants.TAG_CUBE || transform.tag == Constants.TAG_TOP_CUBE)
            && other.tag == Constants.TAG_NEW_CUBE)
        {
            AddNewCubeToStack(other.transform);
            GameController.Instance.TriggerPlayerAnimation(Constants.TRIGGER_PLAYER_ANIMATION_JUMP);
        }
       
        if (other.transform.tag == Constants.TAG_COIN)
        {
           // pointManager.GetComponent<PointManager>().IncreasePoint();
            Destroy(other.gameObject);//buralar icin pool manager
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == Constants.TAG_BARRIER)
        {
            if (transform.tag == Constants.TAG_TOP_CUBE && IsFacing(collision.transform))
            {
                Debug.Log("GAME OVER");
                GameController.Instance.GameOverFunction(false);
            }
            else if (transform.tag == Constants.TAG_CUBE && IsFacing(collision.transform))
            {
                //stop moving
                Debug.Log(transform.name + " " + collision.transform.name);
                if (transform.GetComponent<CubeBehaviour>())
                {
                    transform.GetComponent<CubeBehaviour>().enabled = false;
                    rigidbody.velocity = Vector3.zero;
                }
            }
        }
        if (collision.transform.tag == Constants.TAG_FINISH)
        {
            Debug.Log("GAME OVER finish");

            if (transform.tag == Constants.TAG_TOP_CUBE)
            {
                GameController.Instance.GameOverFunction(true);
            }
            collision.transform.tag = Constants.TAG_FINISH_STEP;
        }
    }

    private void AddNewCubeToStack(Transform cubeTransform)//gelen yeni cube en uste player cubeun altina eklenir
    {
        topCubePosition = topCube.position;
        topCube.position += new Vector3(0, 0.65f, 0);
        cubeTransform.position = topCubePosition;
        CubeBehaviour cubeBehaviour = cubeTransform.GetComponent<CubeBehaviour>();
        cubeBehaviour.enabled = true;
        cubeBehaviour.PrepareForMovement();
    }

    public void PrepareForMovement()
    {
        canMove = true;
        transform.tag = Constants.TAG_CUBE;
        boxCollider.isTrigger = false;
        rigidbody.useGravity = true;
        rigidbody.angularDrag = 0.1f;
    }

    private bool IsFacing(Transform other)
    {
        // Check if the gaze is looking at the front side of the object
        Vector3 forward = transform.forward;
        Vector3 toOther = (other.position - transform.position).normalized;

        if (Vector3.Dot(forward, toOther) < 0.7f)
        {
            Debug.Log("Not facing the object");
            return false;
        }

        Debug.Log("Facing the object");
        return true;
    }

    public void StartMovement()
    {
        canMove = true;
    }
}
