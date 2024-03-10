using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TableTennis.Models;
using TableTennis.Engines;
using Android.Graphics;

namespace TableTennis.Droid
{
    public class MatchFragment : Fragment
    {
        private const double TextSizeRatio = 0.5;

        protected bool IsCreated;
        protected bool IsDisposed;
        protected TextView Player1Name;
        protected TextView Player2Name;
        protected LinearLayout GamesLayout;
        protected List<GameFragment> GameFragments = new List<GameFragment>(5);

        public IMatchReferee MatchReferee { get; set; }

        public Match Match
        {
            get { return _Match; }
            set
            {
                if (_Match == value)
                    return;
                _Match = value;

                PopulateMatch();
            }
        }
        private Match _Match;
        private void PopulateMatch()
        {
            if (!IsCreated)
                return;

            foreach (var fragment in GameFragments)
            {
                fragment.ScoreChanged -= GameFragment_ScoreChanged;
                fragment.Dispose();
            }
            GameFragments.Clear();
            GamesLayout.RemoveAllViews();

            if (_Match != null)
            {
                Player1Name.Text = Match.Player1.Name;
                Player2Name.Text = Match.Player2.Name;

                using (var transaction = FragmentManager.BeginTransaction())
                {
                    foreach (var game in _Match.Games)
                    {
                        var fragment = new GameFragment
                        {
                            Game = game,
                            GameReferee = MatchReferee.GameReferee,
                        };
                        GameFragments.Add(fragment);
                        fragment.ScoreChanged += GameFragment_ScoreChanged;
                        transaction.Add(GamesLayout.Id, fragment);
                    }
                    transaction.Commit();
                }
            }
            else
            {
                Player1Name.Text = Resources.GetString(Resource.String.player_1_name_placeholder);
                Player2Name.Text = Resources.GetString(Resource.String.player_2_name_placeholder);
            }

            Player1Name.RequestLayout();
            Player2Name.RequestLayout();
            GamesLayout.RequestLayout();
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Player1Name = View.FindViewById<TextView>(Resource.Id.matchFragment_player1Name);
            Player2Name = View.FindViewById<TextView>(Resource.Id.matchFragment_player2Name);
            GamesLayout = View.FindViewById<LinearLayout>(Resource.Id.matchFragment_gamesLayout);
            IsCreated = true;

            View.LayoutChange += View_LayoutChange;

            PopulateMatch();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.MatchFragment, container, false);
        }

        protected virtual void OnScoreChanged()
        {
            if (ScoreChanged != null)
                ScoreChanged();
        }
        public event Action ScoreChanged;

        private void View_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            int height = (e.Bottom - e.Top) / 4;
            int textSize = (int)(height * TextSizeRatio);

            Player1Name.LayoutParameters.Height = height;
            Player2Name.LayoutParameters.Height = height;
            Player1Name.SetTextSize(ComplexUnitType.Px, textSize);
            Player2Name.SetTextSize(ComplexUnitType.Px, textSize);

            Player1Name.RequestLayout();
            Player2Name.RequestLayout();

            if (GameFragments.Count > 0)
            {
                int width = (e.Right - e.Left) / GameFragments.Count;
                foreach (var fragment in GameFragments)
                {
                    fragment.View.LayoutParameters.Width = width;
                    fragment.View.RequestLayout();
                }
            }
        }

        private void GameFragment_ScoreChanged()
        {
            var winner = MatchReferee.Winner(Match);
            Player1Name.SetBackgroundColor(Resources.GetColor(winner == 1 ? Resource.Color.team1_win : Resource.Color.team1));
            Player2Name.SetBackgroundColor(Resources.GetColor(winner == 2 ? Resource.Color.team2_win : Resource.Color.team2));
            OnScoreChanged();
        }

        /// <summary>
        /// Disposes of all resources used by this object.
        /// </summary>
        /// <param name="disposing">True to dispose of managed and unmanaged resources.  False to dispose of
        /// unmanaged resources only.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    IsCreated = false;
                    Match = null;

                    View.LayoutChange -= View_LayoutChange;

                    Player1Name = null;
                    Player2Name = null;
                    GamesLayout = null;
                }
                IsDisposed = true;
            }
        }
    }
}