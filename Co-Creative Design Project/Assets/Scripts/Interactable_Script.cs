using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Script : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    [SerializeField] private GameObject occupiedTile = null;
    private const int tileLayer = 8;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));


        GetComponent<SpriteRenderer>().color = new Color32(120, 230, 255, 100); // changes the alpha
        GetComponent<CircleCollider2D>().enabled = false;
        GameManager.gMan.interactableSlot = gameObject;
        if (occupiedTile != null)
        {
            occupiedTile.GetComponent<Tile_Script>().gameObject.SetActive(true);
        }
    }

    void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = new Color32(120, 230, 255, 255); // changes the colour back to the original
        GetComponent<CircleCollider2D>().enabled = true;
        GameManager.gMan.interactableSlot = null;
        occupiedTile = null;
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == tileLayer)
        {
            var activeTile = GameManager.gMan.activeTile;
            if (activeTile == null) { return; }
            
            if (occupiedTile == null)
            {
                occupiedTile = activeTile.gameObject;
                gameObject.transform.position = occupiedTile.transform.position - new Vector3(0, 0, 1); // placed in front of grid
                occupiedTile.GetComponent<Tile_Script>().gameObject.SetActive(false);
                activeTile = null;
            }
        }
    }
}
