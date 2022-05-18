namespace Ponyu.Connector
{
    public class Either<L, R>
    {
        private readonly L? _left;
        private readonly R? _right;

        public Either(L leftValue)
        {
            _left = leftValue;
        }

        public Either(R rightValue)
        {
            _right = rightValue;
        }

        public L Left
        {
            get
            {
                if(_left == null)
                {
                    throw new InvalidOperationException("Left value not set");
                }

                return _left;
            }
        }

        public R Right
        {
            get
            {
                if (_right == null)
                {
                    throw new InvalidOperationException("Right value not set");
                }

                return _right;
            }
        }

        public void Match(Action<L> leftAction, Action<R> rightAction)
        {
            if(_left != null)
            {
                leftAction(_left);
            }
            else if(_right != null)
            {
                rightAction(_right);
            }
            else
            {
                throw new InvalidOperationException("Value not set");
            }
        }
    }
}
