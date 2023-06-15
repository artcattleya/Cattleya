using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gallery : MonoBehaviour
{
    private Image [] _images;
    private bool[] _isLoading;
    private int _maxCount = 66;

    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private GameObject _preview;
    private Image _previewImage;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _images = new Image[_maxCount];
        _isLoading = new bool[_maxCount];

        Image imageTemplate = Resources.Load<Image>("ImageGorGallery");
        float size = Screen.width / 3f;
        imageTemplate.rectTransform.sizeDelta = new Vector2(size, size);

        int posY = 0;
        for (int i = 0; i < _maxCount; i++)
        {
            Image newImage = Instantiate(imageTemplate, transform);
            int posX;
            if (i % 2 == 0)
            {
                posX = - Screen.width / 4;
            }
            else
            {
                posX = Screen.width / 4;
                //posY -= (int)size + Screen.width / 6;
            }
            newImage.rectTransform.localPosition = new Vector3(posX, posY);

            _images[i] = newImage;
            _isLoading[i] = false;

            _images[i].GetComponent<GalleryImage>().id = i;
           

            if (i % 2 != 0) posY -= (int)size + Screen.width / 6;
        }

        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(0, -1 * posY);
        _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.x, 0, 0);

    }

    private void Start()
    {
        _preview.SetActive(false);
        _previewImage = _preview.transform.Find("Image").GetComponent<Image>();

        int i = GetLastVisibleImageWithScrollPos(Screen.height);
        _progressBar.gameObject.SetActive(true);
        StartCoroutine(LoadSeveralImagesWithProgress(i));
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }

    public void OnScroll ()
    {
        int i1 = GetLastVisibleImageWithScrollPos((int)_rectTransform.localPosition.y + Screen.height);
        if (i1 >= _maxCount)
        {
            i1 = _maxCount-1;
        }
        if (!_isLoading[i1])
        {
            for (int i = 0; i <= i1; i++)
            {
                if (!_isLoading[i])
                {
                    StartCoroutine(LoadImageFromServer(i));
                }
            }
        }

    }


    private int GetLastVisibleImageWithScrollPos (int pos)
    {
        int count = 0;
        for (int i = 0; i < _maxCount; i++)
        {
            if (-1 * _images[i].rectTransform.localPosition.y < pos)
            {
                count++;
            }
        }
        return count;
    }


    IEnumerator LoadSeveralImagesWithProgress(int count)
    {
        _progressBar.SetPercent(0);
        float persentStep = 100f / count;
        float lastPersent = 0f;

        for (int i = 0; i < count; i++)
        {
            if (_isLoading[i]) continue;

            yield return StartCoroutine(LoadImageFromServer(i));
            lastPersent += persentStep;
            _progressBar.SetPercent(lastPersent);
        }

        yield return new WaitForSeconds(1);

        _progressBar.gameObject.SetActive(false);
    }

    IEnumerator LoadImageFromServer(int index)
    {
        if (index >= _images.Length) yield break;
        if (_isLoading[index]) yield break;
        
        _isLoading[index] = true;

        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://data.ikppbb.com/test-task-unity-data/pics/" + (index+1) + ".jpg");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D t = ((DownloadHandlerTexture)www.downloadHandler).texture;
            _images[index].sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        }
    }


    public void OnClickImage (int index)
    {
        Sprite s = _images[index].sprite;
        if (s == null) return;
        _preview.SetActive(true);
        _previewImage.sprite = s;

        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    public void OnClickBack ()
    {
        _preview.SetActive(false);
        Screen.orientation = ScreenOrientation.Portrait;
    }

}
