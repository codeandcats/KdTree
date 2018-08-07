using KdTree.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

struct City
{
	public string Address;
	public float Lat;
	public float Long;
	public float DistanceFromToowoomba;
}

namespace KdTree.Tests
{
	[TestClass]
	public class KdTreeTests
	{
		private KdTree<float, string> tree;

		[TestInitialize]
		public void Setup()
		{
			tree = new KdTree<float, string>(2, new FloatMath());

			testNodes = new List<KdTreeNode<float, string>>();
			testNodes.AddRange(new KdTreeNode<float, string>[]
			{
				new KdTreeNode<float, string>(new float[] { 5, 5 }, "Root"),

				new KdTreeNode<float, string>(new float[] { 2.5f, 2.5f }, "Root-Left"),
				new KdTreeNode<float, string>(new float[] { 7.5f, 7.5f }, "Root-Right"),

				new KdTreeNode<float, string>(new float[] { 1, 10 }, "Root-Left-Left"),

				new KdTreeNode<float, string>(new float[] { 10, 10 }, "Root-Right-Right")
			});
		}

		[TestCleanup]
		public void TearDown()
		{
			tree = null;
		}

		private List<KdTreeNode<float, string>> testNodes;

		private void AddTestNodes()
		{
			foreach (var node in testNodes)
				if (!tree.Add(node.Point, node.Value))
					throw new Exception("Failed to add node to tree");
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestAdd()
		{
			// Add nodes to tree
			AddTestNodes();

			// Check count of nodes is right
			Assert.AreEqual(testNodes.Count, tree.Count);
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestAddDuplicateInSkipMode()
		{
			tree = new KdTree<float, string>(2, new FloatMath());

			Assert.AreEqual(AddDuplicateBehavior.Skip, tree.AddDuplicateBehavior);

			AddTestNodes();

			var count = tree.Count;

			var added = tree.Add(testNodes[0].Point, "Some other value");

			Assert.AreEqual(false, added);
			Assert.AreEqual(count, tree.Count);
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestAddDuplicateInErrorMode()
		{
			tree = new KdTree<float, string>(2, new FloatMath(), AddDuplicateBehavior.Error);

			AddTestNodes();

			var count = tree.Count;
			Exception error = null;

			try
			{
				tree.Add(testNodes[0].Point, "Some other value");
			}
			catch (Exception e)
			{
				error = e;
			}

			Assert.AreEqual(count, tree.Count);
			Assert.IsNotNull(error);
			Assert.IsInstanceOfType(error, typeof(DuplicateNodeError));
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestAddDuplicateInUpdateMode()
		{
			tree = new KdTree<float, string>(2, new FloatMath(), AddDuplicateBehavior.Update);

			AddTestNodes();

			var newValue = "I love chicken, I love liver, Meow Mix Meow Mix please deliver";

			var olcCount = tree.Count();

			tree.Add(testNodes[0].Point, newValue);

			var actualValue = tree.FindValueAt(testNodes[0].Point);
			var newCount = tree.Count();

			Assert.AreEqual(newValue, actualValue);
			Assert.AreEqual(olcCount, newCount);
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestTryFindValueAt()
		{
			AddTestNodes();

			string actualValue;

			foreach (var node in testNodes)
			{
				if (tree.TryFindValueAt(node.Point, out actualValue))
					Assert.AreEqual(node.Value, actualValue);
				else
					Assert.Fail("Could not find test node");
			}

			if (!tree.TryFindValueAt(new float[] { 3.14f, 5 }, out actualValue))
				Assert.IsNull(actualValue);
			else
				Assert.Fail("Reportedly found node it shouldn't have");
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestFindValueAt()
		{
			AddTestNodes();

			string actualValue;

			foreach (var node in testNodes)
			{
				actualValue = tree.FindValueAt(node.Point);

				Assert.AreEqual(node.Value, actualValue);
			}

			actualValue = tree.FindValueAt(new float[] { 3.15f, 5 });

			Assert.IsNull(actualValue);
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestFindValue()
		{
			AddTestNodes();

			float[] actualPoint;

			foreach (var node in testNodes)
			{
				actualPoint = tree.FindValue(node.Value);
				Assert.AreEqual(node.Point, actualPoint);
			}

			actualPoint = tree.FindValue("Your Mumma");
			Assert.IsNull(actualPoint);
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestRemoveAt()
		{
			AddTestNodes();

			var nodesToRemove = new KdTreeNode<float, string>[] {
				testNodes[1], // Root-Left
				testNodes[0] // Root
			};

			foreach (var nodeToRemove in nodesToRemove)
			{
				tree.RemoveAt(nodeToRemove.Point);
				testNodes.Remove(nodeToRemove);

				Assert.IsNull(tree.FindValue(nodeToRemove.Value));
				Assert.IsNull(tree.FindValueAt(nodeToRemove.Point));

				foreach (var testNode in testNodes)
				{
					Assert.AreEqual(testNode.Value, tree.FindValueAt(testNode.Point));
					Assert.AreEqual(testNode.Point, tree.FindValue(testNode.Value));
				}

				Assert.AreEqual(testNodes.Count, tree.Count);
			}
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestGetNearestNeighbours()
		{
			var toowoomba = new City()
			{
				Address = "Toowoomba, QLD, Australia",
				Lat = -27.5829487f,
				Long = 151.8643252f,
				DistanceFromToowoomba = 0
			};

			City[] cities = new City[]
			{
				toowoomba,
				new City()
				{
					Address = "Brisbane, QLD, Australia",
					Lat = -27.4710107f,
					Long = 153.0234489f,
					DistanceFromToowoomba = 1.16451615177537f
				},
				new City()
				{
					Address = "Goldcoast, QLD, Australia",
					Lat = -28.0172605f,
					Long = 153.4256987f,
					DistanceFromToowoomba = 1.6206523211724f
				},
				new City()
				{
					Address = "Sunshine, QLD, Australia",
					Lat = -27.3748288f,
					Long = 153.0554193f,
					DistanceFromToowoomba = 1.20913979664506f
				},
				new City()
				{
					Address = "Melbourne, VIC, Australia",
					Lat = -37.814107f,
					Long = 144.96328f,
					DistanceFromToowoomba = 12.3410301438779f
				},
				new City()
				{
					Address = "Sydney, NSW, Australia",
					Lat = -33.8674869f,
					Long = 151.2069902f,
					DistanceFromToowoomba = 6.31882185929341f
				},
				new City()
				{
					Address = "Perth, WA, Australia",
					Lat = -31.9530044f,
					Long = 115.8574693f,
					DistanceFromToowoomba = 36.2710774395312f
				},
				new City()
				{
					Address = "Darwin, NT, Australia",
					Lat = -12.4628198f,
					Long = 130.8417694f,
					DistanceFromToowoomba = 25.895292049265f
				}
				/*,
				new City()
				{
					Address = "London, England",
					Lat = 51.5112139f,
					Long = -0.1198244f,
					DistanceFromToowoomba = 171.33320836029f
					
				}*/
			};

			foreach (var city in cities)
			{
				tree.Add(new float[] { city.Long, -city.Lat }, city.Address);
			}

			/*
			var sb = new System.Text.StringBuilder();
			sb.AppendLine("Before Balance:");
			sb.AppendLine(tree.ToString());
			sb.AppendLine("");
			sb.AppendLine("");
			tree.Balance();
			sb.AppendLine("After Balance:");
			sb.AppendLine(tree.ToString());
			System.Windows.Forms.Clipboard.SetText(sb.ToString());
			*/

			for (var findLimit = 0; findLimit <= cities.Length; findLimit++)
			{
				var actualNeighbours = tree.GetNearestNeighbours(
					new float[] { toowoomba.Long, -toowoomba.Lat },
					findLimit);

				var expectedNeighbours = cities
					.OrderBy(p => p.DistanceFromToowoomba)
					.Take(findLimit)
					.ToArray();

				Assert.AreEqual(findLimit, actualNeighbours.Length);
				Assert.AreEqual(findLimit, expectedNeighbours.Length);

				for (var index = 0; index < actualNeighbours.Length; index++)
				{
					Assert.AreEqual(expectedNeighbours[index].Address, actualNeighbours[index].Value);
				}
			}
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestRadialSearch()
		{
			var toowoomba = new City()
			{
				Address = "Toowoomba, QLD, Australia",
				Lat = -27.5829487f,
				Long = 151.8643252f,
				DistanceFromToowoomba = 0
			};

			City[] cities = new City[]
			{
				toowoomba,
				new City()
				{
					Address = "Brisbane, QLD, Australia",
					Lat = -27.4710107f,
					Long = 153.0234489f,
					DistanceFromToowoomba = 1.16451615177537f
				},
				new City()
				{
					Address = "Goldcoast, QLD, Australia",
					Lat = -28.0172605f,
					Long = 153.4256987f,
					DistanceFromToowoomba = 1.6206523211724f
				},
				new City()
				{
					Address = "Sunshine, QLD, Australia",
					Lat = -27.3748288f,
					Long = 153.0554193f,
					DistanceFromToowoomba = 1.20913979664506f
				},
				new City()
				{
					Address = "Melbourne, VIC, Australia",
					Lat = -37.814107f,
					Long = 144.96328f,
					DistanceFromToowoomba = 12.3410301438779f
				},
				new City()
				{
					Address = "Sydney, NSW, Australia",
					Lat = -33.8674869f,
					Long = 151.2069902f,
					DistanceFromToowoomba = 6.31882185929341f
				},
				new City()
				{
					Address = "Perth, WA, Australia",
					Lat = -31.9530044f,
					Long = 115.8574693f,
					DistanceFromToowoomba = 36.2710774395312f
				},
				new City()
				{
					Address = "Darwin, NT, Australia",
					Lat = -12.4628198f,
					Long = 130.8417694f,
					DistanceFromToowoomba = 25.895292049265f
				}
			};

			foreach (var city in cities)
			{
				tree.Add(new float[] { city.Long, -city.Lat }, city.Address);
			}
			var expectedNeighbours = cities
				.OrderBy(p => p.DistanceFromToowoomba).ToList();

			for (var i = 1; i < 100; i *= 2)
			{
				var actualNeighbours = tree.RadialSearch(new float[] { toowoomba.Long, -toowoomba.Lat }, i);

				for (var index = 0; index < actualNeighbours.Length; index++)
				{
					Assert.AreEqual(expectedNeighbours[index].Address, actualNeighbours[index].Value);
				}
			}
		}

		[TestMethod]
		[TestCategory("KdTree")]
		public void TestEnumerable()
		{
			AddTestNodes();

			foreach (var node in tree)
			{
				var testNode = testNodes.FirstOrDefault(n => n.Point == node.Point && n.Value == node.Value);

				Assert.IsNotNull(testNode);

				testNodes.Remove(testNode);
			}

			Assert.AreEqual(0, testNodes.Count);
		}
	}
}