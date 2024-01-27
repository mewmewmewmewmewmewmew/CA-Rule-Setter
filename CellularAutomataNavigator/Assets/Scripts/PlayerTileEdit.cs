using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerTileEdit : MonoBehaviour
{
    public GameObject player;
    public Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    public Tilemap lifeMap = null;
    public Tile hoverTile = null;
    public  Tile lifeTile = null;


    private Vector3Int previousMousePos = new Vector3Int();

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos))
        {
            interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
            interactiveMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }

        // Left mouse click -> add path tile
        if (Input.GetMouseButton(0)/* && mousePos.y > player.transform.position.y - 170 && mousePos.y < player.transform.position.y + 170*/)
        {
            lifeMap.SetTile(mousePos, lifeTile);
        }

        // Right mouse click -> remove path tile
        if (Input.GetMouseButton(1)/* && mousePos.y > player.transform.position.y - 170 && mousePos.y < player.transform.position.y + 170*/)
        {
            lifeMap.SetTile(mousePos, null);
        }
    }

    public void ClearAll()
    {
        
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
}
//Scripting support from https://lukashermann.dev/writing/unity-highlight-tile-in-tilemap-on-mousever/

