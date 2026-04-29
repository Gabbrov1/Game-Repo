using OpenTK.Mathematics;

namespace MineBlox.Engine.Camera
{
    public class Frustum
    {
        private struct Plane
        {
            public Vector3 Normal;
            public float Distance;
        }

        private readonly Plane[] _planes = new Plane[6];

        public void Update(Matrix4 viewProjection)
        {
            _planes[0] = ExtractPlane(viewProjection.Row3 + viewProjection.Row0);
            _planes[1] = ExtractPlane(viewProjection.Row3 - viewProjection.Row0);

            _planes[2] = ExtractPlane(viewProjection.Row3 + viewProjection.Row1);
            _planes[3] = ExtractPlane(viewProjection.Row3 - viewProjection.Row1);

            _planes[4] = ExtractPlane(viewProjection.Row3 + viewProjection.Row2);
            _planes[5] = ExtractPlane(viewProjection.Row3 - viewProjection.Row2);

        }

        public bool IsBoxInFrustum(Vector3 min, Vector3 max)
        {
            foreach (Plane plane in _planes)
            {
                Vector3 positive = new Vector3(
                    plane.Normal.X >= 0 ? max.X : min.X,
                    plane.Normal.Y >= 0 ? max.Y : min.Y,
                    plane.Normal.Z >= 0 ? max.Z : min.Z
                );

                if (Vector3.Dot(plane.Normal, positive) + plane.Distance < 0)
                    return false;
            }
            return true;
        }

        private static Plane ExtractPlane(Vector4 row)
        {
            float length = row.Xyz.Length;
            return new Plane
            {
                Normal = row.Xyz / length,
                Distance = row.W / length
            };
        }
    }
}