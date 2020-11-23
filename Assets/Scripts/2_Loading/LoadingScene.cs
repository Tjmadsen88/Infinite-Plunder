using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System; //temp, for timekeeping...

public class LoadingScene : MonoBehaviour
{
    public PersistentData persistentData;
    private const byte countdown_max = 20;
    private byte countdown_current = 5;

    private const byte animationFrames_delayBeforeFadeIn = 20;
    private const byte animationFrames_fadeIn = 20;
    private const byte animationFrames_fadeOut = 10;
    // private readonly Vector3 wheelRotationAmount = new Vector3(0f, 0f, -1.5f);
    private readonly Vector3 wheelRotationAmount = new Vector3(0f, 0f, -1.2f);

    public GameObject uiCanvas;
    public CanvasGroup uiCanvasGroup;

    public Image loadingTextImage;

    public GameObject steeringWheelObject;
    private Image SteeringWheelImage;
    private Transform steeringWheelTransform;

    long startTime;


    // Start is called before the first frame update
    void Start()
    {
        startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // persistentData.createLoadingDataWithThread();
        
        steeringWheelTransform = steeringWheelObject.GetComponent<Transform>();
        SteeringWheelImage = steeringWheelObject.GetComponent<Image>();

        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(uiCanvas);

        StartCoroutine("playFadeImageryInAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        steeringWheelTransform.eulerAngles += wheelRotationAmount;

        if (--countdown_current <= 5)
        {
            if (persistentData.getHasLoadingData())
            {
                SceneManager.LoadScene("GameScene");
                
                StartCoroutine("playFadeImageryOutAnimation");

                countdown_current = 255;
            } else {
                countdown_current = countdown_max;
            }
        }
    }

    

    IEnumerator playFadeImageryInAnimation()
    {
        byte amountToFade = (byte)(255 / animationFrames_fadeIn);

        for (byte index = 1; index<=animationFrames_delayBeforeFadeIn; index++) 
        {
            yield return null;
        }

        for (byte index = 1; index<=animationFrames_fadeIn; index++) 
        {
            SteeringWheelImage.color = new Color32(255, 255, 255, (byte)(amountToFade * index));
            loadingTextImage.color = new Color32(255, 255, 255, (byte)(amountToFade * index));
            yield return null;
        }
        
        SteeringWheelImage.color = new Color32(255, 255, 255, 255);
        loadingTextImage.color = new Color32(255, 255, 255, 255);
    }

    IEnumerator playFadeImageryOutAnimation()
    {
        Debug.Log("Finished loading after "+(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()-startTime)+" miliseconds.");

        float amountToFade = (1f / (float)animationFrames_fadeOut);

        for (byte index = 1; index<=animationFrames_fadeOut; index++) 
        {
            uiCanvasGroup.alpha = 1f - amountToFade * index;
            yield return null;
        }
        
        Destroy(uiCanvas);
        Destroy(this.gameObject);
    }
}
