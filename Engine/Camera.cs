﻿using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace IBX_Engine
{
    // This is the camera class as it could be set up after the tutorials on the website.
    // It is important to note there are a few ways you could have set up this camera.
    // For example, you could have also managed the player input inside the camera class,
    // and a lot of the properties could have been made into functions.

    // TL;DR: This is just one of many ways in which we could have set up the camera.
    // Check out the web version if you don't know why we are doing a specific thing or want to know more about the code.
    internal class Camera(Vector3 position, float aspectRatio)
    {
        internal bool FirstMove { get; set; } = true;
        internal Vector2 LastPosition { get; set; } = Vector2.Zero;

        // Those vectors are directions pointing outwards from the camera to define how it rotated.
        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;

        // Rotation around the X axis (radians)
        private float _pitch;

        // Rotation around the Y axis (radians)
        private float _yaw = -MathHelper.PiOver2; // Without this, you would be started rotated 90 degrees right.

        // The field of view of the camera (radians)
        private float _fov = MathHelper.PiOver2;

        // The position of the camera
        public Vector3 Position { get; set; } = position;

        // This is simply the aspect ratio of the viewport, used for the projection matrix.
        public float AspectRatio { private get; set; } = aspectRatio;

        public Vector3 Front => _front;

        public Vector3 Up => _up;

        public Vector3 Right => _right;

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        // The field of view (FOV) is the vertical angle of the camera view.
        // This has been discussed more in depth in a previous tutorial,
        // but in this tutorial, you have also learned how we can use this to simulate a zoom feature.
        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float FOV
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                float angle = MathHelper.Clamp(value, 1f, 90f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        // Get the projection matrix using the same method we have used up until this point
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        }

        // This function is going to update the direction vertices using some of the math learned in the web tutorials.
        private void UpdateVectors()
        {
            // First, the front matrix is calculated using some basic trigonometry.
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
            _front = Vector3.Normalize(_front);

            // Calculate both the right and the up vector using cross product.
            // Note that we are calculating the right from the global up; this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        internal void ProcessCameraInput(KeyboardState keyboard, MouseState mouse, FrameEventArgs args)
        {
            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (keyboard.IsKeyDown(Keys.W))
            {
                Position += Front * cameraSpeed * (float)args.Time; // Forward
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                Position -= Front * cameraSpeed * (float)args.Time; // Backwards
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                Position -= Right * cameraSpeed * (float)args.Time; // Left
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                Position += Right * cameraSpeed * (float)args.Time; // Right
            }
            if (keyboard.IsKeyDown(Keys.Space))
            {
                Position += Up * cameraSpeed * (float)args.Time; // Up
            }
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                Position -= Up * cameraSpeed * (float)args.Time; // Down
            }

            if (FirstMove) // This bool variable is initially set to true.
            {
                LastPosition = new Vector2(mouse.X, mouse.Y);
                FirstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - LastPosition.X;
                var deltaY = mouse.Y - LastPosition.Y;
                LastPosition = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                Yaw += deltaX * sensitivity;
                Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }
    }
}