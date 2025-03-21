
using System;

namespace PackageManagerServer
{
    public class ifClass
    {
         Validate required fields
                if (string.IsNullOrEmpty(uploadData.name) ||
                    string.IsNullOrEmpty(uploadData.version) ||
                    string.IsNullOrEmpty(uploadData.description) ||
                    string.IsNullOrEmpty(uploadData.fileData))
                {
                    context.Response.StatusCode = 400;
                    await WriteJsonResponseAsync(context, new
                    {
                        success = false,
                        message = $"Missing required fields: name={!string.IsNullOrEmpty(uploadData.name)}, " +
                                  $"version={!string.IsNullOrEmpty(uploadData.version)}, " +
                                  $"description={!string.IsNullOrEmpty(uploadData.description)}, " +
                                  $"fileData={!string.IsNullOrEmpty(uploadData.fileData)}"
                    });
                    return;
                }

                // Decode the base64 file data
                byte[] fileData;
                try
                {
                    fileData = Convert.FromBase64String(uploadData.fileData);
                    Console.WriteLine($"Decoded file data, size: {fileData.Length} bytes");
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 400;
                    await WriteJsonResponseAsync(context, new { success = false, message = $"Invalid base64 file data: {ex.Message}" });
                    return;
                }

                // Create package directory if it doesn't exist
                string pkgDir = Path.Combine(PACKAGES_DIR, uploadData.name);
                if (!Directory.Exists(pkgDir))
                {
                    Directory.CreateDirectory(pkgDir);
                }
    }
}
