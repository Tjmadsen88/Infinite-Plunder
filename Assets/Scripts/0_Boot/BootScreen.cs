using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScreen : MonoBehaviour
{
    public PersistentData persistentData;

    // Start is called before the first frame update
    void Start()
    {
        persistentData.removeTerrainColorData();

        SceneManager.LoadScene("TitleScene");
    }
}
