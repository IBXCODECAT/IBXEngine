using IBXEngine;

namespace IBX_Game
{
    internal class Program
    {
        static void Main()
        {
            using GameInstance game = new((1920 / 3) * 2, (1080 / 3) * 2, "Learning OpenTK");
            game.Run();
        }
    }
}
