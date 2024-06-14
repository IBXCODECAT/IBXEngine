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
            // Front face
            -1.0f, -1.0f,  1.0f, 0.0f, 0.0f,
             1.0f, -1.0f,  1.0f, 1.0f, 0.0f,
             1.0f,  1.0f,  1.0f, 1.0f, 1.0f,
            -1.0f,  1.0f,  1.0f, 0.0f, 1.0f,
            // Back face
            -1.0f, -1.0f, -1.0f, 0.0f, 0.0f,
             1.0f, -1.0f, -1.0f, 1.0f, 0.0f,
             1.0f,  1.0f, -1.0f, 1.0f, 1.0f,
            -1.0f,  1.0f, -1.0f, 0.0f, 1.0f,
            // Left face
            -1.0f, -1.0f, -1.0f, 0.0f, 0.0f,
            -1.0f,  1.0f, -1.0f, 1.0f, 0.0f,
            -1.0f,  1.0f,  1.0f, 1.0f, 1.0f,
            -1.0f, -1.0f,  1.0f, 0.0f, 1.0f,
            // Right face
             1.0f, -1.0f, -1.0f, 0.0f, 0.0f,
             1.0f,  1.0f, -1.0f, 1.0f, 0.0f,
             1.0f,  1.0f,  1.0f, 1.0f, 1.0f,
             1.0f, -1.0f,  1.0f, 0.0f, 1.0f,
            // Top face
            -1.0f,  1.0f, -1.0f, 0.0f, 0.0f,
             1.0f,  1.0f, -1.0f, 1.0f, 0.0f,
             1.0f,  1.0f,  1.0f, 1.0f, 1.0f,
            -1.0f,  1.0f,  1.0f, 0.0f, 1.0f,
            // Bottom face
            -1.0f, -1.0f, -1.0f, 0.0f, 0.0f,
             1.0f, -1.0f, -1.0f, 1.0f, 0.0f,
             1.0f, -1.0f,  1.0f, 1.0f, 1.0f,
            -1.0f, -1.0f,  1.0f, 0.0f, 1.0f,
        ];

        private readonly uint[] indices = 
        [
            // Front face
            0, 1, 2,
            2, 3, 0,
            // Back face
            4, 5, 6,
            6, 7, 4,
            // Left face
            8, 9, 10,
            10, 11, 8,
            // Right face
            12, 13, 14,
            14, 15, 12,
            // Top face
            16, 17, 18,
            18, 19, 16,
            // Bottom face
            20, 21, 22,
            22, 23, 20,
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

            GL.Enable(EnableCap.DepthTest);

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

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));

            // Note that we're translating the scene in the reverse direction of where we want to move.
            Matrix4 view = Matrix4.CreateTranslation(0.0f, MathF.Sin((float)DateTime.Now.TimeOfDay.TotalSeconds), -3.0f);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), width / height, 0.1f, 100.0f);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);


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
