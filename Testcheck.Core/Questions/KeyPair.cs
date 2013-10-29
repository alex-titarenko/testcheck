using System;
using System.Collections.Generic;
using System.Text;

namespace TAlex.Testcheck.Core.Questions
{
    [Serializable]
    public struct KeyPair : IComparable, IComparable<KeyPair>
    {
        #region Fields

        private int _key1;

        private int _key2;

        #endregion

        #region Properties

        public int Key1
        {
            get
            {
                return _key1;
            }
        }

        public int Key2
        {
            get
            {
                return _key2;
            }
        }

        #endregion

        #region Constructors

        public KeyPair(int key1, int key2)
        {
            _key1 = key1;
            _key2 = key2;
        }

        #endregion

        #region Methods

        public int CompareTo(KeyPair other)
        {
            if (_key1 != other._key1)
                return _key1.CompareTo(other._key1);
            else
                return _key2.CompareTo(other._key2);
        }

        public int CompareTo(object obj)
        {
            if (obj is KeyPair)
                return CompareTo((KeyPair)obj);
            else
                throw new ArgumentException();
        }

        public override int GetHashCode()
        {
            return _key1.GetHashCode() ^ _key2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            KeyPair pair = (KeyPair)obj;
            return (_key1 == pair._key1) && (_key2 == pair._key2);
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}", _key1, _key2);
        }

        #endregion
    }
}
