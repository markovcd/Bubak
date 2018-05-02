using System;

namespace Bubak.Shared.Misc
{
    public struct Range : IEquatable<Range>
    {
        public int From { get; }
        public int To { get; }

        public Range(int from, int to)
        {
            From = from;
            To = to;
        }

        public Range(int singleValue) : this(singleValue, singleValue)
        {
        }    

        public static bool operator ==(Range left, Range right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Range left, Range right)
        {
            return !(left == right);
        }

        public bool Equals(Range other)
        {
            return From == other.From && To == other.To;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Range)) return false;
            return Equals((Range)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = -1781160927;
                hashCode = hashCode * -1521134295 + From.GetHashCode();
                hashCode = hashCode * -1521134295 + To.GetHashCode();
                return hashCode;
            }
        }
    }
}
