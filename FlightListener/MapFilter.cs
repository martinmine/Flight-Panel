namespace FlightListener;

public class MapFilter
{
    public static IList<State> IsWithinInterestArea(IEnumerable<State> states)
    {
        (double lat, double lng)[] interestAreaTriangle =
        {
            (59.745788, 10.316217),
            (59.713850, 10.755098),            
            (59.969251, 10.759518),
        };
        
        return states.Where(state => isInside(
            interestAreaTriangle[0].lat, interestAreaTriangle[0].lng,
            interestAreaTriangle[1].lat, interestAreaTriangle[1].lng,
            interestAreaTriangle[2].lat, interestAreaTriangle[2].lng, state.Latitude, state.Longitude)).ToList();
        /*
         *
isInside(double x1, double y1, double x2,
        double y2, double x3, double y3,
        double x, double y)
         */
        return states.Where(state => IsPointInTriangle(
            state.Latitude, state.Longitude,
            interestAreaTriangle[0].lat, interestAreaTriangle[0].lng,
            interestAreaTriangle[1].lat, interestAreaTriangle[1].lng,
            interestAreaTriangle[2].lat, interestAreaTriangle[2].lng)).ToList();
    }

    private static bool IsPointInTriangle(double pointLat, double pointLon,
        double trianglePoint1Lat, double trianglePoint1Lon,
        double trianglePoint2Lat, double trianglePoint2Lon,
        double trianglePoint3Lat, double trianglePoint3Lon)
    {
        // Create a ray from the point in question to a point outside the triangle
        var rayLat = pointLat - 100;
        var rayLon = pointLon - 100;

        // Count the number of times the ray intersects with the triangle's sides
        var intersections = 0;
        if (RayIntersectsSegment(pointLat, pointLon, rayLat, rayLon, trianglePoint1Lat, trianglePoint1Lon,
                trianglePoint2Lat, trianglePoint2Lon))
            intersections++;
        if (RayIntersectsSegment(pointLat, pointLon, rayLat, rayLon, trianglePoint2Lat, trianglePoint2Lon,
                trianglePoint3Lat, trianglePoint3Lon))
            intersections++;
        if (RayIntersectsSegment(pointLat, pointLon, rayLat, rayLon, trianglePoint3Lat, trianglePoint3Lon,
                trianglePoint1Lat, trianglePoint1Lon))
            intersections++;

        // If the number of intersections is odd, the point is inside the triangle
        return (intersections % 2 == 1);
    }

    private static bool RayIntersectsSegment(double pointLat, double pointLon,
        double rayLat, double rayLon,
        double segmentPoint1Lat, double segmentPoint1Lon,
        double segmentPoint2Lat, double segmentPoint2Lon)
    {
        // Compute the cross product of the ray and the segment
        var segmentLat = segmentPoint2Lat - segmentPoint1Lat;
        var segmentLon = segmentPoint2Lon - segmentPoint1Lon;
        var rayToPointLat = pointLat - rayLat;
        var rayToPointLon = pointLon - rayLon;
        var crossProduct = rayToPointLat * segmentLon - rayToPointLon * segmentLat;

        // If the cross product is positive, the point is on the left side of the ray,
        // and the ray does not intersect the segment
        if (crossProduct >= 0)
            return false;

        // Compute the dot product of the segment and the segment
        var dotProduct = rayToPointLat * segmentLat + rayToPointLon * segmentLon;

        // If the dot product is negative, the point is behind the ray,
        // and the ray does not intersect the segment
        if (dotProduct < 0)
            return false;

        // Compute the dot product of the segment and the segment
        var segmentDotProduct = segmentLat * segmentLat + segmentLon * segmentLon;

        // If the dot product of the point and the ray is greater than the dot product
        // of the segment and the segment, the point is beyond the end of the segment,
        // and the ray does not intersect the segment
        if (dotProduct > segmentDotProduct)
            return false;

        // Otherwise, the point is between the start and end of the segment,
        // and the ray intersects the segment
        return true;
    }
    
    /* A utility function to calculate area of triangle
formed by (x1, y1) (x2, y2) and (x3, y3) */
    static double area(double x1, double y1, double x2,
        double y2, double x3, double y3)
    {
        return Math.Abs((x1 * (y2 - y3) +
                         x2 * (y3 - y1) +
                         x3 * (y1 - y2)) / 2.0);
    }
 
    /* A function to check whether point P(x, y) lies
    inside the triangle formed by A(x1, y1),
    B(x2, y2) and C(x3, y3) */
    static bool isInside(double x1, double y1, double x2,
        double y2, double x3, double y3,
        double x, double y)
    {
        /* Calculate area of triangle ABC */
        double A = area(x1, y1, x2, y2, x3, y3);
 
        /* Calculate area of triangle PBC */
        double A1 = area(x, y, x2, y2, x3, y3);
 
        /* Calculate area of triangle PAC */
        double A2 = area(x1, y1, x, y, x3, y3);
 
        /* Calculate area of triangle PAB */
        double A3 = area(x1, y1, x2, y2, x, y);
 
        /* Check if sum of A1, A2 and A3 is same as A */
        double area2 = A1 + A2 + A3;
        var diff = Math.Abs(A - area2);

        var rounded = Math.Round(diff, 10);
        return rounded == 0;
    }
}