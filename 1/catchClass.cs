
using System;

namespace PackageManagerServer
{
    public class catchClass
    {
        

                await WriteJsonResponseAsync(context, new
                {
                    success = true,
                    message = $"Chunk {chunkData.chunkIndex + 1}/{chunkData.totalChunks} received"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in chunk upload: {ex.Message}");
                context.Response.StatusCode = 500;
                await WriteJsonResponseAsync(context, new { success = false, message = $"Server error: {ex.Message}" });
            }
        }

        private static async Task CombineChunksAsync(string packageName, string version, string description, int totalChunks, string tempDir)
        {
            try
            {
                Console.WriteLine($"Combining {totalChunks} chunks for {packageName} v{version}");

                // Create package directory if it doesn't exist
                string pkgDir = Path.Combine(PACKAGES_DIR, packageName);
                if (!Directory.Exists(pkgDir))
                {
                    Directory.CreateDirectory(pkgDir);
                }
    }
}
