using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System;
using TableTennis.Engines;

namespace TableTennis.Droid
{

    [Activity]
    public class TeamInputActivity : Activity
    {
        private EditText    Team1Name;
        private EditText    Team1Player1Name;
        private EditText    Team1Player2Name;
        private EditText    Team1Player3Name;
        private EditText    Team2Name;
        private EditText    Team2Player1Name;
        private EditText    Team2Player2Name;
        private EditText    Team2Player3Name;
        private Spinner     GamesSpinner;
        private Spinner     GoalSpinner;
        private Button      PlayButton;
        private IValidation Validator;

        /// <summary>
        /// Initialises this <see cref="Activity"/>.
        /// </summary>
        /// <param name="bundle">The <see cref="Bundle"/> passed from the previous <see cref="Activity"/>.</param>
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TeamInput);
            Team1Name        = FindViewById<EditText>(Resource.Id.Team1Name       );
            Team1Player1Name = FindViewById<EditText>(Resource.Id.Team1Player1Name);
            Team1Player2Name = FindViewById<EditText>(Resource.Id.Team1Player2Name);
            Team1Player3Name = FindViewById<EditText>(Resource.Id.Team1Player3Name);
            Team2Name        = FindViewById<EditText>(Resource.Id.Team2Name       );
            Team2Player1Name = FindViewById<EditText>(Resource.Id.Team2Player1Name);
            Team2Player2Name = FindViewById<EditText>(Resource.Id.Team2Player2Name);
            Team2Player3Name = FindViewById<EditText>(Resource.Id.Team2Player3Name);
            GamesSpinner     = FindViewById<Spinner >(Resource.Id.Games           );
            GoalSpinner      = FindViewById<Spinner >(Resource.Id.Goal            );
            PlayButton       = FindViewById<Button  >(Resource.Id.PlayButton      );

            Validator = new TeamInputValidation(
                () => Team1Name.Text,
                () => Team1Player1Name.Text,
                () => Team2Name.Text,
                () => Team2Player1Name.Text);

            Team1Name       .TextChanged += ValidateInput;
            Team1Player1Name.TextChanged += ValidateInput;
            Team2Name       .TextChanged += ValidateInput;
            Team2Player1Name.TextChanged += ValidateInput;
            PlayButton      .Click       += PlayButton_Click;

            InitializeSpinner(GamesSpinner, Resource.Array.games_options, Resource.String.default_games_option);
            InitializeSpinner(GoalSpinner , Resource.Array.goal_options , Resource.String.default_goal_option );
        }

        /// <summary>
        /// Validates the current input and enables or disables associated controls.
        /// </summary>
        private void ValidateInput(object sender, EventArgs e)
        {
            PlayButton.Enabled = Validator.IsValid();
        }

        /// <summary>
        /// Initialises the list for a <see cref="AbsSpinner"/> control.
        /// </summary>
        /// <param name="spinner">The <see cref="AbsSpinner"/> to initialise.</param>
        /// <param name="resourceItems">The resource string-array containing the items to add to the Spinner.</param>
        /// <param name="defaultSelectionId">The resource string containing the default item to display.</param>
        private void InitializeSpinner(AbsSpinner spinner, int resourceItems, int defaultSelectionId)
        {
            var adapter = ArrayAdapter.CreateFromResource(this, resourceItems, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            var defaultValue = Resources.GetString(defaultSelectionId);

            int i = spinner.Count - 1;
            while (i > 0 && !spinner.GetItemAtPosition(i).Equals(defaultValue))
                i--;
            spinner.SetSelection(i);
        }

        /// <summary>
        /// Sends the input to the scoreboard page.
        /// </summary>
        private void PlayButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ScoreboardActivity));
            intent.PutStringArrayListExtra("team1", new string[] {
                Team1Name.Text,
                Team1Player1Name.Text,
                Team1Player2Name.Text,
                Team1Player3Name.Text,
            });
            intent.PutStringArrayListExtra("team2", new string[] {
                Team2Name.Text,
                Team2Player1Name.Text,
                Team2Player2Name.Text,
                Team2Player3Name.Text,
            });
            intent.PutExtra("games", int.Parse(GamesSpinner.SelectedItem.ToString()));
            intent.PutExtra("goal" , int.Parse(GoalSpinner .SelectedItem.ToString()));
            StartActivity(intent);
        }

        /// <summary>
        /// Removes all view events and nulls all fields.
        /// </summary>
        /// <param name="disposing">True if this object is being disposed programatically; false if this object is being finalised.</param>
        #region protected override void Dispose(bool disposing)
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && !IsDisposed)
                {
                    Team1Name       .TextChanged -= ValidateInput;
                    Team1Player1Name.TextChanged -= ValidateInput;
                    Team2Name       .TextChanged -= ValidateInput;
                    Team2Player1Name.TextChanged -= ValidateInput;
                    PlayButton      .Click       -= PlayButton_Click;

                    Team1Name        = null;
                    Team1Player1Name = null;
                    Team1Player2Name = null;
                    Team1Player3Name = null;
                    Team2Name        = null;
                    Team2Player1Name = null;
                    Team2Player2Name = null;
                    Team2Player3Name = null;
                    GamesSpinner     = null;
                    GoalSpinner      = null;
                    PlayButton       = null;
                    Validator        = null;
                }
                IsDisposed = true;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        /// <summary>
        /// True if this object is disposed; otherwise, false.
        /// </summary>
        public bool IsDisposed { get; private set; } = false;
        #endregion
    }
}
