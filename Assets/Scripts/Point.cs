using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private GameEvent pickedPoint;
    [SerializeField] private float speed;

    private GameObject player;
    private GameObject child;
    private float count;

    private void Start()
    {
        child = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + Vector3.up * 0.5f, count);
            count += Time.deltaTime * speed;
        }
        if (child == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            if(GetComponent<ObjectMovement>() != null)
            {
                GetComponent<ObjectMovement>().enabled = false;
            }
        }
    }
}
