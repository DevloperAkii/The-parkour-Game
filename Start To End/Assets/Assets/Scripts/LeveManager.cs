using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LeveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private TextMeshProUGUI progressText;

    private void Start()
    {
        background.SetActive(false);
    }
    public void LoadLevelWithLoadingBar(int levelIndex)
    {
        StartCoroutine(LoadLevelsync(levelIndex));
    }
    private IEnumerator LoadLevelsync(int levelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        while (!operation.isDone)
        {
            background.SetActive(true);

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressText.text = progress * 100 + "%";
            progressSlider.value = progress;

            yield return null;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
