using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using IBX_Engine.Graphics.Internal;
using IBX_Engine.Mathematics;

namespace IBX_Engine
{
    public class GameInstance(int width, int height, string title) : GameWindow(new GameWindowSettings(), new NativeWindowSettings()
    {
        ClientSize = new Vector2i(width, height),
        Title = title,
    })
    {

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            // We need to update the aspect ratio once the window has been resized.
            camera.AspectRatio = Size.X / (float)Size.Y;
        }

        /// <summary>
        /// Change the field of view of the camera based on the mouse wheel input.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            camera.FOV -= e.OffsetY;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            camera.ProcessCameraInput(KeyboardState, MouseState, args);
        }

        private const float CELL_THICKNESS = 0.5f;

        private static Vec2[] GetHexVertex(float radius)
        {
            // Define the angle between each vertex of the hexagon
            float angleIncrement = MathF.PI / 3.0f;

            // Initialize array to store vertices
            Vec2[] vertices = new Vec2[7];

            vertices[0] = new Vec2(0f, 0f);

            // Calculate each vertex
            for (int i = 0; i < 6; i++)
            {
                float angle = i * angleIncrement;
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                vertices[i + 1] = new(x, y);
            }

            return vertices;
        }

        private static readonly Vec2[] hexPoints = GetHexVertex(1.0f);

        private readonly float[] vertices =
        [
            // Positions                                        Normals             Texture coords
            //====================BOTTOM HEXAGON FACES====================//

            //Triangle 1
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,    0f, -1.0f, 0f,      0f, 1f,
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, -1.0f, 0f,      1f, 1f,

            //Triangle 2
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, -1.0f, 0f,      1f, 1f,
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, -1.0f, 0f,      1f, 0f,
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //Triangle 3
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, -1.0f, 0f,      0f, 1f,
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, -1.0f, 0f,      1f, 1f,

            //Triangle 4
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, -1.0f, 0f,      1f, 1f,
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, -1.0f, 0f,      1f, 0f,
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //Triagnle 5
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, -1.0f, 0f,      0f, 0f,
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, -1.0f, 0f,      0f, 1f,
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, -1.0f, 0f,      1f, 1f,

            //Triangle 6
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, -1.0f, 0f,      1f, 1f,
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,    0f, -1.0f, 0f,      1f, 0f,
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //====================TOP HEXAGON FACES====================//

            //Triangle 1
            hexPoints[2].X,  CELL_THICKNESS, hexPoints[2].Y,    0f,  1.0f, 0f,      0f, 0f,
            hexPoints[1].X,  CELL_THICKNESS, hexPoints[1].Y,    0f,  1.0f, 0f,      0f, 1f,
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 2
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            hexPoints[3].X,  CELL_THICKNESS, hexPoints[3].Y,    0f,  1.0f, 0f,      1f, 0f,
            hexPoints[2].X,  CELL_THICKNESS, hexPoints[2].Y,    0f,  1.0f, 0f,      0f, 0f,

            //Triangle 3
            hexPoints[4].X,  CELL_THICKNESS, hexPoints[4].Y,    0f,  1.0f, 0f,      0f, 0f,
            hexPoints[3].X,  CELL_THICKNESS, hexPoints[3].Y,    0f,  1.0f, 0f,      0f, 1f,
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 4
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            hexPoints[5].X,  CELL_THICKNESS, hexPoints[5].Y,    0f,  1.0f, 0f,      1f, 0f,
            hexPoints[4].X,  CELL_THICKNESS, hexPoints[4].Y,    0f,  1.0f, 0f,      0f, 0f,

            //Triangle 5
            hexPoints[6].X,  CELL_THICKNESS, hexPoints[6].Y,    0f,  1.0f, 0f,      0f, 0f,
            hexPoints[5].X,  CELL_THICKNESS, hexPoints[5].Y,    0f,  1.0f, 0f,      0f, 1f,
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 6
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            hexPoints[1].X,  CELL_THICKNESS, hexPoints[1].Y,    0f,  1.0f, 0f,      1f, 0f,
            hexPoints[6].X,  CELL_THICKNESS, hexPoints[6].Y,    0f,  1.0f, 0f,      0f, 0f,

            //====================SIDE QUAD FACES====================//

            //SIDE 1 - Triangle 1
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[1].X,  CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 1 - Triangle 2
            hexPoints[1].X, CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[2].X, CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 2 - Triangle 1
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[2].X,  CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 2 - Triangle 2
            hexPoints[2].X, CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[3].X, CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 3 - Triangle 1
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[3].X,  CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 3 - Triangle 2
            hexPoints[3].X, CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[4].X, CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 4 - Triangle 1
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[4].X,  CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 4 - Triangle 2
            hexPoints[4].X, CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[5].X, CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 5 - Triangle 1
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[5].X,  CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 5 - Triangle 2
            hexPoints[5].X, CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[6].X, CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 6 - Triangle 1
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[6].X,  CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 6 - Triangle 2
            hexPoints[6].X, CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[1].X, CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,   0f, 0f, 0f,         1f, 0f,
        ];

        private readonly uint[] indices = 
        [
            // ====================BOTTOM HEXAGON FACES====================//
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,

            // ====================TOP HEXAGON FACES====================//
            19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35,

            // Side 0
            36, 37, 38, 39, 40, 41,

            // Side 1
            42, 43, 44, 45, 46, 47,

            // Side 2
            48, 49, 50, 51, 52, 53,

            // Side 3
            54, 55, 56, 57, 58, 59,

            // Side 4
            60, 61, 62, 63, 64, 65,

            // Side 5
            66, 67, 68, 69, 70, 71,
        ];

        private VertexBufferObject vbo;
        private VertexArrayObject vao;
        private ElementBufferObject ebo;

        private ShaderProgram shader;

        private Texture mainTexture;
        private Texture secondaryTexture;

        private Camera camera;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(0.3f, 0f, 0.3f, 1.0f);

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
            vao.VertexAttributePointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float));

            int normalLocation = shader.GetAttributeLocation("aNormal");
            vao.EnableVertexAttribute(normalLocation);
            vao.VertexAttributePointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            int texCoordLocation = shader.GetAttributeLocation("aUVCoord");
            vao.EnableVertexAttribute(texCoordLocation);
            vao.VertexAttributePointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            
            ebo.BufferData(indices, BufferUsageHint.StaticDraw);

            // We initialize the camera so that it is 3 units back from where the rectangle is.
            // We also give it the proper aspect ratio.
            camera = new(Vector3.UnitZ * 3, Size.X / (float)Size.Y)
            {
                Position = new Vector3(-1.5f, 0.5f, 2.0f)
            };

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {

            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float Sin = (MathF.Sin((float)DateTime.Now.TimeOfDay.TotalSeconds));
            //float absoluteCos = MathF.Abs(MathF.Cos((float)DateTime.Now.TimeOfDay.TotalSeconds));

            Matrix4 model = Matrix4.CreateRotationZ(Sin);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());


            shader.SetVector3("lightPos", new Vector3(2f, 3f, -1.5f));
            shader.SetVector3("lightColor", new Vector3(1f, 1f, 1f));

            shader.SetVector3("viewPos", camera.Position);


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
