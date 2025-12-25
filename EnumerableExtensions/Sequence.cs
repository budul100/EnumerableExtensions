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
        /// </summary>
        /// <returns>
        /// A Dictionary: Key = Index in Master Array, Value = Your Input Object.
        /// Returns NULL if the sequence was not found.
        /// </returns>
        public static Dictionary<int, TSearchItem> MapSequence<TMasterItem, TSearchItem>(this IEnumerable<TMasterItem> master,
            IEnumerable<TSearchItem> sequence, Func<TSearchItem, TMasterItem> selector)
        {
            if (master is null)
            {
                throw new ArgumentNullException(nameof(master));
            }

            if (sequence is null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var masterArray = master.ToArray();
            var sequenceArray = sequence.ToArray();

            var masterLength = masterArray.Length;
            var sequenceLength = sequenceArray.Length;

            if (sequenceLength == 0 || sequenceLength > masterLength) return null;

            // ---------------------------------------------------------
            // STEP A: Pre-Mapping (Optimization)
            // We extract the comparison values (IDs) from the complex objects once.
            // This avoids calling the selector function repeatedly in the search loop.
            // ---------------------------------------------------------
            var mappedSearch = new TMasterItem[sequenceLength];
            for (var mapIndex = 0; mapIndex < sequenceLength; mapIndex++)
            {
                mappedSearch[mapIndex] = selector(sequenceArray[mapIndex]);
            }

            // ---------------------------------------------------------
            // STEP B: KMP Pre-Processing (Building LPS Table)
            // This analyzes the input for internal loops (e.g., A->B->A)
            // to ensure the search doesn't fail on repetitive patterns.
            // ---------------------------------------------------------
            var lps = new int[sequenceLength];
            var len = 0;
            var sequenceIndex = 1;

            while (sequenceIndex < sequenceLength)
            {
                if (mappedSearch[sequenceIndex].Equals(mappedSearch[len]))
                {
                    len++;
                    lps[sequenceIndex] = len;
                    sequenceIndex++;
                }
                else
                {
                    if (len != 0) len = lps[len - 1];
                    else { lps[sequenceIndex] = 0; sequenceIndex++; }
                }
            }

            // ---------------------------------------------------------
            // STEP C: The Search (Linear KMP Search)
            // ---------------------------------------------------------
            var masterIndex = 0;
            var searchIndex = 0;
            var foundStartIndex = -1;

            while (masterIndex < masterLength)
            {
                if (mappedSearch[searchIndex].Equals(masterArray.ElementAt(masterIndex)))
                {
                    masterIndex++;
                    searchIndex++;

                    if (searchIndex == sequenceLength)
                    {
                        // FOUND!
                        // Calculate the starting position in the master array
                        foundStartIndex = masterIndex - searchIndex;
                        break; // Stop searching
                    }
                }
                else
                {
                    if (searchIndex != 0)
                    {
                        // Intelligent fallback using the LPS table
                        // This handles scenarios like "A-B-A-B" efficiently
                        searchIndex = lps[searchIndex - 1];
                    }
                    else
                    {
                        masterIndex++;
                    }
                }
            }

            // ---------------------------------------------------------
            // STEP D: Dictionary Creation (Integration)
            // ---------------------------------------------------------
            if (foundStartIndex == -1)
            {
                return default; // Sequence not found in Master
            }

            // Create the dictionary with the exact required size
            var result = new Dictionary<int, TSearchItem>(sequenceLength);

            for (int resultIndex = 0; resultIndex < sequenceLength; resultIndex++)
            {
                // The Key is the absolute index in the Master Array
                var absoluteIndex = foundStartIndex + resultIndex;

                // The Value is the original complex object from the Input
                result[absoluteIndex] = sequenceArray[resultIndex];
            }

            return result;
        }

        #endregion Public Methods
    }
}