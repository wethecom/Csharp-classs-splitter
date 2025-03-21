
using System;

namespace PackageManagerServer
{
    public class WriteJsonResponseAsyncClass
    {
        

                await WriteJsonResponseAsync(context, new { success = true });
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON parsing error: {ex.Message}");
                context.Response.StatusCode = 400;
                await WriteJsonResponseAsync(context, new { success = false, message = $"Invalid JSON format: {ex.Message}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in upload: {ex.Message}");
                context.Response.StatusCode = 500;
                await WriteJsonResponseAsync(context, new { success = false, message = $"Server error: {ex.Message}" });
            }
        }

        private static async Task<Dictionary<string, object>> ParseMultipartFormDataAsync(HttpListenerRequest request)
        {
            var formData = new Dictionary<string, object>();

            try
            {
                // Get the boundary from the content type
                string boundary = GetBoundary(request.ContentType);
                if (string.IsNullOrEmpty(boundary))
                {
                    Console.WriteLine("Could not find boundary in content type");
                    return formData;
                }
    }
}
