using System;
using System.Collections.Generic;
using UnityEngine;

    public static class MathUtil2D
    {
        public static Vector2 ToVec2(this  Vector3 vec3)
        {
            return new Vector2(vec3.x, vec3.y);
        }

        public static Vector3 ToVec3(this Vector2 vec3)
        {
            return new Vector3(vec3.x, vec3.y,0);
        }

        /// <summary>
        /// 获取两圆之间的距离向量
        /// </summary>
        /// <returns>距离向量</returns>
        /// <param name="position1">Position1.</param>
        /// <param name="radius1">Radius1.</param>
        /// <param name="position2">Position2.</param>
        /// <param name="radius2">Radius2.</param>
        /// <param name="dis">输出两圆距离</param>
        public static Vector3 GetDistance(Vector3 position1, double radius1, Vector3 position2, double radius2, out double dis)
        {
            position1.z = 0;
            position2.z = 0;
            Vector3 sourceDir = position2 - position1;
            sourceDir.z = 0;
            dis = sourceDir.magnitude - (radius1 + radius2);
            if (dis > 0)
            {
                Vector3 rd = (float)dis * sourceDir.normalized;
                return rd;
            }
            else
            {
                return Vector3.zero;
            }
        }

        /// <summary>
        /// 获取两圆之间的距离向量
        /// </summary>
        /// <returns>距离向量</returns>
        /// <param name="position1">Position1.</param>
        /// <param name="radius1">Radius1.</param>
        /// <param name="position2">Position2.</param>
        /// <param name="radius2">Radius2.</param>
        public static Vector3 GetDistance(Vector3 position1, double radius1, Vector3 position2, double radius2)
        {
            double dis;
            position1.z = 0;
            position2.z = 0;
            Vector3 d = position2 - position1;
            d.z = 0;
            dis = d.magnitude - (radius1 + radius2);
            if (dis > 0)
            {
                Vector3 rd = (float)dis * d.normalized;
                return rd;
            }
            else
            {
                return Vector3.zero;
            }
        }

        /// <summary>
        /// 转换相对坐标为绝对坐标
        /// </summary>
        /// <returns>绝对坐标</returns>
        /// <param name="relativePosition">Relative position.</param>
        /// <param name="sourcePos">Source position.</param>
        /// <param name="forawrd">Forawrd.</param>
        /// <param name="dir">Dir.</param>
        public static Vector3 TransPosition(Vector3 relativePosition, Vector3 sourcePos, Vector3 forawrd, out Vector3 dir)
        {
            double x = relativePosition.y * forawrd.x - relativePosition.x * forawrd.y;
            double y = relativePosition.y * forawrd.y + relativePosition.x * forawrd.x;
            Vector3 newPos = new Vector3(sourcePos.x + (float)x,  sourcePos.y + (float)y,sourcePos.z + relativePosition.z);
            dir = newPos - sourcePos;
            return newPos;

        }

        public static float zeroDis = 0.001f;
        public static float zeroDisSqrt = 0.000001f;

        // /// <summary>
        // /// Sqrts the distance no y.
        // /// </summary>
        // /// <returns>The distance no y.</returns>
        // /// <param name="a">The alpha component.</param>
        // /// <param name="b">The blue component.</param>
        // public static float SqrtDistanceNoY(Vector3 a, Vector3 b)
        // {
        //     float x = a.x - b.x;
        //     float y = a.y - b.y;
        //     return x * x + y * y;
        // }
        //
        // /// <summary>
        // /// Distances the no y.
        // /// </summary>
        // /// <returns>The no y.</returns>
        // /// <param name="a">The alpha component.</param>
        // /// <param name="b">The blue component.</param>
        // public static float DistanceNoY(Vector3 a, Vector3 b)
        // {
        //     float sqrtDis = SqrtDistanceNoY(a, b);
        //     return Mathf.Sqrt(sqrtDis);
        // }

        /// <summary>
        /// Points the in ellipse.
        /// </summary>
        /// <returns><c>true</c>, if in ellipse was pointed, <c>false</c> otherwise.</returns>
        /// <param name="position0">Position0.</param>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="w">The width.</param>
        /// <param name="l">L.</param>
        public static bool PointInEllipse(Vector3 position0, float a, float b, float w, float l)
        {
            return (w * w) / (a * a) + (l * l) / (b * b) <= 1;
        }

        /// <summary>
        /// cneter在矩形中点
        /// </summary>
        /// <returns><c>true</c>, if in rect was pointed, <c>false</c> otherwise.</returns>
        /// <param name="position0">Position0.</param>
        /// <param name="radiu">Radiu.</param>
        /// <param name="dir">Dir.</param>
        /// <param name="center">Center.</param>
        /// <param name="length">Length.</param>
        /// <param name="width">Width.</param>
        public static bool PointInRect(Vector3 position0, float radiu, Vector3 dir, Vector3 center, float length, float width)
        {
            Vector3 dir0 = dir.normalized;
            Vector3 dir1 = dir0;
            Vector3 dir2 = Rotation(dir0, 90);//Quaternion.Euler(0, 90, 0) * dir0;

            Vector3 position1 = center - dir2 * length / 2 - dir1 * (width / 2 + radiu);
            Vector3 position2 = center + dir2 * length / 2 - dir1 * (width / 2 + radiu);
            //            Vector3 position3 = center + dir2 * length / 2 + dir1 * (width / 2 + radiu);
            Vector3 position4 = center - dir2 * length / 2 + dir1 * (width / 2 + radiu);

            Vector3 offsetX = position2 - position1;
            Vector3 offsetZ = position4 - position1;

            float projectX = Vector3.Dot(position0 - position1, offsetX);
            float projectZ = Vector3.Dot(position0 - position1, offsetZ);
            if (projectX <= offsetX.sqrMagnitude && projectX >= 0 && projectZ <= offsetZ.sqrMagnitude && projectZ >= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// cneter在矩形下方
        /// </summary>
        /// <returns><c>true</c>, if in rect by select was pointed, <c>false</c> otherwise.</returns>
        /// <param name="position0">Position0.</param>
        /// <param name="radiu">Radiu.</param>
        /// <param name="dir">Dir.</param>
        /// <param name="center">Center.</param>
        /// <param name="length">Length.</param>
        /// <param name="width">Width.</param>
        public static bool PointInRectBySelect(Vector3 position0, double radiu, Vector3 dir, Vector3 center, double length, double width)
        {
            Vector3 dir0 = dir.normalized;
            Vector3 dir1 = dir0;
            Vector3 dir2 = Rotation(dir0, 90); //Quaternion.Euler(0, 90, 0) * dir0;

            Vector3 position1 = center - dir2 * (float)width / 2;
            Vector3 position2 = center + dir2 * (float)width / 2;
            //            Vector3 position3 = center + dir2 * (float)width / 2 + dir1 * (float)(length + radiu);
            Vector3 position4 = center - dir2 * (float)width / 2 + dir1 * (float)(length + radiu);

            Vector3 offsetX = position2 - position1;
            Vector3 offsetZ = position4 - position1;

            double projectX = Vector3.Dot(position0 - position1, offsetX);
            double projectZ = Vector3.Dot(position0 - position1, offsetZ);
            if (projectX <= offsetX.sqrMagnitude && projectX >= 0 && projectZ <= offsetZ.sqrMagnitude && projectZ >= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points the in rect center at down.
        /// </summary>
        /// <returns><c>true</c>, if in rect center at down was pointed, <c>false</c> otherwise.</returns>
        /// <param name="p">P.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <param name="p3">P3.</param>
        /// <param name="p4">P4.</param>
        public static bool PointInRectCenterAtDown(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            Vector3 offsetX = p2 - p1;
            Vector3 offsetZ = p4 - p1;

            double projectX = Vector3.Dot(p - p1, offsetX);
            double projectZ = Vector3.Dot(p - p1, offsetZ);
            if (projectX <= offsetX.sqrMagnitude && projectX >= 0 && projectZ <= offsetZ.sqrMagnitude && projectZ >= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points the in circle.
        /// </summary>
        /// <returns><c>true</c>, if in circle was pointed, <c>false</c> otherwise.</returns>
        /// <param name="position0">Position0.</param>
        /// <param name="ignoreRadiu">Ignore radiu.</param>
        /// <param name="dir">Dir.</param>
        /// <param name="center">Center.</param>
        /// <param name="radius">Radius.</param>
        /// <param name="angle">Angle.</param>
        public static bool PointInCircle(Vector3 position0, double ignoreRadiu, Vector3 dir, Vector3 center, double radius, double angle)
        {
            if (angle == 360)
                return PointIn360Circle(position0, ignoreRadiu, center, radius);

            Vector3 offsetAngle = position0 - center;
            offsetAngle.z = 0;
            double angleD2 = Vector3.Angle(dir, offsetAngle);
            if (GetDistance(position0, ignoreRadiu, center, radius) == Vector3.zero
               && angleD2 <= angle * 0.5f)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points the in360 circle.
        /// </summary>
        /// <returns><c>true</c>, if in360 circle was pointed, <c>false</c> otherwise.</returns>
        /// <param name="position0">Position0.</param>
        /// <param name="ignoreRadiu">Ignore radiu.</param>
        /// <param name="center">Center.</param>
        /// <param name="radius">Radius.</param>
        public static bool PointIn360Circle(Vector3 position0, double ignoreRadiu, Vector3 center, double radius)
        {
            Vector3 offsetAngle = position0 - center;
            offsetAngle.z = 0;
            if (GetDistance(position0, ignoreRadiu, center, radius) == Vector3.zero)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Rotation the specified source and angle.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="angle">Angle.</param>
        public static Vector3 Rotation(Vector3 source, double angle)
        {
            double x = source.x;
            double y = source.y;

            double rad = Mathf.Deg2Rad * angle;

            double target_x = x * Mathf.Cos((float)rad) + y * Mathf.Sin((float)rad);
            double target_y = x * Mathf.Sin(-(float)rad) + y * Mathf.Cos((float)rad);

            return new Vector3((float)target_x, (float)target_y,0 );
        }


        /// <summary>
        /// Vectors the T euler.
        /// </summary>
        /// <returns>The T euler.</returns>
        /// <param name="source">Source.</param>
        public static float VectorTEuler(Vector3 source)
        {
            return Mathf.Atan2(source.y, source.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Eulers the T vector.
        /// </summary>
        /// <returns>The T vector.</returns>
        /// <param name="angle">Angle.</param>
        public static Vector3 EulerTVector(float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad),0);
        }

        /// <summary>
        ///  根据相对位置求绝对位置
        /// </summary>
        /// <returns>The relative position.</returns>
        /// <param name="offset">Offset.</param>
        /// <param name="tran">Tran.</param>
        public static Vector3 GetWorldPosFromRelativePos(Vector3 offset, Transform tran)
        {
            Vector3 x = offset.x * tran.right;
            Vector3 y = offset.y * tran.up;
            Vector3 z = offset.z * tran.forward;
            return x + y + z + tran.position;
        }

        public static Vector3 GetRelativePosFromWorldPos(Transform center, Transform trans)
        {
            Vector3 dir = trans.position - center.position;
            Vector3 relativeX = Vector3.Project(dir, center.right);
            Vector3 relativeZ = Vector3.Project(dir, center.forward);
            Vector3 relativeY = Vector3.Project(dir, center.up);

            int xline = Vector3.Dot(relativeX, center.right) > 0 ? 1 : -1;
            int yline = Vector3.Dot(relativeY, center.up) > 0 ? 1 : -1;
            int zline = Vector3.Dot(relativeZ, center.forward) > 0 ? 1 : -1;

            return new Vector3(relativeX.magnitude * xline, relativeY.magnitude * yline, relativeZ.magnitude * zline);

        }


        /// <summary>
        /// 得到dir在Forward的左边还是右边
        /// </summary>
        /// <returns>The dir forward type.</returns>
        /// <param name="forward">Forward.</param>
        /// <param name="dir">Dir.</param>
        internal static DirForwardType GetDirForwardType(Vector3 forward, Vector3 dir)
        {
            Vector2 A = new Vector2(0, 0);
            Vector2 B = new Vector2(forward.x, forward.z);
            Vector2 C = new Vector2(dir.x, dir.z);
            var s = (A.x - C.x) * (B.y - C.y) - (A.y - C.y) * (B.x - C.x);
            if (s > 0)
            {
                return DirForwardType.LEFT;
            }
            else if (s < 0)
            {
                return DirForwardType.RIGHT;
            }
            else
            {
                return DirForwardType.LINE;
            }
        }

        /// <summary>
        /// 求两个向量之间的角度 带正负的
        /// </summary>
        /// <param name="from">起始向量</param>
        /// <param name="to">终止向量</param>
        /// <param name="n">法向量</param>
        /// <returns></returns>
        public static float AngleBettwenVector(Vector3 from, Vector3 to, Vector3 n)
        {
            return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(from, to)), Vector3.Dot(from, to)) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 求线段交点
        /// </summary>
        /// <returns>The point by2 vec.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        public static Vector2 CrossPointBy2Vec(Line2D a, Line2D b)
        {
            Vector2 hp = new Vector2();
            double D = (a.endPoint.x - a.startPoint.x) * (b.startPoint.y - b.endPoint.y)
                - (b.endPoint.x - b.startPoint.x) * (a.startPoint.y - a.endPoint.y);
            double D1 = (b.startPoint.y * b.endPoint.x - b.startPoint.x * b.endPoint.y)
                * (a.endPoint.x - a.startPoint.x)
                - (a.startPoint.y * a.endPoint.x - a.startPoint.x * a.endPoint.y)
                * (b.endPoint.x - b.startPoint.x);
            double D2 = (a.startPoint.y * a.endPoint.x - a.startPoint.x * a.endPoint.y)
                * (b.startPoint.y - b.endPoint.y)
                - (b.startPoint.y * b.endPoint.x - b.startPoint.x * b.endPoint.y)
                * (a.startPoint.y - a.endPoint.y);
            hp.x = (float)(D1 / D);
            hp.y = (float)(D2 / D);
            return hp;
        }


        /// <summary>
        /// 求点到直线线段垂直点
        /// </summary>
        /// <returns>The point.</returns>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <param name="point">Point.</param>
        public static Vector3 NearDistancePoint(Vector3 _p1, Vector3 _p2, Vector3 _point)
        {
            var p1 = _p1.ToVec2();
            var p2 = _p2.ToVec2();
            var point = _point.ToVec2();

            //直线方程
            var A = p2.y - p1.y;
            var B = p1.x - p2.x;
            var C = (p1.y - p2.y) * p1.x - p1.y * (p1.x - p2.x);

            float x;
            float y;

            if (Math.Abs(A * point.x + B * point.y + C) < 1e-13)
            {
                x = point.x;
                y = point.y;
            }
            else
            {
                x = (B * B * point.x - A * B * point.y - A * C) / (A * A + B * B);
                y = (-A * B * point.x + A * A * point.y - B * C) / (A * A + B * B);
            }


            return new Vector2(x, y).ToVec3();



        }
        
        /// <summary>
        /// 斜率方程
        /// </summary>
        /// <returns>The kb.</returns>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        static Vector2 GetKB(Vector2 p1, Vector2 p2)
        {
            if (p1.x == p2.x)
            {
                return Vector2.zero;
            }
            float k, b;
            k = (p2.y - p1.y) / (p2.x - p1.x);

            b = p1.y - (p1.x * (p2.y - p1.y) / (p2.x - p1.x));
            if (p1.y == p2.y)
            {
                k = 0;
            }
            return new Vector2(k, b);
        }

        /// <summary>
        /// 求到圆弧的最近点
        /// </summary>
        /// <returns>The near points.</returns>
        /// <param name="center">Center.</param>
        /// <param name="p2">P2.</param>
        /// <param name="r">The red component.</param>
        public static Vector3 GetNearCirclePoints(Vector3 _center, Vector3 _p2, float r)
        {
            var center = _center.ToVec2();
            var p2 = _p2.ToVec2();

            if (center == p2)
            {
                return center;
            }

            float x1, y1, x2, y2;
            var m = center.x;
            var n = center.y;//-- 圆心坐标m, n
            if (center.x != p2.x)
            {

                var vec2 = GetKB(center, p2);
                var k = vec2.x;
                var b = vec2.y;

                float A, B, C; //--转换一元二次方程Ax ^ 2 + Bx + C = 0
                A = 1 + k * k;
                B = 2 * k * (b - n) - 2 * m;
                C = m * m + (b - n) * (b - n) - r * r;

                x1 = (-B + Mathf.Sqrt(Mathf.Pow(B, 2) - 4 * A * C)) / (2 * A);
                y1 = k * x1 + b;

                x2 = (-B - Mathf.Sqrt(Mathf.Pow(B, 2) - 4 * A * C)) / (2 * A);
                y2 = k * x2 + b;
            }
            else
            {
                x1 = m;
                x2 = m;

                y1 = n + r;
                y2 = n - r;
            }

            var v1 = new Vector2(x1, y1);
            var v2 = new Vector2(x2, y2);

            var d1 = v1 - p2;
            var d2 = v2 - p2;
            if(d1.sqrMagnitude>d2.sqrMagnitude){
                return v2.ToVec3();
            }else{
                return v1.ToVec3();
            }
        }
        
        
        /// <summary>
        /// Sqrts the distance no y.
        /// </summary>
        /// <returns>The distance no y.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        public static float SqrtDistanceNoZ(Vector3 a, Vector3 b)
        {
            float x = a.x - b.x;
            float y = a.y - b.y;
            return x * x + y * y;
        }

        /// <summary>
        /// Distances the no y.
        /// </summary>
        /// <returns>The no y.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        public static float DistanceNoZ(Vector3 a, Vector3 b)
        {
            float sqrtDis = SqrtDistanceNoZ(a, b);
            return Mathf.Sqrt(sqrtDis);
        }
    }

    public enum DirForwardType
    {
        LEFT,
        RIGHT,
        LINE,
    }
    