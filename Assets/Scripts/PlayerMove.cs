using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; // 여기서 maxspeed 선언했기때문에 컨트롤바에서 스피드 :0 으로 속도 조절 가능
    public float jumpPower; // 여기서 jumpPower 선언했기때문에 컨트롤바에서 점프파워 :0 으로 속도 조절 가능
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

    private void Update() // 캐릭터 안밀리는 스크립트 멈출때 speed
    {
        // 플레이어 점프

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
        // 방향전환
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // 애니메이션 걷다가 뛰다가 
        if (Mathf.Abs(rigid.velocity.x) < 0.3) // 수학함수 Mathf 절대값 abs 절대값이 0.3 보다 작으면 하기를 실행
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void FixedUpdate() // 지속적인 키입력
    {
        // 키보드 컨트롤에 의한 이동(좌우)
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h*4, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // 오른쪽 스피드 제한
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 스피드 제한
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
