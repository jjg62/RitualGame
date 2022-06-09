using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cursor object which follows player's mouse
public class MainCursor : MonoBehaviour
{
    [SerializeField]
    private MoveCursor moveCursor; //Cursor for moving totems

    [SerializeField]
    private GameObject defCursor; //Default cursor

    private GameObject currentCursor;

    public bool hovering;
    [HideInInspector]
    public TotemDragDrop moving;

    private void Start()
    {
        Cursor.visible = false;

        //List of types of cursors
        cursors = new GameObject[2];
        cursors[1] = moveCursor.gameObject;
        cursors[0] = defCursor;

        currentCursor = defCursor;
    }

    private void Update()
    {
        //Move to mouse position
        Vector2 pos = Input.mousePosition;
        transform.position = pos;
    }

    private GameObject[] cursors;

    //Activate a specific cursor a disable previously enabled one
    public void ChangeCursor(int id)
    {
        currentCursor.SetActive(false);

        cursors[id].gameObject.SetActive(true);

        currentCursor = cursors[id];
    }

    //Set cooldown for moving totems
    public void SetMoveCooldown(float val)
    {
        moveCursor.SetMoveCooldown(val);
    }

    //Track whether player is currently moving a totem
    public void SetMoving(TotemDragDrop moving)
    {
        this.moving = moving;
        moveCursor.SetMoving(moving != null);
    }

    public bool IsMoving()
    {
        return moving;
    }
}
