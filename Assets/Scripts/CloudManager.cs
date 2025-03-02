using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class CloudManager : MonoBehaviour
{
    public GameObject cloudPrefab; // Bulut prefabý (UI Image olarak kullanýlacak)
    public RectTransform canvasTransform; // Canvas'ý referans alacaðýz
    public int cloudCount = 5; // Ekrandaki maksimum bulut sayýsý
    public float cloudSpeed = 100f; // Bulutlarýn kayma hýzý
    public float spawnRate = 2f; // Yeni bulutlarýn çýkma süresi
    public float startX = 900f; // Bulutlarýn saðdan baþlayacaðý X
    public float destroyX = -900f; // Bulutlarýn soldan kaybolacaðý X
    public float minY = -300f, maxY = 300f; // Bulutlarýn rastgele çýkacaðý Y

    private List<GameObject> cloudPool = new List<GameObject>();

    void Start()
    {
        // Havuzdaki bulutlarý oluþtur
        for (int i = 0; i < cloudCount; i++)
        {
            GameObject cloud = Instantiate(cloudPrefab, canvasTransform);
            cloud.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-700, 800), Random.Range(minY, maxY));
            cloudPool.Add(cloud);
            cloud.transform.SetSiblingIndex(2);
        }

        InvokeRepeating("SpawnCloud", 0f, spawnRate);
    }

    void Update()
    {
        foreach (GameObject cloud in cloudPool)
        {
            RectTransform rt = cloud.GetComponent<RectTransform>();
            rt.anchoredPosition += Vector2.left * cloudSpeed * Time.deltaTime;

            if (rt.anchoredPosition.x < destroyX)
            {
                ResetCloud(rt);
            }
        }
    }

    void SpawnCloud()
    {
        foreach (GameObject cloud in cloudPool)
        {
            if (!cloud.activeInHierarchy)
            {
                ResetCloud(cloud.GetComponent<RectTransform>());
                cloud.SetActive(true);
                return;
            }
        }
    }

    void ResetCloud(RectTransform rt)
    {
        rt.anchoredPosition = new Vector2(startX, Random.Range(minY, maxY));
    }
}
