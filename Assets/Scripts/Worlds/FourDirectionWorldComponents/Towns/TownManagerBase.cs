public abstract class TownManagerBase : WorldManagerBase
{
    /* ==================== Fields ==================== */

    public static TownManagerBase Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public override void PauseGame(bool pause)
    {
        base.PauseGame(pause);

        // Player
        FourDirectionPlayerControl.Instance.PlayerPause(pause);
    }



    /* ==================== Protected Methods ==================== */

    protected override void DeleteInstance()
    {
        base.DeleteInstance();

        Instance = null;
        FourDirectionPlayerControl.Instance.DeleteInstance();
        CameraFourDirectionMovement.Instance.DeleteInstance();
    }


    protected override void Awake()
    {
        base.Awake();

        // Singleton pattern
        Instance = this;
    }
}