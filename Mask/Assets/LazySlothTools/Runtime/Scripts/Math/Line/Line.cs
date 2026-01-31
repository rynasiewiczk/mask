namespace LazySloth.Utilities.Math
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Line
    {
        public static Vector2 GetPointClosestToLineInDirection(List<Vector2> points, PositionWithDirection lineWithDirection, ref bool found)
        {
            var closestDistance = float.MaxValue;
            var closestPoint = lineWithDirection.Start;

            var line = GetLineParallelToVectorPassingThroughPoint(lineWithDirection.Direction, lineWithDirection.Start);

            foreach (var point in points)
            {
                float distance;
                //line is default when it's a vertical line; vertical line is not a function, and because of that it needs a special treatment
                if (line == default)
                {
                    distance = Mathf.Abs(point.x - lineWithDirection.Start.x);
                }
                else
                {
                    distance = GetDistanceFromLineToPoint(line, point);
                }

                if (distance < closestDistance)
                {
                    found = true;

                    closestDistance = distance;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }
        
        private static Vector2 GetLineParallelToVectorPassingThroughPoint(Vector2 vector, Vector2 point)
        {
            vector = vector.normalized;

            var pointInLine = new Vector2(point.x + vector.x, point.y + vector.y);

            var denominator = point.x - pointInLine.x;
            if (Math.Abs(point.x - pointInLine.x) < .01f)
            {
                return new Vector2();
            }

            var lineA = (point.y - pointInLine.y) / denominator;
            var lineB = point.x - point.x * lineA;

            var line = new Vector2(lineA, lineB);
            return line;
        }

        public static bool PointIsInGivenDirection(Vector2 point, PositionWithDirection direction)
        {
            var start = direction.Start;
            var inDirectionCheckpoint = direction.Direction + start;
            var inDirectionDistance = Vector2.Distance(new Vector2(point.x, point.y), inDirectionCheckpoint);

            var notInDirectionCheckpoint = -direction.Direction + start;
            var notInDirectionDistance = Vector2.Distance(new Vector2(point.x, point.y), notInDirectionCheckpoint);

            return inDirectionDistance <= notInDirectionDistance;
        }

        private static float GetDistanceFromLineToPoint(Vector2 line, Vector2 targetPoint)
        {
            var vectorBetweenTargetAndLine = GetVectorBetweenPointAndLine(targetPoint, line);

            return vectorBetweenTargetAndLine.magnitude;
        }

        public static Vector2 GetVectorBetweenPointAndLine(Vector2 point, Vector2 line)
        {
            var distance = (Mathf.Abs(line.x * point.x + line.y * point.y)) / Mathf.Sqrt(Mathf.Pow(line.x, 2) + Mathf.Pow(line.y, 2));
            Debug.Assert(distance >= 0, "Distance cannot be negative.");

            var pointOnLine1 = new Vector2(0, line.y);
            var pointOnLine2 = new Vector2(1, line.x + line.y);
            var a = point - pointOnLine1;
            var onLineDistance = pointOnLine2 - pointOnLine1;
            var vectorBetweenTargetAndLine = pointOnLine1 + Vector3.Dot(a, onLineDistance) /
                Vector3.Dot(onLineDistance, onLineDistance) * onLineDistance - point;
            return vectorBetweenTargetAndLine;
        }

        /// <summary>
        /// Return point of intersection between a line, and a line made from point that is perpendicular to the first line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 GetIntersectionPointBetweenLineAndPerpendicularToLineAndPoint(Vector2 line, Vector2 point)
        {
            var vectorBetweenTargetAndLine = GetVectorBetweenPointAndLine(point, line);

            var intersectionPoint = point + vectorBetweenTargetAndLine;
            return intersectionPoint;
        }

        /// <summary>
        /// Gets point of intersection between 2 lines.
        /// Returns positive infinity when lines are parallel.
        /// </summary>
        /// <param name="firstLinePoint1"></param>
        /// <param name="firstLinePoint2"></param>
        /// <param name="secondLinePoint1"></param>
        /// <param name="secondLinePoint2"></param>
        /// <returns></returns>
        public static Vector2 GetIntersectionPointForTwoLines(Vector2 firstLinePoint1, Vector2 firstLinePoint2, Vector2 secondLinePoint1, Vector2 secondLinePoint2)
        {
            //Line1
            var a1 = firstLinePoint2.y - firstLinePoint1.y;
            var b1 = firstLinePoint2.x - firstLinePoint1.x;
            var c1 = a1 * firstLinePoint1.x + b1 * firstLinePoint1.y;

            //Line2
            var a2 = secondLinePoint2.y - secondLinePoint1.y;
            var b2 = secondLinePoint2.x - secondLinePoint1.x;
            var c2 = a2 * secondLinePoint1.x + b2 * secondLinePoint1.y;

            var det = a1 * b2 - a2 * b1;
            if (det == 0)
            {
                Debug.LogWarning("Trying to get intersection point for parallel lines.");
                return Vector2.positiveInfinity;
            }

            var x = (b2 * c1 - b1 * c2) / det;
            var y = (a1 * c2 - a2 * c1) / det;
            return new Vector2(x, y);
        }

        public static Vector2 GetLine(Vector2 point1, Vector2 point2)
        {
            var a = (point2.y - point1.y) / (point2.x - point1.x);
            var b = point1.y - a * point1.x;
            return new Vector2(a, b);
        }

        public static Vector2 GetLinePerpendicularToLineGoingThroughPoint(Vector2 line, Vector3 point)
        {
            var perpendicular = new Vector2(-(1 / line.x), line.y);
            var perpB = point.y - (perpendicular.x * point.x);
            perpendicular = new Vector2(perpendicular.x, perpB);
            return perpendicular;
        }

        public static Vector2 GetProjectionPointOnLine(Vector2 point, Vector2 line)
        {
            var vectorBetweenPointAndLine =
                GetVectorBetweenPointAndLine(point, line);
            var projectionOnLine = point + vectorBetweenPointAndLine;

            return projectionOnLine;
        }

        public static Vector2 GetLinesIntersectionPoint(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out bool found)
        {
            var tmp = (b2.x - b1.x) * (a2.y - a1.y) - (b2.y - b1.y) * (a2.x - a1.x);

            if (tmp == 0)
            {
                // No solution!
                found = false;
                return Vector2.zero;
            }

            var mu = ((a1.x - b1.x) * (a2.y - a1.y) - (a1.y - b1.y) * (a2.x - a1.x)) / tmp;

            found = true;

            return new Vector2(
                b1.x + (b2.x - b1.x) * mu,
                b1.y + (b2.y - b1.y) * mu
            );
        }
    }
}