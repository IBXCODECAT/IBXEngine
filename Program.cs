namespace LearningOpenTK
{
    internal class Program
    {
        static void Main()
        {
            using GameInstance game = new(800, 600, "Learning OpenTK");
            game.Run();
        }
    }
}
