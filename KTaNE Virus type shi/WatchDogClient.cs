using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Pipes;
using System.Security.Permissions;
using System.Diagnostics;

namespace KTaNE_Virus_type_shi
{
    internal class WatchDogClient
    {
        private NamedPipeClientStream? pipe;

        private CancellationTokenSource? heartBeatCts;
            
        public async Task ConnectAsync()
        {

            pipe = new NamedPipeClientStream
                (
                    ".",
                    "KTaNE_bomb",
                    PipeDirection.Out
                );
            try
            {
                Debug.WriteLine("Connecting to watchdog");

                await pipe.ConnectAsync(5000);

                Debug.WriteLine("Connected to WatchDog");
            }
            catch(Exception ex) 
            {
                Debug.WriteLine("WatchDog connection timeout");

                pipe.Dispose();
                pipe = null;
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if(pipe == null || !pipe.IsConnected)
            {
                Debug.WriteLine("Connection not established");
                return;
            }

            byte[] data = Encoding.UTF8.GetBytes(message);

            await pipe.WriteAsync(data);
            await pipe.FlushAsync();

            Debug.WriteLine($"Sent to watchdog: {message}");
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
                        await SendMessageAsync("NOMINAL");

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

        public void FlatLine()
        {
            heartBeatCts?.Cancel();
        }

        
    }
}
