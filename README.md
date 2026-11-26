# EnumerableExtensions

A collection of extension methods for working with enumerable data in .NET, providing utilities for counting, merging, converting, grouping and manipulating collections.

## Installation

Install via NuGet:

```dotnet add package budul.EnumerableExtensions```

Or visit: https://www.nuget.org/packages/budul.EnumerableExtensions

## Features

### Counting & Aggregation
- `CountOrDefault<T>()` - Returns count or 0 if null/empty
- `MaxOrDefault<T>()` / `MaxOrNull<T>()` - Safe max operations
- `MinOrDefault<T>()` / `MinOrNull<T>()` - Safe min operations

### Merging
- `Merge()` - Combines non-null items into a delimited string with optional sorting and distinct filtering
- `Merge<T, TProp>()` - Merges items by selecting a specific property

### Conversion
- `ToArrayOrDefault()` - Converts to array or returns null if empty
- `ToListOrDefault()` - Converts to list or returns null if empty
- `AsSingleEnumerable()` / `AsArray()` / `AsList()` - Wraps a single value

### Grouping & Chaining
- `ChunkAfter()`, `ChunkBefore()`, `SplitAt()`, `SplitAtChange()`, `Framed()`
- `GroupByHash()` - Groups by the values of several properties
- `ToPairs()`, `ToTriples()`, `ToConsecutivePairs()`, `ToConsecutiveTriples()`, `DistinctSuccessive()`

### Sequences
- `MapSequence()` - Finds a sequence inside a master track (KMP) and maps the master indexes to the input items

## Usage Example

```csharp
using EnumerableExtensions;
var items = new[] { "c", "b", "a", "c" };

// Merge with sorting and distinct
string result = items.Merge(); // "a,b,c"

// Merge without sorting (still distinct)
string unsorted = items.Merge(preventSort: true); // "c,b,a"

// Merge without sorting and without distinct
string raw = items.Merge(preventSort: true, preventDistinct: true); // "c,b,a,c"

// Safe counting
int count = items.CountOrDefault(); // 4
```

## Target Frameworks

- .NET 10

## Breaking changes in 2.0

- Target framework is .NET 10 only (previously .NET Standard 2.0 / 2.1, .NET 7 / 8).
- `AsEnumerable<T>(this T)` is now `AsSingleEnumerable<T>(this T)`, the old name hijacked `System.Linq.Enumerable.AsEnumerable` for arrays and lists.
- `ToConsecutivePairs` and `ToConsecutiveTriples` return nothing for an empty sequence.
- `Merge` keeps default values of value types (e.g. `0`), sorts numbers numerically and strings ordinally.
- `GroupByHash` compares the property values themselves instead of a hash, the dependency on `budul.HashExtensions` is gone.
- Argument validation of iterator based methods now happens at the call, not at the first enumeration.

## License

This project is licensed under the MIT License.
