using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int EnemiesKilled = 0;
    public float TimeDelay = 3f;
    public float TimeDelayCounter = 0;
    public Slider TimeDelaySlider;
    public Text killText;
    public AudioSource explosionSourceAudio;
    public AudioClip[] explosionSounds;
    public GameObject endGameCanvas;
    public GameObject mainCanvas;
    public Text endGameText;
    public AudioClip defeated;
    // Start is called before the first frame update
    void Start()
    {
        endGameCanvas.SetActive(false);
        TimeDelayCounter = TimeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        SlowTime();
        TimeDelaySlider.value = TimeDelayCounter / TimeDelay;
        killText.text = "Enemies killed:  " + EnemiesKilled.ToString();
    }
    void SlowTime()
    {
        if (Input.GetKey(KeyCode.Space) && TimeDelayCounter > 0f)
        {
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = 0.02f * Time.deltaTime;
            TimeDelayCounter -= Time.fixedDeltaTime * 100f;
            if(TimeDelayCounter < 0f)
            {
                TimeDelayCounter = 0f;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) || TimeDelayCounter == 0f)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.deltaTime;
            TimeDelayCounter += Time.deltaTime / 0.1f;
        }
        if (!Input.GetKey(KeyCode.Space) && TimeDelayCounter <= TimeDelay)
        {
            TimeDelayCounter += Time.deltaTime * 0.2f;
        }
        if(TimeDelayCounter > 3f)
        {
            TimeDelayCounter = 3f;
        }
        
    }
    public void PlayExplosionSounds()
    {
        int random = UnityEngine.Random.Range(0, explosionSounds.Length);
        explosionSourceAudio.PlayOneShot(explosionSounds[random]);
    }
    public void EndGame()
    {
        explosionSourceAudio.PlayOneShot(defeated);
        endGameCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        endGameText.text = EnemiesKilled.ToString() + " enemies have been killed";
        
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
