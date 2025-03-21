
using System;

namespace PackageManagerServer
{
    public class HttpListenerClass
    {
         new HttpListener();
            listener.Prefixes.Add($"http://localhost:{PORT}/");
            listener.Start();
            Console.WriteLine($"Server started at http://localhost:{PORT}/");
            Console.WriteLine("Press Ctrl+C to stop the server");

            // Set up console cancellation
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                isRunning = false;
                Console.WriteLine("Shutting down server...");
            };

            // Handle requests
            while (isRunning)
            {
                try
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    _ = ProcessRequestAsync(context);
                }
                catch (Exception ex)
                {
                    if (isRunning) // Only log if not shutting down
                    {
                        Console.WriteLine($"Error handling request: {ex.Message}");
                    }
                }
            }

            // Clean up
            listener.Stop();
            Console.WriteLine("Server stopped");
        }

        private static void InitializeDatabase()
        {
            if (File.Exists(DB_FILE))
            {
                string json = File.ReadAllText(DB_FILE);
                packageDb = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, PackageVersionInfo>>>(json);
                Console.WriteLine($"Loaded package database with {packageDb.Count} packages");
            }
    }
}
