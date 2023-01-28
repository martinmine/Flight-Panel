using FlightListener.Model;

namespace FlightListener;

public class MapFilter
{
    public IList<Aircraft> IsWithinInterestArea(IEnumerable<Aircraft> states)
    {
        // Coordinates that draws a triangle towards the Oslo fjord.
        (double lat, double lng)[] interestAreaTriangle =
        {
            (59.745788, 10.316217),
            (59.713850, 10.755098),            
            (59.969251, 10.759518),
        };
        
        return states.Where(state => IsInside(
            interestAreaTriangle[0].lat, interestAreaTriangle[0].lng,
            interestAreaTriangle[1].lat, interestAreaTriangle[1].lng,
            interestAreaTriangle[2].lat, interestAreaTriangle[2].lng, state.Lat, state.Lng)).ToList();
    }

    /// <summary>
    /// A utility function to calculate area of triangle formed by (x1, y1) (x2, y2) and (x3, y3)
    /// </summary>
    private double Area(double x1, double y1, double x2,
        double y2, double x3, double y3)
    {
        return Math.Abs((x1 * (y2 - y3) +
                         x2 * (y3 - y1) +
                         x3 * (y1 - y2)) / 2.0);
    }
    
    /// <summary>
    /// A function to check whether point P(x, y) lies inside the triangle formed by A(x1, y1), B(x2, y2) and C(x3, y3).
    /// Code from https://www.geeksforgeeks.org/check-whether-a-given-point-lies-inside-a-triangle-or-not/
    /// </summary>
    private bool IsInside(double x1, double y1, double x2,
        double y2, double x3, double y3,
        double x, double y)
    {
        /* Calculate area of triangle ABC */
        double A = Area(x1, y1, x2, y2, x3, y3);
 
        /* Calculate area of triangle PBC */
        double A1 = Area(x, y, x2, y2, x3, y3);
 
        /* Calculate area of triangle PAC */
        double A2 = Area(x1, y1, x, y, x3, y3);
 
        /* Calculate area of triangle PAB */
        double A3 = Area(x1, y1, x2, y2, x, y);
 
        /* Check if sum of A1, A2 and A3 is same as A */
        double area2 = A1 + A2 + A3;
        var diff = Math.Abs(A - area2);

        var rounded = Math.Round(diff, 10);
        return rounded == 0;
    }
}