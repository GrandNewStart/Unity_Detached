using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScrolling : MonoBehaviour
{
    [SerializeField] Transform[] m_tfBackgrounds = null;
    [SerializeField] float m_speed = 0f;
    float playerVelocity = 0f;
    public GameObject player;

    private int arr_size;
    public float t_length;
    private float m_leftPosX = 0f;
    private float m_rightPosX = 0f;
    private int leftIndex;
    private int rightIndex;


    void Start()
    {
        t_length = m_tfBackgrounds[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        arr_size = m_tfBackgrounds.Length;
        m_leftPosX = m_tfBackgrounds[0].position.x - t_length*0.5f;//-t_lenght;

        /*for(int i=1; i<arr_size; i++)
        {
            Vector3 pos = m_tfBackgrounds[i - 1].position;
            m_tfBackgrounds[i].position = new Vector3(pos.x + t_length*2, pos.y, pos.z);
        }*/
        m_rightPosX = m_tfBackgrounds[arr_size - 1].position.x + t_length*0.5f;//t_lenght * m_tfBackgrounds.Length;
        leftIndex = 0;
        rightIndex = arr_size - 1;

    }


    void Update()
    {
        playerVelocity = player.GetComponent<Rigidbody2D>().velocity.x;
        for(int i=0; i<m_tfBackgrounds.Length; i++)
        {
            m_tfBackgrounds[i].position += new Vector3(m_speed* playerVelocity, 0, 0);  
        }

        if (m_tfBackgrounds[leftIndex].position.x < m_leftPosX)
        {
            Vector3 t_selfPos = m_tfBackgrounds[leftIndex].position;
            //t_selfPos.Set(m_rightPosX, t_selfPos.y, t_selfPos.z);
            t_selfPos.Set(m_tfBackgrounds[rightIndex].position.x + t_length, t_selfPos.y, t_selfPos.z);
            m_tfBackgrounds[leftIndex].position = t_selfPos;

            rightIndex = leftIndex;
            leftIndex = (leftIndex + 1) % arr_size;
        }
        else if (m_tfBackgrounds[rightIndex].position.x > m_rightPosX)
        {
            Vector3 t_selfPos = m_tfBackgrounds[rightIndex].position;
            //t_selfPos.Set(m_leftPosX, t_selfPos.y, t_selfPos.z);
            t_selfPos.Set(m_tfBackgrounds[leftIndex].position.x - t_length, t_selfPos.y, t_selfPos.z);
            m_tfBackgrounds[rightIndex].position = t_selfPos;

            leftIndex = rightIndex;
            rightIndex = (rightIndex - 1 + arr_size) % arr_size;
        }
    }
}
