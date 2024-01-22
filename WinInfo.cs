namespace TicTacToe
{
    public class WinInfo
    //Подробности о победе игрока
    {
        public WinType Type { get; set; } //Направление ряда из 3-х символов
        public int Number { get; set; } //Номер заполненной строки/столбца
    }
}
