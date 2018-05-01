using System.Runtime.CompilerServices;

namespace KdTree.Math
{
	public struct GeoLocation : IBundle<float>
	{
		public float Latitude;
		public float Longitude;

		public GeoLocation(float latitude, float longitude) => (Latitude, Longitude) = (latitude, longitude);

		public float this[int index]
		{
			get => Unsafe.Add(ref Unsafe.As<GeoLocation, float>(ref this), index);
			set => Unsafe.Add(ref Unsafe.As<GeoLocation, float>(ref this), index) = value;
		}

		public int Length => 2;
	}

	public struct FloatGeoMetic : IMetrics<float, GeoLocation>
	{
		public float DistanceSquaredBetweenPoints(GeoLocation a, GeoLocation b)
		{
			double dst = GeoUtils.Distance(a.Latitude, a.Longitude, b.Latitude, b.Longitude, 'K');
			return (float)(dst * dst);
		}

		public bool Equals(GeoLocation x, GeoLocation y) => x.Latitude == y.Latitude && x.Longitude == y.Longitude;
		public int GetHashCode(GeoLocation obj) => obj.GetHashCode();
	}
}
