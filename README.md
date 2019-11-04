KdTree
======

A fast, generic, and multi-dimensional Binary Search Tree written in C#.

# Install

Install via NuGet Package Manager Console:

```
PM> Install-Package KdTree
```

# Examples 

## Find nearest point in two dimensions

```cs
var tree = new KdTree<float, int>(2, new FloatMath());
tree.Add(new[] { 50.0f, 80.0f }, 100);
tree.Add(new[] { 20.0f, 10.0f }, 200);

var nodes = tree.GetNearestNeighbours(new[] { 30.0f, 20.0f }, 1);
```
