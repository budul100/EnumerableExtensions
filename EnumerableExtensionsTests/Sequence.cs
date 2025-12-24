using System;
using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using Xunit;

namespace EnumerableExtensionsTests
{
    public class Sequence
    {
        #region Public Methods

        // ---------------------------------------------------------------
        // SCENARIO 4: KMP "Backtracking" Logic
        // Tests if the algorithm correctly handles partial matches (A-B-A-C vs A-B-A-B)
        // ---------------------------------------------------------------
        [Fact]
        public void Should_Handle_Partial_Matches_Correctly()
        {
            // Arrange
            // The algorithm might get tricked by the first "1, 2, 1" if not using KMP/LPS correctly
            int[] masterTrack = [1, 2, 1, 3, 1, 2, 1, 2];

            // We are looking for 1-2-1-2
            var inputSeq = new[]
            {
                new Item { SegmentId = 1 },
                new Item { SegmentId = 2 },
                new Item { SegmentId = 1 },
                new Item { SegmentId = 2 }
            };

            // Act
            var result = masterTrack.MapSequence(
                sequence: inputSeq,
                selector: x => x.SegmentId
                                                );

            // Assert
            Assert.NotNull(result);
            // The sequence 1,2,1,2 starts at Index 4 in the master track
            Assert.True(result.ContainsKey(4));
            Assert.True(result.ContainsKey(7));
        }

        // ---------------------------------------------------------------
        // SCENARIO 1: The "Happy Path" with a Mini-Loop
        // This tests the core requirement: Input "A -> B -> A" mapped to Master indices.
        // ---------------------------------------------------------------
        [Fact]
        public void Should_Map_MiniLoops_To_Correct_MasterIndices()
        {
            // Arrange
            // Master Track: IDs [10, 20, 10, 20, 30]
            // Indices:           0   1   2   3   4
            int[] masterTrack = [10, 20, 10, 20, 30];

            // Input: Driver drove [20, 10, 20]
            // This corresponds to Master Indices [1, 2, 3]
            var inputSeq = new[]
            {
                new Item { SegmentId = 20, Note = "First 20" },  // Input[0]
                new Item { SegmentId = 10, Note = "Middle 10" }, // Input[1]
                new Item { SegmentId = 20, Note = "Second 20" }  // Input[2]
            };

            // Act
            var result = masterTrack.MapSequence(
                sequence: inputSeq,
                selector: x => x.SegmentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            // Verify Index 1 maps to the first input item
            Assert.True(result.ContainsKey(1));
            Assert.Equal("First 20", result[1].Note);

            // Verify Index 2 maps to the middle input item
            Assert.True(result.ContainsKey(2));
            Assert.Equal("Middle 10", result[2].Note);

            // Verify Index 3 maps to the last input item
            // This proves the dictionary handles duplicate IDs correctly by using the MasterIndex as Key
            Assert.True(result.ContainsKey(3));
            Assert.Equal("Second 20", result[3].Note);
        }

        // ---------------------------------------------------------------
        // SCENARIO 5: Empty Input
        // ---------------------------------------------------------------
        [Fact]
        public void Should_Return_Null_On_Empty_Input()
        {
            int[] master = [1, 2, 3];
            var input = Array.Empty<Item>();

            var result = master.MapSequence(
                sequence: input,
                selector: x => x.SegmentId);

            Assert.Null(result);
        }

        // ---------------------------------------------------------------
        // SCENARIO 3: Input larger than Master
        // ---------------------------------------------------------------
        [Fact]
        public void Should_Return_Null_When_Input_Longer_Than_Master()
        {
            // Arrange
            int[] masterTrack = [1, 2];
            var inputSeq = new[]
            {
                new Item { SegmentId = 1 },
                new Item { SegmentId = 2 },
                new Item { SegmentId = 3 }
            };

            // Act
            var result = masterTrack.MapSequence(
                sequence: inputSeq,
                selector: x => x.SegmentId
                                                );

            // Assert
            Assert.Null(result);
        }

        // ---------------------------------------------------------------
        // SCENARIO 2: Sequence not found
        // ---------------------------------------------------------------
        [Fact]
        public void Should_Return_Null_When_Sequence_Not_In_Master()
        {
            // Arrange
            int[] masterTrack = [1, 2, 3, 4, 5];

            // "1, 3" exists separately, but not as a sequence
            var inputSeq = new[]
            {
                new Item { SegmentId = 1 },
                new Item { SegmentId = 3 }
            };

            // Act
            var result = masterTrack.MapSequence(
                sequence: inputSeq,
                selector: x => x.SegmentId
                                                );

            // Assert
            Assert.Null(result);
        }

        #endregion Public Methods
    }
}