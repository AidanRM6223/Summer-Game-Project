using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float afterImageDelay;
    private float afterImageDelaySeconds;
    public GameObject afterImage;
    public bool makeAfterImage = false;
    // Start is called before the first frame update
    void Start()
    {
        afterImageDelaySeconds = afterImageDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(makeAfterImage)
        {
            if(afterImageDelaySeconds>0)
            {
                afterImageDelaySeconds -= Time.deltaTime;
            }
            else {
                // Generate an after image
                GameObject currentAfterImage = Instantiate(afterImage, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentAfterImage.transform.localScale = this.transform.localScale;
                currentAfterImage.GetComponent<SpriteRenderer>().sprite = currentSprite;
                Destroy(currentAfterImage, 1f);
                afterImageDelaySeconds = afterImageDelay;
            }
        }
        
    }
}
