using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using TableTennis.Engines;
using TableTennis.Models;

namespace TableTennis.Droid
{
    public class GameFragment : Fragment
    {
        private const double TextSizeRatio = 0.75;

        protected bool IsCreated;
        protected bool IsDisposed;
        protected TextView Player1Score;
        protected TextView Player2Score;

        public Game Game
        {
            get { return _Game; }
            set
            {
                if (_Game == value)
                    return;
                _Game = value;
                PopulateGame();
            }
        }
        private Game _Game;
        private void PopulateGame()
        {
            if (!IsCreated)
                return;

            if (_Game == null)
            {
                Player1Score.Text = "-";
                Player2Score.Text = "-";
                View.Enabled = false;
                View.Visibility = ViewStates.Gone;
            }
            else
            {
                Player1Score.Text = _Game.Player1Score.ToString();
                Player2Score.Text = _Game.Player2Score.ToString();
                View.Enabled = false;
                View.Visibility = ViewStates.Visible;
            }
            View.RequestLayout();
        }

        public IGameReferee GameReferee { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.GameFragment, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Player1Score = View.FindViewById<TextView>(Resource.Id.gameFragment_player1Score);
            Player2Score = View.FindViewById<TextView>(Resource.Id.gameFragment_player2Score);
            IsCreated = true;

            View.LayoutChange += View_LayoutChange;
            Player1Score.Click += Player1Score_Click;
            Player2Score.Click += Player2Score_Click;
            Player1Score.GenericMotion += Player1Score_GenericMotion;
            Player2Score.GenericMotion += Player2Score_GenericMotion;

            PopulateGame();
        }

        protected virtual void OnScoreChanged()
        {
            Player1Score.Text = _Game.Player1Score.ToString();
            Player2Score.Text = _Game.Player2Score.ToString();

            var winner = GameReferee.Winner(Game);
            Player1Score.SetBackgroundColor(Resources.GetColor(winner == 1 ? Resource.Color.team1_win : Resource.Color.team1));
            Player2Score.SetBackgroundColor(Resources.GetColor(winner == 2 ? Resource.Color.team2_win : Resource.Color.team2));

            if (ScoreChanged != null)
                ScoreChanged();
        }
        public event Action ScoreChanged;

        private void View_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            int height = (e.Bottom - e.Top) / 2 - View.PaddingTop - View.PaddingBottom;
            int width = e.Right - e.Left - View.PaddingLeft - View.PaddingRight;
            int textSize = (int)(Math.Min(height, width) * TextSizeRatio);

            Player1Score.LayoutParameters.Height = height;
            Player2Score.LayoutParameters.Height = height;
            Player1Score.SetTextSize(ComplexUnitType.Px, textSize);
            Player2Score.SetTextSize(ComplexUnitType.Px, textSize);

            Player1Score.RequestLayout();
            Player2Score.RequestLayout();
        }

        private void Player1Score_Click(object sender, EventArgs e)
        {
            _Game.Player1Score++;
            OnScoreChanged();
        }

        private void Player2Score_Click(object sender, EventArgs e)
        {
            _Game.Player2Score++;
            OnScoreChanged();
        }

        private void Player1Score_GenericMotion(object sender, View.GenericMotionEventArgs e)
        {
            //TODO: Swipe player 1's score.
        }

        private void Player2Score_GenericMotion(object sender, View.GenericMotionEventArgs e)
        {
            //TODO: Swipe player 2's score.
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
                        Game = null;

                        Player1Score.Click -= Player1Score_Click;
                        Player2Score.Click -= Player2Score_Click;
                        Player1Score.GenericMotion -= Player1Score_GenericMotion;
                        Player2Score.GenericMotion -= Player2Score_GenericMotion;
                        View.LayoutChange -= View_LayoutChange;

                        Player1Score = null;
                        Player2Score = null;
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