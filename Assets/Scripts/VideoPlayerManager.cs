using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    public static VideoPlayerManager instance;

    [HideInInspector] public Episode currentEpisode;
    public TMP_Text currentEpisodeName;
    public TMP_Text currentEpisodeViews;

    public Canvas canvas;

    //public GameObject videoPlayerAdditional;
    //public GameObject verticalVideoPlayer;
    //public GameObject horizontalVideoPlayer;
    //GameObject currentVideoPlayer;

    public VideoPlayer videoPlayer;
    public RectTransform videoPlayerArea;
    public Slider videoTimeSlider;
    public TMP_Text videoTime;

    public RawImage thumbnail;

    public Button playButton;
    public Button fullscreenButton;
    public RawImage playButtonImage;
    public RawImage fullscreenButtonImage;
    public RawImage pauseFade;

    public Texture pauseIcon;
    public Texture playIcon;
    public Texture replayIcon;
    public Texture fullscreenOnIcon;
    public Texture fullscreenOffIcon;

    public bool isPaused = true;
    public bool pauseScreen = true;
    public bool fullscreen = false;
    bool lockRotation = false;
    bool canReplay = false;

    public GameObject warningMessage;
    public Button validateButton;

    public SeriesInfoManager seriesInfoManager;
    public PageSwiper pageSwiper;

    void Awake()
    {
        if (!instance) instance = this;
    }

    void Start()
    {
        ShowWarningMessage(false);
        AdjustPlayer(false);
    }

    private void Update()
    {
        if (!isPaused) videoTimeSlider.value = (float)videoPlayer.time;
        else videoPlayer.time = videoTimeSlider.value;

        if ((Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) && !lockRotation)
        {
            AdjustPlayer(false);
            fullscreenButtonImage.texture = fullscreenOnIcon;
            fullscreen = false;

        }
        else if ((Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight) && !lockRotation)
        {
            AdjustPlayer(true);
            fullscreenButtonImage.texture = fullscreenOffIcon;
            fullscreen = true;
        }

        int hours = TimeSpan.FromSeconds((float)videoPlayer.time).Hours;
        int minutes = TimeSpan.FromSeconds((float)videoPlayer.time).Minutes;
        int seconds = TimeSpan.FromSeconds((float)videoPlayer.time).Seconds;

        if (hours == 0) videoTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        else videoTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        pageSwiper.canSwipe = !fullscreen;

        if (videoTimeSlider.value == videoTimeSlider.maxValue || (float)videoPlayer.time >= (float)videoPlayer.length - 0.1)
        {
            isPaused = true;
            canReplay = true;
            videoPlayer.Pause();

            playButtonImage.texture = replayIcon;
        }
        else
        {
            canReplay = false;

            if (isPaused) playButtonImage.texture = playIcon;
            else playButtonImage.texture = pauseIcon;
        }
    }

    public void Play()
    {
        if (isPaused)
        {
            if (videoPlayer.time == 0) ShowThumbnail(false);
            if (canReplay) videoPlayer.Stop();

            isPaused = false;
            videoPlayer.Play();

            StartCoroutine(PauseScreenIdle(true));
            pauseScreen = false;
        }
        else
        {
            isPaused = true;
            videoPlayer.Pause();

            pauseScreen = true;
        }
    }

    public void PauseScreen()
    {
        if (pauseScreen)
        {
            StartCoroutine(PauseScreenOverlay("out", 3f));
            pauseScreen = false;
        }
        else
        {
            StartCoroutine(PauseScreenOverlay("in", 3f));
            pauseScreen = true;

            if (!isPaused) StartCoroutine(ScreenOverlayIdleCheck());
        }
    }

    public void ShowWarningMessage(bool show)
    {
        if (EpisodesListManager.instance.episodes.Count == 1) validateButton.interactable = false;
        else validateButton.interactable = true;

        warningMessage.SetActive(show);
    }

    public void ShowThumbnail(bool show)
    {
        thumbnail.gameObject.SetActive(show);
    }

    public void Fullscreen()
    {
        AdjustPlayer(!fullscreen);

        if (fullscreen)
        {
            lockRotation = false;
            fullscreenButtonImage.texture = fullscreenOnIcon;
        }
        else
        {
            lockRotation = true;
            fullscreenButtonImage.texture = fullscreenOffIcon;
        }

        fullscreen = !fullscreen;
    }

    void AdjustPlayer(bool fullscreen)
    {
        if (fullscreen)
        {
            videoPlayerArea.sizeDelta = new Vector2(canvas.GetComponent<RectTransform>().rect.height - canvas.GetComponent<RectTransform>().rect.width, canvas.GetComponent<RectTransform>().rect.width);
            videoPlayerArea.anchoredPosition = new Vector2(canvas.GetComponent<RectTransform>().rect.width / 2, -canvas.GetComponent<RectTransform>().rect.height / 2);
            if (videoPlayerArea.rotation.z == 0) videoPlayerArea.transform.Rotate(axis: Vector3.forward, -90);
        }
        else
        {
            videoPlayerArea.sizeDelta = new Vector2(0, 607.5f);
            videoPlayerArea.anchoredPosition = new Vector2(0, -200);
            videoPlayerArea.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void ResetPlayerInfo(Episode episode)
    {
        videoPlayer.Stop();
        
        isPaused = true;
        pauseScreen = true;
        fullscreen = false;
        canReplay = false;

        currentEpisodeName.text = episode.episodeName;
        currentEpisodeViews.text = episode.views.ToString("N0").Replace(",", " ");

        pauseFade.color = new Color(pauseFade.color.r, pauseFade.color.g, pauseFade.color.b, 0.4f);

        playButton.enabled = true;
        fullscreenButton.enabled = true;
        playButtonImage.color = new Color(playButtonImage.color.r, playButtonImage.color.g, playButtonImage.color.b, 1);
        fullscreenButtonImage.color = new Color(fullscreenButtonImage.color.r, fullscreenButtonImage.color.g, fullscreenButtonImage.color.b, 1);
        playButtonImage.texture = playIcon;

        videoPlayer.clip = episode.clip;
        thumbnail.texture = episode.thumbnail;

        videoTimeSlider.maxValue = (float)videoPlayer.length;
        videoTimeSlider.value = (float)videoPlayer.time;

        ShowThumbnail(true);

        if (seriesInfoManager.seriesInfoVisible) StartCoroutine(seriesInfoManager.AdjustSeriesInfo());
    }

    public void DeleteCurrentEpisode()
    {
        currentEpisode.Delete();
        currentEpisode = null;
    }

    IEnumerator PauseScreenOverlay(string inOut, float speed)
    {
        UnityEngine.Color fadeColor = pauseFade.color;
        UnityEngine.Color playButtonColor = playButtonImage.color;
        UnityEngine.Color fullscreenButtonColor = fullscreenButtonImage.color;

        if (inOut == "in")
        {
            for (float i = 0; i <= 1; i += Time.deltaTime * speed)
            {
                playButtonColor.a = i;
                playButtonImage.color = playButtonColor;
                fullscreenButtonColor.a = i;
                fullscreenButtonImage.color = fullscreenButtonColor;
                if (i <= 0.4f)
                {
                    fadeColor.a = i;
                    pauseFade.color = fadeColor;
                }

                yield return null;
            }

            playButtonColor.a = 1;
            playButtonImage.color = playButtonColor;
            fullscreenButtonColor.a = 1;
            fullscreenButtonImage.color = fullscreenButtonColor;
            fadeColor.a = 0.4f;
            pauseFade.color = fadeColor;

            yield return fullscreenButton.enabled = true;
            yield return playButton.enabled = true;
        }
        else if (inOut == "out")
        {
            yield return playButton.enabled = false;
            yield return fullscreenButton.enabled = false;

            for (float i = 1; i >= 0; i -= Time.deltaTime * speed)
            {
                playButtonColor.a = i;
                playButtonImage.color = playButtonColor; 
                fullscreenButtonColor.a = i;
                fullscreenButtonImage.color = fullscreenButtonColor;
                if (i <= 0.4f)
                {
                    fadeColor.a = i;
                    pauseFade.color = fadeColor;
                }

                yield return null;
            }

            playButtonColor.a = 0;
            playButtonImage.color = playButtonColor;
            fullscreenButtonColor.a = 0;
            fullscreenButtonImage.color = fullscreenButtonColor;
            fadeColor.a = 0;
            pauseFade.color = fadeColor;
        }
    }

    IEnumerator PauseScreenIdle(bool wait)
    {
        if (wait) yield return new WaitForSeconds(1.5f);

        StartCoroutine(PauseScreenOverlay("out", 2.5f));
    }

    IEnumerator ScreenOverlayIdleCheck()
    {
        float timer = 0;
        bool wasPaused = false;

        while (timer < 1)
        {
            timer += Time.deltaTime;
            if (isPaused || currentEpisode.clip != videoPlayer.clip) wasPaused = true;

            yield return null;
        }

        if (!wasPaused)
        {
            StartCoroutine(PauseScreenIdle(false));
            pauseScreen = false;
        }
    }
}
