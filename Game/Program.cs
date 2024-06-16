using IBX_Engine;

namespace IBX_Game
{
    internal class Program
    {
        static void Main()
        {
            Logger.LogInfo("Creating Game Window");
            using GameInstance game = new((1920 / 3) * 2, (1080 / 3) * 2, "Learning OpenTK");
            game.Run();
            Logger.LogWarning("Game Process Died");
        }
    }
}
