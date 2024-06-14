using LearningOpenTK.Buffers;
using LearningOpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using OpenTK.Mathematics;

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

        private readonly float[] vertices =
        [
            //Position          Texture coordinates
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        ];

        private readonly uint[] indices = [
            0, 1, 3,
            1, 2, 3
        ];

        private VertexBufferObject vbo;
        private VertexArrayObject vao;
        private ElementBufferObject ebo;

        private ShaderProgram shader;

        private Texture mainTexture;
        private Texture secondaryTexture;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            vao = new();
            vbo = new();
            ebo = new();

            mainTexture = new("Assets/trees.png");
            secondaryTexture = new("Assets/pfp.png");

            shader = new("Assets/shader.vert", "Assets/shader.frag");

            shader.Use();

            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);

            mainTexture.Use(TextureUnit.Texture0);
            secondaryTexture.Use(TextureUnit.Texture1);

            vbo.SetData(vertices, BufferUsageHint.StaticDraw);

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            int vertexLocation = shader.GetAttributeLocation("aPosition");
            vao.EnableVertexAttribute(vertexLocation);
            vao.VertexAttributePointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float));

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            int texCoordLocation = shader.GetAttributeLocation("aUVCoord");
            vao.EnableVertexAttribute(texCoordLocation);
            vao.VertexAttributePointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            
            ebo.BufferData(indices, BufferUsageHint.StaticDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
            Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
            Matrix4 translation = rotation * scale;

            GL.UniformMatrix4(shader.GetUniformLocation("transform"), true, ref translation);

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

            mainTexture.Dispose();
        }
    }
}
