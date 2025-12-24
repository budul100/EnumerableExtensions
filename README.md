# EnumerableExtensions

A collection of extension methods for working with enumerable data in .NET, providing utilities for counting, merging, converting, and manipulating collections.

## Installation

Install via NuGet:

```dotnet add package budul.EnumerableExtensions```

Or visit: https://www.nuget.org/packages/budul.EnumerableExtensions

## Features

### Counting & Aggregation
- `CountOrDefault<T>()` - Returns count or 0 if empty
- `MaxOrDefault<T>()` / `MaxOrNull<T>()` - Safe max operations
- `MinOrDefault<T>()` / `MinOrNull<T>()` - Safe min operations

### Merging
- `Merge()` - Combines enumerable items into a delimited string with optional sorting and distinct filtering
- `Merge<T, TProp>()` - Merges items by selecting a specific property

### Conversion
- `ToArrayOrDefault()` - Converts to array or returns null if empty
- `ToListOrDefault()` - Converts to list or returns null if empty

## Usage Example

```csharp
using EnumerableExtensions;
var items = new[] { "c", "b", "a", "c" };

// Merge with sorting and distinct 
string result = items.Merge(); // "a,b,c"

// Merge without sorting 
string unsorted = items.Merge(preventSort: true); // "c,b,a,c"

// Safe counting 
int count = items.CountOrDefault(); // 4
```

## Target Frameworks

- .NET Standard 2.0 / 2.1
- .NET 7 / 8

## License

See LICENSE file for details.