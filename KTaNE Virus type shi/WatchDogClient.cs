using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Pipes;
using System.Security.Permissions;
using System.Diagnostics;
using System.Security.Policy;

namespace KTaNE_Virus_type_shi
{
    internal class WatchDogClient
    {

        private CancellationTokenSource? heartBeatCts;

        private DateTime lastWatchdogHeartbeat = DateTime.UtcNow;

        private NamedPipeClientStream? sendPipe;
        private NamedPipeClientStream? receivePipe;

        private StreamWriter? writer;
        private StreamReader? reader;


        public async Task ConnectAsync()
        {
            sendPipe = new NamedPipeClientStream(
                ".",
                "KTaNE_Bomb",
                PipeDirection.Out
            );

            receivePipe = new NamedPipeClientStream(
                ".",
                "KTaNE_Watchdog",
                PipeDirection.In
            );

            try
            {
                Debug.WriteLine("Connecting to watchdog...");

                await sendPipe.ConnectAsync(5000);
                Debug.WriteLine("Bomb → Watchdog connected.");

                await receivePipe.ConnectAsync(5000);
                Debug.WriteLine("Watchdog → Bomb connected.");

                writer = new StreamWriter(sendPipe)
                {
                    AutoFlush = true
                };

                reader = new StreamReader(receivePipe);

                Debug.WriteLine("Both watchdog pipes connected.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Watchdog connection failed: {ex}");

                sendPipe?.Dispose();
                receivePipe?.Dispose();

                sendPipe = null;
                receivePipe = null;
            }
        }


        public async Task SendMessageAsync(string message)
        {
            if (writer == null)
            {
                Debug.WriteLine("Watchdog connection not established.");
                return;
            }

            try
            {
                await writer.WriteLineAsync(message);

                Debug.WriteLine(
                    $"Sent to watchdog: {message}"
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    $"WATCHDOG SEND FAILED: {ex}"
                );
            }
        }


        public async Task DeadMansSwitchSafety(bool DMSSafetySwitch)
        {

                try
                {
                    string message = "DMS_Armed";

                    if (DMSSafetySwitch)
                    {
                        await SendMessageAsync(message);
                        Console.WriteLine("DMS-ARMED");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"DMS-Armament failed: {ex}");
                }
        }

        public void HeartBeat()
        {
            heartBeatCts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                while (!heartBeatCts.Token.IsCancellationRequested)
                {
                    try
                    {
                        string message = "NOMINAL\n";

                        await SendMessageAsync(message);

                        await Task.Delay(1000, heartBeatCts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch(Exception ex )
                    {
                        Debug.WriteLine($"Arrhytmia detected: {ex.Message}");
                    }
                }
            });
        }


        public async Task FlatLine()
        {
            try
            {
                await SendMessageAsync("SHUTDOWN_APPROVED");

                heartBeatCts?.Cancel();
            }
            catch( Exception ex )
            {
                Debug.WriteLine($"SHUTDOWN EXCEPTION: {ex.Message}");
            }
        }


        public async Task ListenAsync()
        {
            if (reader == null)
                return;

            try
            {
                while (true)
                {
                    string? message = await reader.ReadLineAsync();

                    if (message == null)
                    {
                        Debug.WriteLine("WATCHDOG_LOST");
                        break;
                    }

                    if (message == "WATCHDOG_OPERATIONAL")
                    {
                        lastWatchdogHeartbeat = DateTime.UtcNow;

                        Debug.WriteLine(
                            $"WATCHDOG OPERATIONAL received at " +
                            $"{lastWatchdogHeartbeat:HH:mm:ss.fff}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    $"WATCHDOG CONNECTION LOST: {ex}"
                );
            }
        }

        public void WatchdogECG()
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    TimeSpan sinceWatchDogResponse =
                        DateTime.UtcNow - lastWatchdogHeartbeat;
                    if (sinceWatchDogResponse.TotalSeconds > 3)
                    {
                        Debug.WriteLine("WATCHDOG ARRHYTMIA DETECTED");
                    }
                }
            });
        }

        
    }
}
