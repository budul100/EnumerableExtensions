using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static partial class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Searches for the sequence within the master track and returns a dictionary
        /// mapping every involved master index to the corresponding input object.
        /// Only the first occurrence is mapped.
        /// </summary>
        /// <returns>
        /// A Dictionary: Key = Index in Master Array, Value = Your Input Object.
        /// Returns NULL if the sequence was not found.
        /// </returns>
        public static Dictionary<int, TSearchItem> MapSequence<TMasterItem, TSearchItem>(this IEnumerable<TMasterItem> master,
            IEnumerable<TSearchItem> sequence, Func<TSearchItem, TMasterItem> selector,
            IEqualityComparer<TMasterItem> comparer = default)
        {
            ArgumentNullException.ThrowIfNull(master);
            ArgumentNullException.ThrowIfNull(sequence);
            ArgumentNullException.ThrowIfNull(selector);

            // Arrays are only read, so they are used as they are.
            var masterArray = master as TMasterItem[] ?? master.ToArray();
            var sequenceArray = sequence as TSearchItem[] ?? sequence.ToArray();

            var masterLength = masterArray.Length;
            var sequenceLength = sequenceArray.Length;

            if (sequenceLength == 0
                || sequenceLength > masterLength)
            {
                return default;
            }

            // Null-safe and free of boxing for value types.
            comparer ??= EqualityComparer<TMasterItem>.Default;

            // Map the search items once, so the selector is not called inside the search loop.
            var mappedSearch = new TMasterItem[sequenceLength];

            for (var mapIndex = 0; mapIndex < sequenceLength; mapIndex++)
            {
                mappedSearch[mapIndex] = selector(sequenceArray[mapIndex]);
            }

            // KMP pre-processing: longest proper prefix which is also a suffix for every position.
            // Required for patterns with internal repetitions (A-B-A-B).
            var lps = new int[sequenceLength];
            var length = 0;
            var sequenceIndex = 1;

            while (sequenceIndex < sequenceLength)
            {
                if (comparer.Equals(mappedSearch[sequenceIndex], mappedSearch[length]))
                {
                    length++;
                    lps[sequenceIndex] = length;
                    sequenceIndex++;
                }
                else if (length != 0)
                {
                    length = lps[length - 1];
                }
                else
                {
                    lps[sequenceIndex] = 0;
                    sequenceIndex++;
                }
            }

            // Linear KMP search.
            var masterIndex = 0;
            var searchIndex = 0;
            var foundStartIndex = -1;

            while (masterIndex < masterLength)
            {
                if (comparer.Equals(mappedSearch[searchIndex], masterArray[masterIndex]))
                {
                    masterIndex++;
                    searchIndex++;

                    if (searchIndex == sequenceLength)
                    {
                        foundStartIndex = masterIndex - searchIndex;
                        break;
                    }
                }
                else if (searchIndex != 0)
                {
                    searchIndex = lps[searchIndex - 1];
                }
                else
                {
                    masterIndex++;
                }
            }

            if (foundStartIndex == -1)
            {
                return default;
            }

            var result = new Dictionary<int, TSearchItem>(sequenceLength);

            for (var resultIndex = 0; resultIndex < sequenceLength; resultIndex++)
            {
                result[foundStartIndex + resultIndex] = sequenceArray[resultIndex];
            }

            return result;
        }

        #endregion Public Methods
    }
}
