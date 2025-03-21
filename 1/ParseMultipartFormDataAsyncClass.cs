
using System;

namespace PackageManagerServer
{
    public class ParseMultipartFormDataAsyncClass
    {
         await ParseMultipartFormDataAsync(context.Request);

            // Get form fields
            string name = formData.ContainsKey("name") ? formData["name"].ToString() : null;
            string version = formData.ContainsKey("version") ? formData["version"].ToString() : null;
            string description = formData.ContainsKey("description") ? formData["description"].ToString() : null;
            byte[] fileData = formData.ContainsKey("file") ? (byte[])formData["file"] : null;

            Console.WriteLine($"Processing upload: {name}, {version}, {description}, File size: {(fileData?.Length ?? 0)} bytes");

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(version) ||
                string.IsNullOrEmpty(description) || fileData == null || fileData.Length == 0)
            {
                context.Response.StatusCode = 400;
                await WriteJsonResponseAsync(context, new
                {
                    success = false,
                    message = $"Missing required fields: name={!string.IsNullOrEmpty(name)}, version={!string.IsNullOrEmpty(version)}, description={!string.IsNullOrEmpty(description)}, file={fileData != null && fileData.Length > 0}"
                });
                return;
            }

            try
            {
                // Create package directory if it doesn't exist
                string pkgDir = Path.Combine(PACKAGES_DIR, name);
                if (!Directory.Exists(pkgDir))
                {
                    Directory.CreateDirectory(pkgDir);
                }
    }
}
