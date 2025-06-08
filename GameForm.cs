using Game_Radionova.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Media;

namespace Game_Radionova
{
    public partial class GameForm : Form
    {
        private GameSettings settings;
        private GameEngine engine;
        private Game game;
        private GameController controller;

        private List<TileView> tileViews;

        private CountdownTimer countdownTimer;
        private BackgroundMusicPlayer musicPlayer;

        private Player player1;
        private Player player2;

        private TableLayoutPanel gamePanel;
        private Label timeLabel;
        private Label scoreLabel;
        private Label currentPlayerLabel;
        private Panel topPanel;

        private Timer revealTimer;
        private TileView firstTile, secondTile;

        private bool isMultiplayer => settings.IsMultiplayer;

        public GameForm(GameSettings settings)
        {
            InitializeComponent();
            this.settings = settings;

            engine = new GameEngine(settings);
            var cards = engine.GenerateCards();
            player1 = new Player("Игрок 1");
            player2 = isMultiplayer ? new Player("Игрок 2") : null;

            game = new Game(cards, isMultiplayer ? new List<Player> { player1, player2 } : new List<Player> { player1 });
            controller = new GameController(game);

            controller.OnMatchChecked += HandleMatchChecked;
            controller.OnTurnChanged += player => UpdatePlayerLabel();
            controller.OnGameFinished += ShowGameResult;

            InitUI();
            StartGame();
        }

        private void InitUI()
        {
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Color.DarkSlateGray
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            topPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(64, 64, 64)
            };

            currentPlayerLabel = new Label
            {
                Text = isMultiplayer ? "Ход игрока 1" : "Одиночная игра",
                Font = new Font("Courier New", 12, FontStyle.Bold),
                ForeColor = Color.Lime,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            topPanel.Controls.Add(currentPlayerLabel);

            if (isMultiplayer)
            {
                scoreLabel = new Label
                {
                    Text = "Очки: 0 - 0",
                    Font = new Font("Courier New", 10),
                    ForeColor = Color.LightCyan,
                    AutoSize = true,
                    Location = new Point(10, 35)
                };
                topPanel.Controls.Add(scoreLabel);
            }
            else
            {
                timeLabel = new Label
                {
                    Font = new Font("Courier New", 12, FontStyle.Bold),
                    ForeColor = Color.Lime,
                    AutoSize = true,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Text = "Время: 00:00",
                    Location = new Point(10, 10)
                };
                topPanel.Controls.Add(timeLabel);
            }

            gamePanel = GameBoardRenderer.Render(game, engine.Rows, engine.Columns, OnTileClicked, out tileViews);

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(gamePanel, 0, 1);
            Controls.Add(mainLayout);
        }

        private void StartGame()
        {
            InitRevealTimer();
            InitMusic();

            if (!isMultiplayer)
                InitTimer();
        }

        private void InitRevealTimer()
        {
            revealTimer = new Timer { Interval = 1000 };
            revealTimer.Tick += (s, e) =>
            {
                revealTimer.Stop();

                if (firstTile != null && !firstTile.Card.IsMatched)
                    firstTile.Hide();
                if (secondTile != null && !secondTile.Card.IsMatched)
                    secondTile.Hide();

                controller.ResetTurn();
                ResetSelection();
            };
        }

        private void InitTimer()
        {
            countdownTimer = new CountdownTimer(TimeSpan.FromMinutes(2));
            countdownTimer.TimeUpdated += remaining =>
            {
                timeLabel.Text = $"Время: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
                timeLabel.Font = remaining.TotalSeconds <= 10
                    ? new Font("Courier New", 16, FontStyle.Bold)
                    : new Font("Courier New", 12, FontStyle.Bold);
                timeLabel.ForeColor = remaining.TotalSeconds <= 10 ? Color.Red : Color.Lime;
            };
            countdownTimer.TimeEnded += () =>
            {
                MessageBox.Show("Время вышло! Попробуйте снова.", "Проигрыш");
                Close();
            };
            countdownTimer.Start();
        }

        private void InitMusic()
        {
            musicPlayer = new BackgroundMusicPlayer();
            musicPlayer.Play("retro_theme.wav");
        }

        private void OnTileClicked(TileView tile)
        {
            if (revealTimer.Enabled) return;

            if (!controller.SelectCard(tile.Card)) return;

            tile.Update();

            if (firstTile == null)
            {
                firstTile = tile;
            }
            else if (secondTile == null && tile != firstTile)
            {
                secondTile = tile;
            }
        }

        private void HandleMatchChecked(Card card1, Card card2, bool isMatch)
        {
            var tile1 = tileViews.FirstOrDefault(t => t.Card == card1);
            var tile2 = tileViews.FirstOrDefault(t => t.Card == card2);

            if (tile1 == null || tile2 == null)
                return;

            if (isMatch)
            {
                tile1.ShowMatch();
                tile2.ShowMatch();
                UpdateScore();
                controller.ResetTurn();
                ResetSelection();
            }
            else
            {
                firstTile = tile1;
                secondTile = tile2;

                revealTimer.Start();
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateScore();
            UpdatePlayerLabel();
        }

        private void ResetSelection()
        {
            firstTile = null;
            secondTile = null;
        }

        private void UpdateScore()
        {
            if (isMultiplayer)
                scoreLabel.Text = $"Очки: {player1.Score} - {player2.Score}";
        }

        private void UpdatePlayerLabel()
        {
            if (isMultiplayer)
                currentPlayerLabel.Text = $"Ход игрока {game.CurrentPlayerIndex + 1}";
        }

        private void ShowGameResult(Player p1, Player p2)
        {
            countdownTimer?.Stop();
            musicPlayer?.Stop();

            GameResultDisplayer.Show(game, p1, p2);
            Close();
        }
    }
}