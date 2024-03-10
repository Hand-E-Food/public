using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TableTennis.Models;
using TableTennis.Engines;
using System.Collections.Generic;
using Android.Util;

namespace TableTennis.Droid
{
    [Activity]
    public class ScoreboardActivity : Activity
    {
        private const int DefaultGames = 3;
        private const int DefaultGoal = 21;
        private const double TextSizeRatio = 0.5;

        protected bool IsCreated;
        protected bool IsDisposed;
        protected LinearLayout MatchesLayout;
        protected TextView Team1Name;
        protected TextView Team2Name;
        protected RelativeLayout ThisView;
        protected List<MatchFragment> MatchFragments = new List<MatchFragment>(9);

        private IContestReferee ContestReferee;

        public RoundRobinContest Contest
        {
            get { return _Contest; }
            set
            {
                if (_Contest == value)
                    return;
                _Contest = value;
                PopulateContest();
            }
        }
        private RoundRobinContest _Contest;
        private void PopulateContest()
        {
            if (!IsCreated)
                return;

            foreach (var fragment in MatchFragments)
            {
                fragment.ScoreChanged -= MatchFragment_ScoreChanged;
                fragment.Dispose();
            }
            MatchFragments.Clear();
            MatchesLayout.RemoveAllViews();

            if (_Contest != null)
            {
                Team1Name.Text = Contest.Team1.Name;
                Team2Name.Text = Contest.Team2.Name;

                using (var transaction = FragmentManager.BeginTransaction())
                {
                    foreach (var match in _Contest.Matches)
                    {
                        var fragment = new MatchFragment
                        {
                            Match = match,
                            MatchReferee = ContestReferee.MatchReferee,
                        };
                        MatchFragments.Add(fragment);
                        fragment.ScoreChanged += MatchFragment_ScoreChanged;
                        transaction.Add(MatchesLayout.Id, fragment);
                    }
                    transaction.Commit();
                }
            }
            else
            {
                Team1Name.Text = Resources.GetString(Resource.String.team_1_name_placeholder);
                Team2Name.Text = Resources.GetString(Resource.String.team_2_name_placeholder);
            }
            MatchesLayout.RequestLayout();
        }

        /// <summary>
        /// Initialises this <see cref="Activity"/>.
        /// </summary>
        /// <param name="bundle">The <see cref="Bundle"/> passed from the previous <see cref="Activity"/>.</param>
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            Team team1 = Transform.ToTeam(Intent.GetStringArrayListExtra("team1"));
            Team team2 = Transform.ToTeam(Intent.GetStringArrayListExtra("team2"));
            int games = Intent.GetIntExtra("games", DefaultGames);
            int goal = Intent.GetIntExtra("goal", DefaultGoal);

            SetContentView(Resource.Layout.Scoreboard);
            ThisView = FindViewById<RelativeLayout>(Resource.Id.scoreboard_rootLayout);
            MatchesLayout = FindViewById<LinearLayout>(Resource.Id.scoreboard_matchesLayout);
            Team1Name = FindViewById<TextView>(Resource.Id.scoreboard_team1Name);
            Team2Name = FindViewById<TextView>(Resource.Id.scoreboard_team2Name);

            var gameReferee = new WinGameBy2PointsReferee(goal);
            var matchReferee = new WinMatchByMajorityReferee(gameReferee);
            ContestReferee = new WinContestByMajorityReferee(matchReferee);

            ThisView.LayoutChange += View_LayoutChange;
            ThisView.RequestLayout();

            IsCreated = true;

            Contest = new RoundRobinContest(team1, team2, games);
        }

        private void View_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            int height = (e.Bottom - e.Top) / 6;
            int width = e.Right - e.Left;
            int textSize = (int)(height * TextSizeRatio);

            Team1Name.LayoutParameters.Height = height;
            Team2Name.LayoutParameters.Height = height;
            Team1Name.SetTextSize(ComplexUnitType.Px, textSize);
            Team2Name.SetTextSize(ComplexUnitType.Px, textSize);

            foreach (var fragment in MatchFragments)
            {
                fragment.View.LayoutParameters.Width = width;
                fragment.View.RequestLayout();
            }   

            Team1Name.RequestLayout();
            Team2Name.RequestLayout();
            MatchesLayout.RequestLayout();
        }

        private void MatchFragment_ScoreChanged()
        {
            var winner = ContestReferee.Winner(Contest);
            Team1Name.SetBackgroundColor(Resources.GetColor(winner == 1 ? Resource.Color.team1_win : Resource.Color.team1));
            Team2Name.SetBackgroundColor(Resources.GetColor(winner == 2 ? Resource.Color.team2_win : Resource.Color.team2));
        }

        /// <summary>
        /// Disposes of all resources used by this object.
        /// </summary>
        /// <param name="disposing">True to dispose of managed and unmanaged resources.  False to dispose of
        /// unmanaged resources only.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (disposing)
                    {
                        IsCreated = false;
                        Contest = null;

                        ThisView.LayoutChange -= View_LayoutChange;

                        ContestReferee = null;
                        MatchesLayout = null;
                        Team1Name = null;
                        Team2Name = null;
                        ThisView = null;
                    }
                    IsDisposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
