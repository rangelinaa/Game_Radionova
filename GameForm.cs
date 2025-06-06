using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Game_Radionova.Logic;
using System.Media;

namespace Game_Radionova
{
    public partial class GameForm : Form
    {
        private SoundPlayer backgroundMusic;

        private GameSettings settings;
        private GameEngine engine;

        private PictureBox firstClicked = null;
        private PictureBox secondClicked = null;
        private Timer revealTimer;

        private bool isMultiplayer;
        private int currentPlayer = 1;
        private Label currentPlayerLabel;

        private int scorePlayer1 = 0;
        private int scorePlayer2 = 0;
        private Label scoreLabel;

        private Timer gameTimer;
        private TimeSpan elapsedTime;
        private TimeSpan maxTime = TimeSpan.FromMinutes(2);

        private TableLayoutPanel tableLayoutPanel1;
        private Label timeLabel;
        private Panel topPanel;

        public GameForm(GameSettings settings)
        {
            InitializeComponent();

            backgroundMusic = new SoundPlayer("Resources/retro_theme.wav");
            backgroundMusic.PlayLooping();


            this.settings = settings;
            this.isMultiplayer = settings.IsMultiplayer;

            engine = new GameEngine(settings);

            InitRevealTimer();
            InitUI();
            InitGameField();

            if (!isMultiplayer)
                InitializeTimer();
        }

        private void InitUI()
        {
            // Главное расположение: 2 строки — панель и поле
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Color.DarkSlateGray
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Верхняя панель в ретро-стиле
            topPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(64, 64, 64) // тёмно-серый, как старые окна
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
                    Font = new Font("Courier New", 10, FontStyle.Regular),
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
                    Location = new Point(ClientSize.Width - 180, 15),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                topPanel.Controls.Add(timeLabel);
            }

            tableLayoutPanel1 = new TableLayoutPanel
            {
                RowCount = engine.Rows,
                ColumnCount = engine.Columns,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(0, 0, 64),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            mainLayout.Controls.Add(topPanel, 0, 0);
            mainLayout.Controls.Add(tableLayoutPanel1, 0, 1);
            Controls.Add(mainLayout);
        }

        private void InitializeTimer()
        {
            elapsedTime = TimeSpan.Zero;
            gameTimer = new Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            timeLabel = new Label
            {
                Font = new Font("Courier New", 12, FontStyle.Bold),
                ForeColor = Color.Lime,
                AutoSize = true,
                Anchor = AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 180, 15)
            };

            topPanel.Controls.Add(timeLabel);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            TimeSpan remainingTime = maxTime - elapsedTime;

            if (remainingTime.TotalSeconds <= 0)
            {
                timeLabel.Text = "Время вышло!";
                gameTimer.Stop();
                MessageBox.Show("Время вышло! Попробуй ещё раз.", "Проигрыш");
                backgroundMusic?.Stop();

                Close();
            }
            else
            {
                timeLabel.Text = $"Время: {remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                timeLabel.Font = remainingTime.TotalSeconds <= 10 ? new Font("Courier New", 16, FontStyle.Bold) : new Font("Courier New", 12, FontStyle.Bold);
                timeLabel.ForeColor = remainingTime.TotalSeconds <= 10 ? Color.Red : Color.Lime;
            }
        }

        private void InitGameField()
        {
            for (int i = 0; i < engine.Rows; i++)
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / engine.Rows));

            for (int j = 0; j < engine.Columns; j++)
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / engine.Columns));

            foreach (var iconPath in engine.Icons)
            {
                PictureBox pb = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black,
                    BorderStyle = BorderStyle.Fixed3D,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Tag = iconPath,
                    Image = null,
                    Margin = new Padding(2)
                };

                pb.Click += OnPictureClick;
                tableLayoutPanel1.Controls.Add(pb);
            }
        }

        private void OnPictureClick(object sender, EventArgs e)
        {
            if (revealTimer.Enabled) return;

            var clicked = sender as PictureBox;
            if (clicked == null || clicked.Image != null) return;

            clicked.Image = Image.FromFile(clicked.Tag.ToString());

            if (firstClicked == null)
            {
                firstClicked = clicked;
                return;
            }

            secondClicked = clicked;

            if (firstClicked.Tag.ToString() == secondClicked.Tag.ToString())
            {
                firstClicked = secondClicked = null;

                if (isMultiplayer)
                {
                    if (currentPlayer == 1)
                        scorePlayer1++;
                    else
                        scorePlayer2++;

                    scoreLabel.Text = $"Очки: {scorePlayer1} - {scorePlayer2}";
                }

                CheckWin();
            }
            else
            {
                revealTimer.Start();

                if (isMultiplayer)
                {
                    currentPlayer = currentPlayer == 1 ? 2 : 1;
                    currentPlayerLabel.Text = $"Ход игрока {currentPlayer}";
                }
            }
        }

        private void InitRevealTimer()
        {
            revealTimer = new Timer { Interval = 1000 };
            revealTimer.Tick += (s, e) =>
            {
                revealTimer.Stop();
                firstClicked.Image = null;
                secondClicked.Image = null;
                firstClicked = secondClicked = null;
            };
        }

        private void CheckWin()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is PictureBox pb && pb.Image == null)
                    return;
            }

            gameTimer?.Stop();

            string message = "Вы соединили все карточки!";

            if (isMultiplayer)
            {
                if (scorePlayer1 > scorePlayer2)
                    message = $"Победа игрока 1!\nСчёт: {scorePlayer1} - {scorePlayer2}";
                else if (scorePlayer2 > scorePlayer1)
                    message = $"Победа игрока 2!\nСчёт: {scorePlayer1} - {scorePlayer2}";
                else
                    message = $"Ничья!\nСчёт: {scorePlayer1} - {scorePlayer2}";
            }

            MessageBox.Show(message, "Результат");
            backgroundMusic?.Stop();
            Close();
        }
    }
}
