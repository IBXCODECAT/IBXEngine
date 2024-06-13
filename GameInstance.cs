using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningOpenTK
{
    internal class GameInstance : GameWindow
    {
        public GameInstance(int width, int height, string title) : base(new GameWindowSettings(), new NativeWindowSettings()
        {
            Size = new OpenTK.Mathematics.Vector2i(width, height),
            Title = title
        }) { }

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if(KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
    }
}
