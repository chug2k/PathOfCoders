using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; // Degrees per second
    private Vector3 targetPosition;
    private bool isMoving = false;
    private const float minMoveDistance = 0.1f; // Minimum distance to move

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check for input depending if we are on a mobile device or not
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            SetTargetPosition();
        }

        if (isMoving)
        {
            Move();
        }
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Use touches if on mobile
        if (Input.touchCount > 0)
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        }

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Check if the hit point is far enough to warrant movement
            if (Vector3.Distance(hit.point, transform.position) > minMoveDistance)
            {
                targetPosition = hit.point;
                isMoving = true;
                animator.SetBool("isWalking", true);
            }
        }
    }

    void Move()
    {
        // Calculate direction vector
        Vector3 targetDirection = new Vector3(targetPosition.x, transform.position.y, targetPosition.z) - transform.position;
        
        // Check if we have a valid direction vector
        if (targetDirection.sqrMagnitude > 0.0001)
        {
            // Rotate towards the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the character has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < minMoveDistance)
        {
            animator.SetBool("isWalking", false);
            isMoving = false;
        }
    }
}
