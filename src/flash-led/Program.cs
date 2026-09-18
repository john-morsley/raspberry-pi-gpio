using System;
using System.Device.Gpio;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

// Simple LED flasher for Raspberry Pi using System.Device.Gpio
// Usage: dotnet run -- [pin] [milliseconds]
// Example: dotnet run -- 18 500

int pin = 18;
int intervalMs = 500;

if (args.Length >= 1 && int.TryParse(args[0], out var p))
{
    pin = p;
}

if (args.Length >= 2 && int.TryParse(args[1], out var ms))
{
    intervalMs = ms;
}

if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    Console.WriteLine("Warning: GPIO typically requires Linux (Raspberry Pi). Exiting.");
    return;
}

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => { e.Cancel = true; cts.Cancel(); };

try
{
    using var controller = new GpioController(PinNumberingScheme.Logical);
    controller.OpenPin(pin, PinMode.Output);

    Console.WriteLine($"Flashing GPIO pin {pin} every {intervalMs}ms. Press Ctrl+C to stop.");

    // Ensure LED is off initially
    controller.Write(pin, PinValue.Low);

    while (!cts.Token.IsCancellationRequested)
    {
        controller.Write(pin, PinValue.High);
        await Task.Delay(intervalMs, cts.Token);
        controller.Write(pin, PinValue.Low);
        await Task.Delay(intervalMs, cts.Token);
    }

    controller.Write(pin, PinValue.Low);
}
catch (OperationCanceledException)
{
    // graceful shutdown
}
catch (PlatformNotSupportedException ex)
{
    Console.WriteLine($"Platform not supported: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine("Stopped.");
