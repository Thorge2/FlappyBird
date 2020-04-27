using FlappyBird.Engine;
using OpenTK;
using System.ComponentModel.Design;

namespace FlappyBird.Game
{
    class Player
    {
        private Renderer _renderer;
        private Rectangle _rectangle;

        private float _velocity;
        private float _position;
        private float _angle;
        private float _height = 0.3f;
        private float _width = 0.2f;

        public int Group { get; }
        public int Score { get; private set; }
        public bool Alive { get; private set; }

        public Player(Renderer renderer)
        {
            _renderer = renderer;
            _rectangle = new Rectangle(0f, 0f, _width, _height, 1f);
            Group = _renderer.CreateRenderGroup();
            _renderer.AddRectangleToGroup(Group, _rectangle);

            Alive = true;
        }

        public void MovePlayer(float eTime)
        {
            if (_velocity < 3f)
                 _velocity -= 0.07f * eTime;

            _position += _velocity;

            if (_velocity < 0 && _angle > -5)
                _angle -= 1;
            if (_velocity > 0.02f && _angle < 9)
                _angle += 1;

            Matrix4 transform = Matrix4.Identity;
            transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_angle));
            transform *= Matrix4.CreateTranslation(0f, _position, 0f);

            _renderer.SetTransformRenderGroup(Group, transform);
        }

        public void DetectCollision(Pipes pipes)
        {
            //top/bottom colision
            if (this._position > 1f - _height/3 || this._position < -1f + _height/3)
                Alive = false;

            foreach (PipePair pair in pipes.PipePairs)
            {
                //top pipe collision
                if (_position > pair.HorizontalOffset + 0.4f - _height / 3 && pair.MovePosition < -1f + _width / 3 && pair.MovePosition > -1f + _width / 3 - 0.25f)
                {
                    Alive = false;
                    break;
                }

                //bottom pipe collision
                if (_position < pair.HorizontalOffset - 0.4f + _height / 3 && pair.MovePosition < -1f + _width / 3 && pair.MovePosition > -1f + _width / 3 - 0.25f)
                {
                    Alive = false;
                    break;
                }

                if (pair.MovePosition < -1f && pair.MovePosition > -1.005f)
                {
                    Score++;
                }
            }
        }

        public void Jump()
        {
            if (_position == -1f)
                _position = -0.999f;
            _velocity = 0.03f;
            _angle = 6;
        }
    }
}
