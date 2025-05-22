using UnityEngine;

public class Boss_Dash : StateMachineBehaviour
{
    public float dashSpeed = 15f;
    public float stopDistance = 2f;
    Transform player;
    Rigidbody2D rb;
    Boss boss;
    private float dashTimer;
    public float dashDuration = 4f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dashTimer += Time.deltaTime;

        if (dashTimer >= dashDuration)
        {
            animator.SetBool("Dash", false); // quay lại state Run (giả sử Run có điều kiện Dash == false)
        }
        boss.LookAtPlayer();

        float distance = Vector2.Distance(player.position, rb.position);

        if (distance <= stopDistance)
        {
            animator.SetBool("Dash", false);
            animator.SetTrigger("Attack");
        }
        else
        {
            Vector2 target = new Vector2(player.position.x, player.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, dashSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }


    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optional: reset velocity or animation flags
        dashTimer = 0f;
    }
}
