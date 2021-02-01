using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector2 m_movement;
    private Rigidbody2D m_rgbd;
    private Camera m_cam;
    private GameObject m_sword; 

    [SerializeField]
    private float m_speed;
 

    // Start is called before the first frame update
    void Start()
    {
        m_movement = Vector2.zero;
        m_rgbd = gameObject.GetComponent<Rigidbody2D>();
        m_cam = Camera.main;
        m_sword = gameObject.transform.GetChild(0).gameObject;
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
        
        if (Input.GetMouseButtonDown(0))
        {
            Swing(m_cam.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void Jump(Vector2 currDir)
    {
        m_rgbd.AddForce(currDir * 10);
    }

    private void Swing(Vector2 mousePos)
    {
        m_sword?.SetActive(true);
        float angletotarget = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
        m_sword.transform.SetPositionAndRotation(Vector2.MoveTowards(m_rgbd.position, mousePos, 0.5f), Quaternion.Euler(0, 0, -angletotarget));
    }
}
