using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vest
{
    class Polygon
    {
        bool convex;
        Edge[] edges;
        Vector2[] satHelpers;
        Vector2 center;
        float height;

        //Used so that inherited classes can potentially reorder input data
        protected bool WasNotCounterClockwise = false;

        public Polygon(Vector2[] points, float height = 0)
        {
            this.height = height;
            CalculateValues(points);
        }

        private void CalculateValues(Vector2[] points)
        {
            if (points.Length < 3)
                throw new ArgumentException("Polygon must consist of at least 2 points");

            edges = new Edge[points.Length];
            center = Vector2.Zero;
            for (int x = 0; x < points.Length - 1; x++)
            {
                edges[x] = new Edge(points[x], points[x + 1]);
                center += points[x];
            }
            edges[edges.Length - 1] = new Edge(points[points.Length - 1], points[0]);

            center += points[points.Length - 1];
            center /= points.Length;

            convex = CheckIfConvex();

            CalculateSATHelpers();
        }

        //Needed information to optimize collision detection
        private bool CheckIfConvex()
        {
            //Check last point first
            Vector2 dir = edges[0].End - edges[edges.Length - 1].Start;
            Vector2 normal = new Vector2(dir.Y, -dir.X);
            float proj = Vector2.Dot(normal, edges[edges.Length - 1].End - edges[edges.Length - 1].Start);
            if (proj < 0)
                return false;

            for (int x = 0; x < edges.Length - 1; x++)
            {
                //Remove connect two points leaving out the one in the middle
                //Check if left out point is closer to the center or further away
                //If closer the polygon is not convex
                //Calculate normal
                dir = edges[x + 1].End - edges[x].Start;
                normal = new Vector2(dir.Y, -dir.X);
                //Dot(Normal, Left out point)
                proj = Vector2.Dot(normal, edges[x].End - edges[x].Start);
                if (proj < 0)
                    return false;
            }
            return true;
        }

        public void MoveTo(Vector2 position)
        {
            Move(position - center);
        }

        public void Move(Vector2 diff)
        {
            center += diff;
            for (int x = 0; x < edges.Length; x++)
            {
                edges[x].Start += diff;
                edges[x].End += diff;
            }
        }

        private bool CheckCounterClockwise()
        {
            //Checking only one edge is sufficient since we are assuming convex polygons
            //and the sign is flipped for an inverted polygon
            return Vector2.Dot(edges[0].Normal, edges[0].Start) > Vector2.Dot(edges[0].Normal, center);
        }

        private void TurnCounterClockwise()
        {
            if (CheckCounterClockwise())
                return;
            //Invert the order of vertices
            for (int x = 0; x < edges.Length; x++)
            {
                Vector2 helper = edges[x].Start;
                edges[x].Start = edges[x].End;
                edges[x].End = helper;
                edges[x].Normal = -edges[x].Normal;
                edges[x].Dir = -edges[x].Dir;
            }
        }

        //Calculate how far the polygon extends along each normal
        //to speed up SAT calculation
        private void CalculateSATHelpers()
        {
            satHelpers = new Vector2[edges.Length];
            for (int x = 0; x < satHelpers.Length; x++)
            {
                float min = float.MaxValue;
                float max = float.MinValue;

                for (int y = 0; y < satHelpers.Length; y++)
                {
                    float proj = Vector2.Dot(edges[x].Normal, edges[y].Start - center);
                    if (proj < min)
                        min = proj;
                    if (proj > max)
                        max = proj;
                }

                satHelpers[x].X = min;
                satHelpers[x].Y = max;
            }
        }

        public bool Intersects(Polygon polygon)
        {
            //Check SAT, SAT will yield correct negatives for all polygons
            //But may yield false positives for concave polygons
            //So we check SAT first since we expect most things not to intersect
            //And if SAT says we collide and one polygon is not convex we test more intensively
            if (!InternalSATIntersect(polygon))
                return false;
            if (!polygon.InternalSATIntersect(this))
                return false;
            //For convex polygons SAT is sufficient
            if (IsConvex && polygon.IsConvex)
                return true;
            //TODO: For concave polygons we will need a more extensive check
            return true;
        }

        //Helper function to check if two ranges overlap
        private bool RangeOverlaps(Vector2 one, Vector2 two)
        {
            return (one.Y >= two.X && two.Y >= one.X);
        }

        private bool InternalSATIntersect(Polygon polygon)
        {
            //For every normal check if the projection of this polygon
            //intersects with the projection of the other one
            //If yes try next normal, if not we don't intersect
            for (int x = 0; x < satHelpers.Length; x++)
            {
                Vector2 extent = new Vector2(float.MaxValue, float.MinValue);

                for (int y = 0; y < satHelpers.Length; y++)
                {
                    float proj = Vector2.Dot(edges[x].Normal, polygon.edges[y].Start-center);
                    if (proj < extent.X)
                        extent.X = proj;
                    if (proj > extent.Y)
                        extent.Y = proj;
                }
                if (!RangeOverlaps(satHelpers[x], extent))
                    return false;
            }
            return true;
        }

        public bool IsConvex
        {
            get { return convex; }
        }

        public Edge[] Edges
        {
            get { return edges; }
        }

        public Vector2 Center
        {
            get { return center; }
        }

        public float Height
        {
            get { return height; }
        }
    }
}
