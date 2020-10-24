using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float scroll_Speed = 0.1f;
    private MeshRenderer meshRenderer;
    public int sortingOrder = -10;
    public Rigidbody2D playerRB;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerRB.velocity = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.sortingLayerName = "Background";
        meshRenderer.sortingOrder = sortingOrder;
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        float x = Time.deltaTime * scroll_Speed;
        PlayerController playerController = playerRB.gameObject.GetComponent<PlayerController>();
        if(Input.GetKey(KeyCode.LeftArrow) && playerRB.velocity.x < 0)
        {
            Vector2 offset = new Vector2(meshRenderer.sharedMaterial.mainTextureOffset.x - x, 0);
            meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
        else if(Input.GetKey(KeyCode.RightArrow) && playerRB.velocity.x > 0)
        {
            Vector2 offset = new Vector2(meshRenderer.sharedMaterial.mainTextureOffset.x + x, 0);
            meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
        if(playerRB.gameObject.GetComponent<PlayerController>().m_currMovingPlatform != null)
        {
            print("Is on a moving platform!");
            if(!Input.GetKey(KeyCode.LeftArrow) && playerController.m_currMovingPlatform.GetComponent<MovingPlatform>().isMovingHorizontallyLeft)
            {
                Vector2 offset = new Vector2(meshRenderer.sharedMaterial.mainTextureOffset.x - x / 1.5f, 0);
                meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
            }
            else if(!Input.GetKey(KeyCode.RightArrow) && playerController.m_currMovingPlatform.GetComponent<MovingPlatform>().isMovingHorizontallyRight)
            {
                Vector2 offset = new Vector2(meshRenderer.sharedMaterial.mainTextureOffset.x + x / 1.5f, 0);
                meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
            }
            
            else {
                Vector2 offset = new Vector2(meshRenderer.sharedMaterial.mainTextureOffset.x + 0, 0);
                meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
            }
            
        }

    }
    private void OnDisable() {
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0,0));

    }
}
