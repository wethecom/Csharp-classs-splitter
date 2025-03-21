
using System;

namespace PackageManagerServer
{
    public class SaveDatabaseClass
    {
         Save database immediately
                SaveDatabase();

                Console.WriteLine($"Database updated for {name} {version}");
                await WriteJsonResponseAsync(context, new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in upload: {ex.Message}");
                context.Response.StatusCode = 500;
                await WriteJsonResponseAsync(context, new { success = false, message = $"Server error: {ex.Message}" });
            }
        }

        private static async Task UploadPackageJsonAsync(HttpListenerContext context)
        {
            Console.WriteLine("JSON Upload request received");

            if (context.Request.ContentType != "application/json")
            {
                context.Response.StatusCode = 400;
                await WriteJsonResponseAsync(context, new { success = false, message = "Expected application/json content type" });
                return;
            }
    }
}
