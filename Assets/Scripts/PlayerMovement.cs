using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector2 m_movement;
    private Rigidbody2D m_rgbd;
    private Camera m_cam;
    private GameObject m_sword;

    private bool m_swinging;
    private float m_currswing;
    private Vector2 m_swingpos;
    private float m_swingangle;
    private Vector2 m_swingstart;
    private Vector2 m_swingend;
    private Vector2 m_swingmid;
    private Vector2 m_bez1;
    private Vector2 m_bez2;

    [SerializeField]
    private float m_speed;

    [SerializeField]
    private float m_swingtime;
 

    // Start is called before the first frame update
    void Start()
    {
        m_movement = Vector2.zero;
        m_rgbd = gameObject.GetComponent<Rigidbody2D>();
        m_cam = Camera.main;
        m_sword = gameObject.transform.GetChild(0).gameObject;
        m_swinging = false;
        m_currswing = 0.0f;
        m_swingpos = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        input = input.normalized;
        m_rgbd.AddForce(input * m_speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(input);
        }

        if (Input.GetMouseButtonDown(0) || m_swinging)
        {
            Swing(m_cam.ScreenToWorldPoint(Input.mousePosition));
            //Jump(input);
        }

        //MoveSword(m_cam.ScreenToWorldPoint(Input.mousePosition));

    }

    private void Jump(Vector2 currDir)
    {
        m_rgbd.AddForce(currDir * 10);
    }

    private void Swing(Vector2 mousePos)
    {
        m_sword?.SetActive(true);
        float a;
        float b;
        if (m_swinging == false)
        {
            m_swingangle = Mathf.Atan2((mousePos.x - m_rgbd.position.x), (mousePos.y - m_rgbd.position.y)) * Mathf.Rad2Deg;
            m_swingpos = mousePos;
            m_swinging = true;
            m_currswing = Time.time;
            Vector2 diff = mousePos - m_rgbd.position;
            diff.Normalize();


            a = m_swingangle + 40.0f;
            b = m_swingangle - 40.0f;

            Vector2 d = new Vector2(Mathf.Sin(a * Mathf.Deg2Rad), Mathf.Cos(a * Mathf.Deg2Rad));
            Vector2 e = new Vector2(Mathf.Sin(b * Mathf.Deg2Rad), Mathf.Cos(b * Mathf.Deg2Rad));

            m_swingstart = m_rgbd.position + d;
            m_swingend = m_rgbd.position + e;
            m_swingmid = m_rgbd.position + diff * 1.5f;

            Debug.DrawLine(mousePos, m_swingend, Color.red, 2.0f);
            Debug.DrawLine(mousePos, m_swingmid, Color.red, 2.0f);
            Debug.DrawLine(mousePos, m_swingstart, Color.red, 2.0f);


        }
        float swingratio = (Time.time - m_currswing) / m_swingtime;
        if (swingratio < 0 || swingratio > 1)
        {
            m_swinging = false;
            return;
        }
        m_bez1 = Vector2.Lerp(m_swingstart, m_swingmid, swingratio);
        m_bez2 = Vector2.Lerp(m_swingmid, m_swingend, swingratio);

        m_sword.transform.SetPositionAndRotation(Vector2.Lerp(m_bez1, m_bez2, swingratio), Quaternion.Slerp(Quaternion.Euler(0, 0, -m_swingangle - 45.0f), Quaternion.Euler(0, 0, -m_swingangle + 45.0f), swingratio));
    }


    private void MoveSword(Vector2 mousePos)
    {
        m_sword?.SetActive(true);
        float angletotarget = Mathf.Atan2((mousePos.x - m_rgbd.position.x), (mousePos.y - m_rgbd.position.y)) * Mathf.Rad2Deg;
        m_sword.transform.SetPositionAndRotation(Vector2.MoveTowards(m_rgbd.position, mousePos, 0.5f), Quaternion.Euler(0, 0, -angletotarget));
    }
}
