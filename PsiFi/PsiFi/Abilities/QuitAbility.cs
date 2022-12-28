using PsiFi.Interactions;

namespace PsiFi.Abilities
{
    /// <summary>
    /// The ability to quit the game.
    /// </summary>
    public class QuitAbility : Ability
    {
        public bool CanActivateAbility => true;
        public string Name => "Quit game";

        public IEnumerable<Interaction> ActivateAbility()
        {
            var confirmation = new YesNoInteraction("Quit the game?");
            yield return confirmation;
            if (confirmation.Response) Activated = true;
        }

        public bool Activated { get; private set; } = false;
    }
}
