using FlightListener.Model;
using FlightListener.OpenSkyNetwork;
using NAudio.Wave;

namespace FlightListener;

public interface IPlaneSpotter
{
    public void NotifyViewablePlane(IList<Aircraft> planes);
}

public class AircraftConsoleWriter : IPlaneSpotter
{
    private Dictionary<string, Aircraft> _currentStates = new();

    public void NotifyViewablePlane(IList<Aircraft> planes)
    {
        var added = false;
        var removed = false;
        
        foreach (var plane in planes)
        {
            if (!_currentStates.ContainsKey(plane.Callsign))
            {
                _currentStates.Add(plane.Callsign, plane);
                added = true;
            }
        }

        List<string> toRemove = new();
        
        foreach (var callSign in _currentStates.Keys.Where(callSign => planes.All(plane => plane.Callsign != callSign)))
        {
            toRemove.Add(callSign);
            removed = true;
        }

        foreach (var callsignToRemove in toRemove)
        {
            _currentStates.Remove(callsignToRemove);
        }

        if (added)
        {
            // play ding
            using var audioFile = new Mp3FileReader(@"C:\Users\marti\Desktop\Fasten seatbelt.mp3");
            using var outputDevice = new WaveOutEvent();

            outputDevice.Init(audioFile);
            outputDevice.Play();
            
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(1000);
            }
        }

        if (added || removed)
        {
            // Print state
            Console.WriteLine("Oh there are planes on the sky!");
        
            foreach (var plane in _currentStates.Values)
            {
                Console.WriteLine("=== AIRCRAFT ===");
                Console.WriteLine($"LAT: {plane.Lat}");
                Console.WriteLine($"LNG: {plane.Lng}");
                Console.WriteLine($"SGN: {plane.Callsign}");
            }
        }
    }
}