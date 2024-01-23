namespace TicTacToe
{
    public class GameResult
    //Результаты игры
    {
        public Player Winner { get; set; } //Определение победителя/ничьи
        public WinInfo WinInfo { get; set; } //Подробности о победе игрока
    }
}