using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    public bool activate = false;

    [SerializeField]
    private SpriteRenderer rangeSpriteRenderer;

    public static Hover self;

    private void Awake()
    {
        if (self == null)
        {
            self = this;
        }
        else
        {
            Destroy(this);
        }
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {

        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

    }

    public void Activate(Sprite sprite)
    {
        activate = true;

        this.rangeSpriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
    }

    public void Deactivate()
    {
        activate = false;

        rangeSpriteRenderer.enabled = false;
        spriteRenderer.enabled = false;
        GameManager.self.clickedBtn = null;
    }


}
