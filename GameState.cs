using System;

namespace TicTacToe
{
    public class GameState
    //Коренной код игры
    {
        public Player[,] GameGrid { get; private set; } //Игровое поле 3×3
        public Player CurrentPlayer { get; private set; } //Чей ход
        public int TurnsPassed { get; private set; } //Число сделанных ходов
        public bool GameOver { get; private set; } //Регистрация конца игры

        public event Action<int, int> MoveMade; //Момент завершения хода игрока
        public event Action<GameResult> GameEnded; //Момент завершения игры
        public event Action GameRestarted; //Момент начала новой игры

        public GameState() //Игровое поле и первый ход в начале игры
        {
            GameGrid = new Player[3, 3];
            CurrentPlayer = Player.X;
            TurnsPassed = 0;
            GameOver = false;
        }
        private bool CanMakeMove(int r, int c) //Метод разрешённого хода
        {
            return !GameOver && GameGrid[r, c] == Player.None;
        }
        private bool IsGridFull() //Метод заполненного игрового поля
        {
            return TurnsPassed == 9;
        }

        private void SwitchPlayer() //Метод смены игрока после завершения хода
        {
            if (CurrentPlayer == Player.X)
            {
                CurrentPlayer = Player.O;
            }
            else
            {
                CurrentPlayer = Player.X;
            }
        }

        private bool AreSquaresMarked((int, int)[] squares, Player player)
        //Проверить, все ли поля отмечены этим игроком в нужном диапазоне
        {
            foreach ((int r, int c) in squares)
            {
                if (GameGrid[r, c] != player)
                {
                    return false;
                }
            }
            return true;
        }

        private bool DidMoveWin(int r, int c, out WinInfo winInfo) //Анализ выигрышного хода
        {
            (int, int)[] row = new[] { (r, 0), (r, 1), (r, 2) };
            (int, int)[] col = new[] { (0, c), (1, c), (2, c) };
            (int, int)[] mainDiag = new[] { (0, 0), (1, 1), (2, 2) };
            (int, int)[] antiDiag = new[] { (0, 2), (1, 1), (2, 0) };

            if (AreSquaresMarked(row, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.Row, Number = r };
                return true;
            }
            if (AreSquaresMarked(col, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.Column, Number = c };
                return true;
            }
            if (AreSquaresMarked(mainDiag, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.MainDiagonal };
                return true;
            }
            if (AreSquaresMarked(antiDiag, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.AntiDiagonal };
                return true;
            }

            winInfo = null;
            return false;
        }
        private bool DidMoveEndGame(int r, int c, out GameResult gameResult)
        //Проверка окончания игры
        {
            if (DidMoveWin(r, c, out WinInfo winInfo))
            {
                gameResult = new GameResult { Winner = CurrentPlayer, WinInfo = winInfo };
                return true;
            }
            if (IsGridFull())
            {
                gameResult = new GameResult { Winner = Player.None };
                return true;
            }
            gameResult = null;
            return false;
        }
        public void MakeMove(int r, int c) //Основной метод осуществления хода
        {
            if (!CanMakeMove(r, c))
            {
                return;
            }

            GameGrid[r, c] = CurrentPlayer;
            TurnsPassed++;

            if (DidMoveEndGame(r, c, out GameResult gameResult))
            {
                GameOver = true;
                MoveMade?.Invoke(r, c);
                GameEnded?.Invoke(gameResult);
            }
            else
            {
                SwitchPlayer();
                MoveMade?.Invoke(r, c);
            }
        }

        public void Reset() //Сброс GameState
        {
            GameGrid = new Player[3, 3];
            CurrentPlayer = Player.X;
            TurnsPassed = 0;
            GameOver = false;
            GameRestarted?.Invoke();
        }
    }
}