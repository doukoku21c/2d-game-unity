using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Think();

        Invoke("Think", 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2 (nextMove, rigid.velocity.y);

        // ray���� �̿��� ���� üũ
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
            Turn();
     
    }

    // ����Լ� = �ڱ� �ڽ��� �ٽ� ȣ���ϴ� �Լ� ���⼭�� 5�ʸ��� �����ϰ� ����
    void Think()
    {
        // ���� �ൿ�� �¾���
        nextMove = Random.Range(-1, 2);

        //sprite animaiton
        anim.SetInteger("WalkSpeed", nextMove);

        // flip xüũ�ڽ��� ������ Ǯ�����ϸ鼭 �¿�� ���� �� 
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1; 

        // �ݺ�����
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }
    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }
}