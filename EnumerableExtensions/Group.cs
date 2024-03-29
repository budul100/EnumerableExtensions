﻿using HashExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        public static IEnumerable<IEnumerable<T>> ChunkAfter<T>(this IEnumerable<T> items,
            Func<T, bool> predicate)
        {
            var result = new List<T>();

            if (predicate == default)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var item in items.IfAny())
            {
                result.Add(item);

                if (result.Count > 0
                    && predicate.Invoke(item))
                {
                    yield return result;
                    result = new List<T>();
                }
            }

            if (result.Count > 0)
            {
                yield return result;
            }
        }

        public static IEnumerable<IEnumerable<T>> ChunkBefore<T>(this IEnumerable<T> items,
            Func<T, bool> predicate)
        {
            var result = new List<T>();

            if (predicate == default)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var item in items.IfAny())
            {
                if (result.Count > 0
                    && predicate.Invoke(item))
                {
                    yield return result;
                    result = new List<T>();
                }

                result.Add(item);
            }

            if (result.Count > 0)
            {
                yield return result;
            }
        }

        public static IEnumerable<IEnumerable<T>> Framed<T>(this IEnumerable<T> items,
            Func<T, bool> predicate)
        {
            var result = items.SplitAt(predicate)
                .Where(g => predicate.Invoke(g.First())
                    && predicate.Invoke(g.Last()));

            return result;
        }

        public static IEnumerable<IEnumerable<T>> GroupByHash<T, TProperty>(this IEnumerable<T> items,
            params Func<T, TProperty>[] properties)
        {
            if (properties == default)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            var result = items?
                .GroupBy(s => properties.GetSequenceHash(p => p(s))).ToArray();

            return result ?? Enumerable.Empty<IEnumerable<T>>();
        }

        public static IEnumerable<IEnumerable<T>> SplitAt<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (predicate == default)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var result = new List<T>();
            var anyFound = false;

            foreach (var item in items.IfAny())
            {
                result.Add(item);

                if (result.Count > 1
                    && predicate.Invoke(item))
                {
                    yield return result;

                    result = new List<T>
                    {
                        item
                    };

                    anyFound = true;
                }
            }

            if (result.Count > 1
                || (!anyFound && result.AnyItem()))
            {
                yield return result;
            }
        }

        public static IEnumerable<IEnumerable<T>> SplitAtChange<T, TProperty>(this IEnumerable<T> items,
            Func<T, TProperty> property)
        {
            if (property == default)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var result = new List<T>();
            var last = default(TProperty);

            foreach (var item in items.IfAny())
            {
                var current = property != default
                    ? property(item)
                    : default;

                if (result.Count > 0
                    && !current.IsEqual(last))
                {
                    yield return result;
                    result = new List<T>();
                }

                result.Add(item);
                last = current;
            }

            if (result.Count > 0)
            {
                yield return result;
            }
        }

        #endregion Public Methods
    }
}