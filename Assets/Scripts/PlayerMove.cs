using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; // ���⼭ maxspeed �����߱⶧���� ��Ʈ�ѹٿ��� ���ǵ� :0 ���� �ӵ� ���� ����
    public float jumpPower; // ���⼭ jumpPower �����߱⶧���� ��Ʈ�ѹٿ��� �����Ŀ� :0 ���� �ӵ� ���� ����
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update() // ĳ���� �ȹи��� ��ũ��Ʈ ���⶧ speed
    {
        // �÷��̾� ����

        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        // ������ȯ
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // �ִϸ��̼� �ȴٰ� �ٴٰ� 
        if (Mathf.Abs(rigid.velocity.x) < 0.3) // �����Լ� Mathf ���밪 abs ���밪�� 0.3 ���� ������ �ϱ⸦ ����
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void FixedUpdate() // �������� Ű�Է�
    {
        // Ű���� ��Ʈ�ѿ� ���� �̵�(�¿�)
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h*4, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // ������ ���ǵ� ����
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // ���� ���ǵ� ����
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            OnDamaged(collision.transform.position);
        {

        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 11;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        Invoke("OffDamaged", 3);

        
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}