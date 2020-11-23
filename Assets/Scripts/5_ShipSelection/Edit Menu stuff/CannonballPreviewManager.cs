using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonballPreviewManager : MonoBehaviour
{
    public GameObject cannonballPreview_object;

    private RectTransform cannonballPreview_transform;
    private Image cannonballPreview_image;
    private CanvasGroup cannonballPreview_group; 

    private const byte slideInAnimationTime = 6;
    private const byte slideOutAnimationTime = 12;

    private bool previewIsVisible = false;


    // Start is called before the first frame update
    void Start()
    {
        cannonballPreview_transform = cannonballPreview_object.GetComponent<RectTransform>();
        cannonballPreview_image = cannonballPreview_object.GetComponent<Image>();
        cannonballPreview_group = cannonballPreview_object.GetComponent<CanvasGroup>();
    }


    public void changeImageScale(float newScale)
    {
        cannonballPreview_transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void changeImageColor(Color32 cannonballColor)
    {
        cannonballPreview_image.color = cannonballColor;
    }

    

    public bool requestMakeAppear()
    {
        if (!previewIsVisible)
        {
            previewIsVisible = true;

            StartCoroutine("playSlideInAnimation_group");
            return true;
        }
        
        return false;
    }

    public bool requestMakeDisappear()
    {
        if (previewIsVisible)
        {
            previewIsVisible = false;
            StartCoroutine("playSlideOutAnimation_group");

            return true;
        }
        
        return false;
    }

    IEnumerator playSlideInAnimation_group()
    {
        float amountToFade = 1f / (float)slideInAnimationTime;

        cannonballPreview_group.alpha = 0f;

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            cannonballPreview_group.alpha += amountToFade;
            yield return null;
        }

        cannonballPreview_group.alpha = 1f;
    }


    IEnumerator playSlideOutAnimation_group()
    {
        float amountToFade = 1f / (float)slideInAnimationTime;

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            cannonballPreview_group.alpha -= amountToFade;
            yield return null;
        }

        cannonballPreview_group.alpha = 0f;
    }
}
