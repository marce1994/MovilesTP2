public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        LevelManager.Instance.BeginLevel();
    }
    

}
