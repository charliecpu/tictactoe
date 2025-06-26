using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TicTacToeControl : UserControl
    {
        enum TurnEnum { X, O };
        TurnEnum currentturn = TurnEnum.X;

        enum GameStatusEnum { NotStarted, Playing, Tie, Winner }
        GameStatusEnum gamestatus = GameStatusEnum.NotStarted;

        List<Button> lstbuttons;
        List<List<Button>> lstwinningsets;
        List<Button> lstrankedbuttons;

        int xWins = 0;
        int oWins = 0;
        int tieGames = 0;

        Color defaultbackcolor;
        bool playcomputer = false;

        public TicTacToeControl()
        {
            InitializeComponent();
            lblScoreboard.Text = GetScoreboardText();
            defaultbackcolor = btn1.BackColor;
            lstbuttons = new() { btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };

            lstbuttons.ForEach(b => b.Click += SpotButton_Click);
            btnStart.Click += BtnStart_Click;
            /*
1,2,3
4,5,6
7,8,9
*/
            lstwinningsets = new() {
                new() { btn1, btn2, btn3},
                new() {btn4, btn5, btn6},
                new() {btn7, btn8, btn9},
                new() {btn1, btn4, btn7},
                new() {btn2, btn5, btn8},
                new() {btn3, btn6, btn9},
                new() {btn1, btn5, btn9},
                new() {btn3, btn5, btn7}
            };

            lstrankedbuttons = new() { btn5, btn1, btn3, btn7, btn9 };

            lstbuttons.ForEach(b => b.Enabled = false);

            DisplayGameStatus();
        }

        private void StartGame()
        {
            lstbuttons.ForEach(b => { b.Text = ""; b.Enabled = true; });
            gamestatus = GameStatusEnum.Playing;
            currentturn = TurnEnum.X;
            playcomputer = optPlayComputer.Checked;
            DisplayGameStatus();
            SetButtonsBackcolor(lstbuttons);
            UpdateScoreboard();
        }


        private void DoTurn(Button btn)
        {
            if (btn.Text == "" && gamestatus == GameStatusEnum.Playing)
            {
                btn.Text = currentturn.ToString();

                lstwinningsets.ForEach(l => DetectWinner(l));

                if (gamestatus == GameStatusEnum.Playing)
                {
                    DetectTie();
                    //switch turn
                    if (gamestatus == GameStatusEnum.Playing)
                    {
                        if (currentturn == TurnEnum.X)
                        {
                            currentturn = TurnEnum.O;
                        }
                        else
                        {
                            currentturn = TurnEnum.X;
                        }
                        if (IsComputerTurn())
                        {
                            //1st step of algorithm - offense
                            DoComputerTurnOffenseDefense(TurnEnum.O);
                            if (IsComputerTurn())
                            {
                                //if turn not taken computer then defense
                                DoComputerTurnOffenseDefense(TurnEnum.X);
                                if (IsComputerTurn())
                                {
                                    //if turn not taken computer then by rank
                                    DoComputerTurnRank();

                                    if (IsComputerTurn())
                                    {
                                        //if turn not taken computer then random available button
                                        DoComputerTurnRandom();
                                    }
                                }
                            }
                        }

                    }
                }

                DisplayGameStatus();
            }
        }

        private bool IsComputerTurn()
        {
            return currentturn == TurnEnum.O && gamestatus == GameStatusEnum.Playing && playcomputer == true;
        }

        private void DoComputerTurnOffenseDefense(TurnEnum turn)
        {
            //find first set that has two buttongs with computer letter and one blank
            //get the blank button out of the set
            var lst = lstwinningsets.FirstOrDefault(l =>
                l.Count(b => b.Text == turn.ToString()) == 2
                &&
                l.Count(b => b.Text == "") == 1
                );

            if (lst != null)
            {
                var btn = lst.First(b => b.Text == "");
                DoTurn(btn);
            }

        }

        private void DoComputerTurnRank()
        {
            var btn = lstrankedbuttons.FirstOrDefault(b => b.Text == "");
            if (btn != null)
            {
                DoTurn(btn);
            }
        }

        private void DoComputerTurnRandom()
        {
            var lst = lstbuttons.Where(b => b.Text == "").ToList();
            var btn = lst[new Random().Next(0, lst.Count())];
            DoTurn(btn);
        }

        private void DetectWinner(List<Button> lst)
        {
            if (gamestatus == GameStatusEnum.Playing && lst.Count(b => b.Text == currentturn.ToString()) == lst.Count())
            {
                gamestatus = GameStatusEnum.Winner;
                SetButtonsBackcolor(lst);
                if (currentturn == TurnEnum.X)
                {
                    xWins++;
                }
                else
                {
                    oWins++;
                }
                UpdateScoreboard();
            }
        }


        private void DetectTie()
        {
            if (gamestatus == GameStatusEnum.Playing && lstbuttons.Count(b => b.Text == "") == 0)
            {
                gamestatus = GameStatusEnum.Tie;
                SetButtonsBackcolor(lstbuttons);
                tieGames++;
                UpdateScoreboard();
            }
        }

        private void SetButtonsBackcolor(List<Button> lst)
        {
            Color c = defaultbackcolor;

            switch (gamestatus)
            {
                case GameStatusEnum.Tie:
                    c = Color.White;
                    break;
                case GameStatusEnum.Winner:
                    c = Color.Yellow;
                    break;
            }
            lst.ForEach(b => b.BackColor = c);
        }

        private string GetScoreboardText()
        {
            return $"X Wins: {xWins}  O Wins: {oWins}  Ties: {tieGames}";
        }

        private void UpdateScoreboard()
        {
            lblScoreboard.Text = GetScoreboardText();
        }
        private void DisplayGameStatus()
        {
            string msg = "Click Start to begin Game";

            switch (gamestatus)
            {
                case GameStatusEnum.Playing:
                    msg = "Current Turn: " + currentturn.ToString();
                    break;
                case GameStatusEnum.Tie:
                    msg = "Tie";
                    break;
                case GameStatusEnum.Winner:
                    msg = "Winner is: " + currentturn.ToString();
                    break;
            }

            msg = (playcomputer ? optPlayComputer.Text : optTwoPlayer.Text) + " - " + msg;

            lblStatus.Text = msg;
        }
        private void SpotButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button)
            {
                DoTurn((Button)sender);
            }

        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            StartGame();
        }
    }
}
