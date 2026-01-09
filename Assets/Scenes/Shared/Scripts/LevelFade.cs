using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//some of the code for the fade was taken from here: https://medium.com/@Brian_David/how-to-create-screen-fade-transitions-in-unity-xr-for-vr-cutscenes-83174f598780#id_token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjRiYTZlZmVmNWUxNzIxNDk5NzFhMmQzYWJiNWYzMzJlMGY3ODcxNjUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIyMTYyOTYwMzU4MzQtazFrNnFlMDYwczJ0cDJhMmphbTRsamRjbXMwMHN0dGcuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIyMTYyOTYwMzU4MzQtazFrNnFlMDYwczJ0cDJhMmphbTRsamRjbXMwMHN0dGcuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMTEyNDA4NDg2NzUwOTgwMTc4NzYiLCJlbWFpbCI6ImFtaW5hLm90dGF3YUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwibmJmIjoxNzY3OTEzOTk0LCJuYW1lIjoiQW1pbmEgQSIsInBpY3R1cmUiOiJodHRwczovL2xoMy5nb29nbGV1c2VyY29udGVudC5jb20vYS9BQ2c4b2NLUEdHTTRDLTlaT2FvRmdBQzNWbkdlX2w3eDFJeDQ4XzE0TU1SczNKZk4wNUlERFE9czk2LWMiLCJnaXZlbl9uYW1lIjoiQW1pbmEiLCJmYW1pbHlfbmFtZSI6IkEiLCJpYXQiOjE3Njc5MTQyOTQsImV4cCI6MTc2NzkxNzg5NCwianRpIjoiZTkxN2FiNTVmM2YzYzNmOTRkYWY0YjFkNTY4NmVjYmNkMTlkZDAwMiJ9.i4Sm771UVsUv1XDUY67MX3QUCX88TKRfwSeDr-z_u_hk5E-LksBMzX2Gqpz29OAGtp_HtvrHf1rtBrcT3Hu59OEdYTXFtSValGMRnmHfL54n6OzSlUTmz0NmOwbwPy3-iGH-5Vl5pU5zHO-REIY2b6H7bBNF_ZfV86xlOt8X-Yy0wEJgQ6B4P5X1aM8ibSB8GvY9VRbQF3KOKQP96Cd5nUSsKnOhR3pJv42fDQWVgM-HQjv1cqdjqKAAIiMAipFbmNw8WXADAVFJhR9SlgUD7EDRjbReKhoVKM3TbP4FHly8QGuUNtKWln892pfFHDZBJ6TXT64LSxWeCKsO2aE3NQ
//and from here: https://www.youtube.com/watch?v=JCyJ26cIM0Y

public class LevelFade : MonoBehaviour
{
    public static LevelFade Instance;

    [SerializeField] private CanvasGroup fade_canvas_group;
    [SerializeField] private bool fade_in_at_start;
    [SerializeField] private GameObject locomotion;

    const float TARGET_FADE_TIME = 0.5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        //add fade in for level
        if(fade_in_at_start){
            StartCoroutine(ScreenFade(1f, 0f));
        }
    }

    public void LoadNextLevel() {
        StartCoroutine(NextLevel());
    }

    //load the next level
    private IEnumerator NextLevel()
    {
        // disable movement
        locomotion.SetActive(false);

        //fade to black and load next level
        yield return StartCoroutine(ScreenFade(0f, 1f));
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //operation.allowSceneActivation = false;

        //////wait until 
        ////while (operation.progress < 0.9f) { 
        ////    yield return null;
        ////}

        //operation.allowSceneActivation = true;
    }

    //fades the screen to black or out
    private IEnumerator ScreenFade(float start_alpha, float end_alpha)
    {
        float passed_time = 0f;
        while (passed_time < TARGET_FADE_TIME) {
            passed_time += Time.deltaTime;
            fade_canvas_group.alpha = Mathf.Lerp(start_alpha, end_alpha, passed_time / TARGET_FADE_TIME);
            yield return null;
        }
        fade_canvas_group.alpha = end_alpha;
    }
}
