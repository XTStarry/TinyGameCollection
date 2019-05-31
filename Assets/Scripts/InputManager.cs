using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
   {
       left,right,up,down
   }

public class InputManager : MonoBehaviour
{

    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gm.Move(MoveDirection.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gm.Move(MoveDirection.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gm.Move(MoveDirection.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gm.Move(MoveDirection.down);
        }
    }
}
