using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MineBlox.Engine.camera
{
    /// <summary>
    /// Represents a 3D perspective camera.
    /// Handles position, orientation, and matrix generation.
    /// </summary>
    public class PlayerCamera
    {
        private Vector3 _position;
        private Vector3 _front;
        private readonly Vector3 _up = Vector3.UnitY;

        private float _yaw;
        private float _pitch;

        private readonly float _aspectRatio;
        private readonly float _fov;
        private readonly float _viewDistance;

        private const float MovementSpeed = 5f;
        private const float MouseSensitivity = 0.1f;

        /// <summary>
        /// Initialises the camera at the given position.
        /// </summary>
        /// <param name="position">Starting world position.</param>
        /// <param name="aspectRatio">Viewport width divided by height.</param>
        public PlayerCamera(Vector3 position, float aspectRatio)
        {
            _position = position;
            _aspectRatio = aspectRatio;
            _fov = MathHelper.DegreesToRadians(75f);
            _viewDistance = 800f;
            _yaw = -MathHelper.PiOver2; // Face forward along -Z axis
            _pitch = 0f;

            UpdateFrontVector();
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _position + _front, _up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                _fov,
                _aspectRatio,
                0.1f,
                _viewDistance);
        }

        /// <summary>
        /// Rotates the camera based on mouse delta movement.
        /// </summary>
        public void ProcessMouseMovement(float deltaX, float deltaY)
        {
            _yaw += deltaX * MouseSensitivity;
            _pitch -= deltaY * MouseSensitivity;

            // Clamp pitch so the camera doesn't flip upside down
            _pitch = Math.Clamp(_pitch, -89f, 89f);

            UpdateFrontVector();
        }

        /// <summary>
        /// Moves the camera based on keyboard input.
        /// </summary>
        public void ProcessKeyboard(KeyboardState keyboard, float deltaTime)
        {
            float speed = MovementSpeed * deltaTime;

            if (keyboard.IsKeyDown(Keys.W))
                _position += _front * speed;
            if (keyboard.IsKeyDown(Keys.S))
                _position -= _front * speed;
            if (keyboard.IsKeyDown(Keys.A))
                _position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * speed;
            if (keyboard.IsKeyDown(Keys.D))
                _position += Vector3.Normalize(Vector3.Cross(_front, _up)) * speed;
        }

        // ---------------------------------------------------------------
        // Private Helpers
        // ---------------------------------------------------------------

        private void UpdateFrontVector()
        {
            _front = Vector3.Normalize(new Vector3(
                MathF.Cos(MathHelper.DegreesToRadians(_yaw)) * MathF.Cos(MathHelper.DegreesToRadians(_pitch)),
                MathF.Sin(MathHelper.DegreesToRadians(_pitch)),
                MathF.Sin(MathHelper.DegreesToRadians(_yaw)) * MathF.Cos(MathHelper.DegreesToRadians(_pitch))
            ));
        }
    }
}