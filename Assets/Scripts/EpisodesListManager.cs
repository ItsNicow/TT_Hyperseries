using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class EpisodesListManager : MonoBehaviour
{
    public static EpisodesListManager instance;

    public List<Episode> episodes;

    public GameObject episodePrefab;
    public GameObject episodeParent;

    public TMP_InputField newEpisodeName;
    public TMP_InputField newEpisodeViews;

    public VideoClip defaultEpisodeClip;
    public Texture defaultEpisodeThumbnail;

    void Awake()
    {
        if (!instance) instance = this;

        episodes = GetComponentsInChildren<Episode>().ToList();

        SelectFirstEpisode();
    }

    public void CreateEpisode()
    {
        GameObject ep = Instantiate(episodePrefab, episodeParent.transform);

        Episode episode = ep.GetComponent<Episode>();

        episode.episodeName = newEpisodeName.text;
        episode.views = newEpisodeViews.text == "" ? 0 : int.Parse(newEpisodeViews.text);
        episode.clip = defaultEpisodeClip;
        episode.thumbnail = defaultEpisodeThumbnail;

        episodes.Add(episode);

        newEpisodeName.text = "";
        newEpisodeViews.text = "";
    }

    public void SelectFirstEpisode()
    {
        episodes.First().Select();
    }
}
