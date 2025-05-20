using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 10f;
    public float attackRange = 5f;
    Transform player;
    Rigidbody2D rb;

    Boss boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!boss.isActivated) return;
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, player.position.y); // ✅ Theo cả X và Y
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        Debug.Log("Boss running toward player");
        Debug.Log(Vector2.Distance(player.position, rb.position));

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            int randomSkill = Random.Range(1, 4); // Random từ 1 đến 3

            switch (randomSkill)
            {
                case 1:
                    animator.SetTrigger("Attack1");
                    break;
                case 2:
                    animator.SetTrigger("Attack2");
                    break;
                case 3:
                    animator.SetTrigger("Skill1");
                    break;
            }
        }
        float distance = Vector2.Distance(player.position, rb.position);
        if (distance > 20f) // nếu xa hơn 5 đơn vị thì dash
        {
            animator.SetBool("Dash", true);
            return;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack1");
    }


}
