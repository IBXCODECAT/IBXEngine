using LearningOpenTK.Buffers;
using LearningOpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearningOpenTK
{
    internal class GameInstance(int width, int height, string title) : GameWindow(new GameWindowSettings(), new NativeWindowSettings()
    {
        ClientSize = new OpenTK.Mathematics.Vector2i(width, height),
        Title = title,
    })
    {

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if(KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        private readonly float[] vertices = [
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        ];

        private readonly uint[] indices = [
            0, 1, 3,
            1, 2, 3
        ];

        private VertexBufferObject vbo;
        private VertexArrayObject vao;
        private ElementBufferObject ebo;

        private ShaderProgram shader;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            vao = new();
            vbo = new();
            ebo = new();

            shader = new("Assets/shader.vert", "Assets/shader.frag");

            shader.Use();

            vbo.SetData(vertices, BufferUsageHint.StaticDraw);

            vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float));
            vao.EnableVertexAttribute(0);

            ebo.BufferData(indices, BufferUsageHint.StaticDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            ebo.Dispose();
            vao.Dispose();
            vbo.Dispose();
            shader.Dispose();
        }
    }
}
