﻿using System.Collections.Generic;

namespace Nito.Comparers.Util
{
    /// <summary>A comparer that uses another comparer if the source comparer determines the objects are equal.</summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class CompoundComparer<T> : SourceComparerBase<T, T>
    {
        /// <summary>The second comparer.</summary>
        private readonly IComparer<T> _secondSource;

        /// <summary>Initializes a new instance of the <see cref="CompoundComparer&lt;T&gt;"/> class.</summary>
        /// <param name="source">The source comparer. If this is <c>null</c>, the default comparer is used.</param>
        /// <param name="secondSource">The second comparer. If this is <c>null</c>, the default comparer is used.</param>
        public CompoundComparer(IComparer<T> source, IComparer<T> secondSource)
            : base(source, true)
        {
            _secondSource = ComparerHelpers.NormalizeDefault(secondSource);
        }

        /// <summary>Returns a hash code for the specified object.</summary>
        /// <param name="obj">The object for which to return a hash code. This object may be <c>null</c>.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected override int DoGetHashCode(T obj)
        {
            unchecked
            {
                var ret = (int)2166136261;
                ret += ComparerHelpers.GetHashCodeFromComparer(_source, obj);
                ret *= 16777619;
                ret += ComparerHelpers.GetHashCodeFromComparer(_secondSource, obj);
                ret *= 16777619;
                return ret;
            }
        }

        /// <summary>Compares two objects and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</summary>
        /// <param name="x">The first object to compare. This object may be <c>null</c>.</param>
        /// <param name="y">The second object to compare. This object may be <c>null</c>.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
        protected override int DoCompare(T x, T y)
        {
            var ret = _source.Compare(x, y);
            if (ret != 0)
                return ret;
            return _secondSource.Compare(x, y);
        }

        /// <summary>Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.</summary>
        public override string ToString()
        {
            return "Compound(" + _source + ", " + _secondSource + ")";
        }
    }
}
