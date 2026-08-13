using System.Data;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;

bool ShutdownHandshake = false;

DateTime lastHeartbeat = DateTime.UtcNow;


//--------------------------------------------


Console.WriteLine("WATCHDOG STARTED");

if (args.Length == 0)
{
    Console.WriteLine("NO PID RECEIVED");
    Console.ReadLine();
    return;
}

if (!int.TryParse(args[0], out int bombPid))
{
    Console.WriteLine("INVALID PID");
    Console.ReadLine();
    return;
}

Console.WriteLine($"PID received: {bombPid}");


//--------------------------------------------


Process bomb;

try
{
    bomb = Process.GetProcessById(bombPid);
}
catch (Exception ex)
{
    Console.WriteLine($"FAILED TO FIND BOMB: {ex.Message}");
    Console.ReadLine();
    return;
}

Console.WriteLine($"Found Bomb: {bomb.ProcessName}");
Console.WriteLine("Creating pipe...");


//--------------------------------------------


using var pipe = new NamedPipeServerStream(
    "KTaNE_Bomb",
    PipeDirection.In
);

Console.WriteLine("Pipe created.");
Console.WriteLine("Waiting for Bomb connection...");

pipe.WaitForConnection();

Console.WriteLine("BOMB CONNECTED!");


//--------------------------------------------


Task processMonitor = Task.Run(() =>
{
    bomb.WaitForExit();

    Console.WriteLine("Bomb process terminated");
});


//--------------------------------------------


Task ECGListener = Task.Run(() =>
{
    byte[] buffer = new byte[1024];

    while (true)
    {
        int bytesRead = pipe.Read
        (
            buffer,
            0,
            buffer.Length
        );

        if( bytesRead == 0)
        {
            Console.WriteLine("Pipe connection terminated");
            break;
        }

        string message = Encoding.UTF8.GetString
        (
            buffer, 
            0, 
            bytesRead
        );

        Console.WriteLine(message);

        if(message == "NOMINAL")
        {
            lastHeartbeat = DateTime.UtcNow;
        }
        else if (message == "SHUTDOWN_APPROVED")
        {
            ShutdownHandshake = true;
            Console.WriteLine("Shutdown handshake received");
        }
    }
});


//--------------------------------------------


Task watchDogMonitor = Task.Run(async () => 
{ 
    await Task.Delay(1000);

    TimeSpan sinceHeartBeat = DateTime.UtcNow - lastHeartbeat;

    if(sinceHeartBeat.TotalSeconds > 3)
    {
        Console.WriteLine("ARRHYTMIA DETECTED");
    }
});


//--------------------------------------------


await processMonitor;


if (ShutdownHandshake)
{
    Console.WriteLine("System approved shutdown");
}
else
{
    Console.WriteLine("Unauthorized termination");
}


//--------------------------------------------


Console.WriteLine("Loop terminated");
Console.WriteLine($"Clean shutdown: {ShutdownHandshake}");
Console.ReadLine();