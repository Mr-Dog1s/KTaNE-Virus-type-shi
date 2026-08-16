using System.Data;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;

bool ShutdownHandshake = false;

bool DeadMansSwitchArmed = false;

DateTime lastHeartbeat = DateTime.UtcNow;


//--------------------------------------------


Console.WriteLine("WATCHDOG STARTED\n");

if (args.Length == 0)
{
    Console.WriteLine("NO PID RECEIVED\n");
    Console.ReadLine();
    return;
}

if (!int.TryParse(args[0], out int bombPid))
{
    Console.WriteLine("INVALID PID\n");
    Console.ReadLine();
    return;
}

Console.WriteLine($"PID received: {bombPid}\n");


//--------------------------------------------


Process bomb;

try
{
    bomb = Process.GetProcessById(bombPid);
}
catch (Exception ex)
{
    Console.WriteLine($"FAILED TO FIND BOMB: {ex.Message}\n");
    Console.ReadLine();
    return;
}

Console.WriteLine($"Found Bomb: {bomb.ProcessName}\n");
Console.WriteLine("Creating pipe...\n");


//--------------------------------------------


using var pipe = new NamedPipeServerStream(
    "KTaNE_Bomb",
    PipeDirection.In
);

Console.WriteLine("Pipe created.\n");
Console.WriteLine("Waiting for Bomb connection...\n");

pipe.WaitForConnection();

Console.WriteLine("BOMB CONNECTED!\n");

using var watchdogPipe = new NamedPipeServerStream(
    "KTaNE_Watchdog",
    PipeDirection.Out
);

Console.WriteLine("Waiting for Bomb watchdog connection...");

watchdogPipe.WaitForConnection();

Console.WriteLine("Watchdog output pipe connected!");


//--------------------------------------------


Task processMonitor = Task.Run(() =>
{
    bomb.WaitForExit();

    Console.WriteLine("Bomb process terminated\n");
});


//--------------------------------------------


using StreamReader reader = new StreamReader(pipe);

Task ECGListener = Task.Run(async () =>
{
    while (!ShutdownHandshake)
    {
        string? message = await reader.ReadLineAsync();

        if (message == null)
        {
            Console.WriteLine("BOMB CONNECTION TERMINATED");
            break;
        }

        if(message == "DMS_Armed")
        {
            Console.WriteLine("DMS ARMED, CONFIRMED");
            DeadMansSwitchArmed = true;
        }

        Console.WriteLine($"RECEIVED: {message}");

        if (message == "NOMINAL")
        {
            lastHeartbeat = DateTime.UtcNow;
        }
        else if (message == "SHUTDOWN_APPROVED")
        {
            ShutdownHandshake = true;
            Console.WriteLine("SHUTDOWN HANDSHAKE RECEIVED");
        }
    }
});


//--------------------------------------------


Task watchDogMonitor = Task.Run(async () => 
{
    while (!ShutdownHandshake)
    {
        await Task.Delay(1000);

        TimeSpan sinceHeartBeat =
            DateTime.UtcNow - lastHeartbeat;

        Console.WriteLine(
            $"Heartbeat age: {sinceHeartBeat.TotalSeconds:F1}s"
        );

        if (sinceHeartBeat.TotalSeconds > 3)
        {
            Console.WriteLine("ARRHYTMIA DETECTED\n");
        }

        if (bomb.HasExited)
        {
            break;
        }
    }
});


//--------------------------------------------


Task watchdogHeartbeat = Task.Run(async () =>
{
    using StreamWriter writer = new StreamWriter(watchdogPipe)
    {
        AutoFlush = true
    };

    while (!ShutdownHandshake)
    {
        try
        {
            Console.WriteLine("Sending WATCHDOG_OPERATIONAL...");

            await writer.WriteLineAsync("WATCHDOG_OPERATIONAL");

            Console.WriteLine("WATCHDOG_OPERATIONAL sent.");

            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"WATCHDOG HEARTBEAT FAILED: {ex}"
            );

            break;
        }
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
    Thread.Sleep(2000);
    Console.WriteLine($"DMS Arming Status: {DeadMansSwitchArmed}");
    if (DeadMansSwitchArmed)
    {
        Console.WriteLine("DEAD-MAN-SWITCH ACTIVATED");
        Thread.Sleep(2000);
        Console.WriteLine("TERMINATING USER");
    }
    
    
    
}


//--------------------------------------------


Console.WriteLine("Loop terminated");
Console.WriteLine($"Clean shutdown: {ShutdownHandshake}");
Thread.Sleep(5000);
Console.WriteLine("SLEEP COMPLETE");
Console.WriteLine("WATCHDOG EXITING");