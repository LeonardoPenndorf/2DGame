using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToCamera : MonoBehaviour
{
    [SerializeField] string PlayerTag, confinerTag;
    [SerializeField] CinemachineVirtualCamera[] cvcArray;

    // Start is called before the first frame update
    void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag(PlayerTag).transform;

        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag(confinerTag).GetComponent<PolygonCollider2D>();

        for (int i = 0; i < cvcArray.Length; i++)
        {
            cvcArray[i].Follow = player;

            var confiner = cvcArray[i].GetComponent<CinemachineConfiner>();
            confiner.m_BoundingShape2D = confinerShape;
        }
    }
}
