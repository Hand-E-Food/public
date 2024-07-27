using StarCoder.View;

namespace StarCoder;

public class GameEngine
{
    /// <summary>
    /// Starts a new game.
    /// </summary>
    /// <param name="gameSettings">The game's settings.</param>
    public GameEngine(GameSettings gameSettings)
    {
        Settings = gameSettings;
        State = new(gameSettings);
    }

    private readonly MainScreen mainScreen = new();

    /// <summary>
    /// This game's settings.
    /// </summary>
    public GameSettings Settings { get; }

    /// <summary>
    /// The game's current state.
    /// </summary>
    public GameState State { get; }

    /// <summary>
    /// Plays the game.
    /// </summary>
    public void PlayGame()
    {
        InitialiseScreen();
        while (GameContinues())
        {
            AdjustWeek();
            AdjustDifficulty();
            PublishContracts();
            PlanWeek();
            ExecutePlan();
            AdjustBurnout();
            ReplenishHand();
        }
    }

    private void InitialiseScreen()
    {
        mainScreen.Focus();
        mainScreen.DrawWeek(State.Week);
        mainScreen.DrawBurnout(State.Coder.Burnout);
        mainScreen.DrawHand(State.Coder.Languages.Hand);
    }

    private void AdjustWeek()
    {
        mainScreen.DrawWeek(++State.Week);
    }

    private void AdjustDifficulty()
    {
        //TODO: Increase the difficulty over time.
    }

    private void PublishContracts()
    { }

    private void PlanWeek()
    { }

    private void ExecutePlan()
    { }

    private void AdjustBurnout()
    {
        State.Coder.Burnout = Math.Max(0, State.Coder.Burnout + Settings.BurnoutThreshold - State.Coder.Languages.Hand.Count);
        mainScreen.DrawBurnout(State.Coder.Burnout);
    }

    private void ReplenishHand()
    {
        State.Coder.Languages.Draw(Math.Min(Settings.DrawPerTurn, Settings.HandLimit - State.Coder.Languages.Hand.Count));
        mainScreen.DrawHand(State.Coder.Languages.Hand);
    }

    private bool GameContinues()
    {
        if (State.Week >= 52 * 50)
        {
            mainScreen.DrawMessage("Happy 70th birthday! Time to retire. Game over!");
            return false;
        }
        return true;
    }
}
