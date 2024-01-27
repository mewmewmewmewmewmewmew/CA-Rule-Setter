//Very simple 8 directional movement with legacy inputs
//Flippable YZ axis for use in 2D projects or 3D projects
//Not dash friendly

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EightDirectionalMovement3D : MonoBehaviour
{

    Vector3 position;
    public bool UseZAxis;
    public int speed;

    private void Start()
    {
       position = transform.position;
    }


    void Update()
    {
        if (UseZAxis)
        {
            if (Input.GetKey(KeyCode.W))
            {
                position = new Vector3(position.x, position.y, position.z + (speed * Time.deltaTime));
            }

            if (Input.GetKey(KeyCode.S))
            {
                position = new Vector3(position.x, position.y, position.z - (speed * Time.deltaTime));
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                position = new Vector3(position.x, position.y + (speed * Time.deltaTime), position.z);
            }

            if (Input.GetKey(KeyCode.S))
            {
                position = new Vector3(position.x, position.y - (speed * Time.deltaTime), position.z);
            }

        }

        if (Input.GetKey(KeyCode.A))
        {
            position = new Vector3(position.x - (speed * Time.deltaTime), position.y , position.z);
        }

        if (Input.GetKey(KeyCode.D))
        {
            position = new Vector3(position.x + (speed * Time.deltaTime), position.y , position.z);
        }

        transform.position = position;
    }

   
}
