using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PackageManagerServer
{
    class Program
    {
        private const string PACKAGES_DIR = "packages";
        private const string DB_FILE = "package_db.json";
        private const int PORT = 5005;
        private static Dictionary<string, Dictionary<string, PackageVersionInfo>> packageDb;
        private static HttpListener listener;
        private static bool isRunning = true;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Package Manager Server");
            Console.WriteLine("======================");

            // Create packages directory if it doesn't exist
            if (!Directory.Exists(PACKAGES_DIR))
            {
                Directory.CreateDirectory(PACKAGES_DIR);
                Console.WriteLine($"Created packages directory: {PACKAGES_DIR}");
            }

            // Initialize package database
            InitializeDatabase();

            // Add sample packages if database is empty
            if (packageDb.Count == 0)
            {
                AddSamplePackages();
            }

            // Start HTTP server
            listener =
    public new HttpListener()
    {
        return new HttpListenerClass().HttpListener();
    }
            else
            {
                packageDb = new Dictionary<string, Dictionary<string, PackageVersionInfo>>();
                SaveDatabase();
                Console.WriteLine("Created new package database");
            }
        }

        private static void SaveDatabase()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(packageDb, options);
            File.WriteAllText(DB_FILE, json);
        }

        private static void AddSamplePackages()
        {
            var samplePackages = new List<(string name, string version, string description)>
            {
                ("sample-lib", "1.0.0", "A sample library for demonstration purposes"),
                ("data-utils", "2.1.3", "Utilities for data processing and analysis"),
                ("web-framework", "0.9.5", "Lightweight web framework for applications")
            };

            foreach (var pkg in samplePackages)
            {
                // Create package directory
                string pkgDir = Path.Combine(PACKAGES_DIR, pkg.name);
                if (!Directory.Exists(pkgDir))
                {
                    Directory.CreateDirectory(pkgDir);
                }

                // Create a simple zip file
                string today = DateTime.Now.ToString("yyyyMMdd");
                string zipFilename = $"{pkg.name}-{pkg.version}-{today}.zip";
                string zipPath = Path.Combine(pkgDir, zipFilename);

                // Create a simple zip with a README
                using (var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                {
                    var readmeEntry = zipArchive.CreateEntry("README.md");
                    using (var writer = new StreamWriter(readmeEntry.Open()))
                    {
                        writer.WriteLine($"# {pkg.name} v{pkg.version}");
                        writer.WriteLine();
                        writer.WriteLine(pkg.description);
                    }
                }

                // Update database
                if (!packageDb.ContainsKey(pkg.name))
                {
                    packageDb[pkg.name] = new Dictionary<string, PackageVersionInfo>();
                }

                packageDb[pkg.name][pkg.version] = new PackageVersionInfo
                {
                    Description = pkg.description,
                    FilePath = zipPath,
                    UploadDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
            }

            SaveDatabase();
            Console.WriteLine("Added sample packages to the database");
        }

        private static async Task ProcessRequestAsync(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            string method = context.Request.HttpMethod;

            Console.WriteLine($"Request: {method} {path}");

            try
            {
                if (path == "/" && method == "GET")
                {
                    await ServeHomepageAsync(context);
                }
                else if (path.StartsWith("/api/search") && method == "GET")
                {
                    await SearchPackagesAsync(context);
                }
                else if (path.StartsWith("/api/details/") && method == "GET")
                {
                    await GetPackageDetailsAsync(context);
                }
                else if (path.StartsWith("/api/download/") && method == "GET")
                {
                    await DownloadPackageAsync(context);
                }
                else if (path == "/api/upload" && method == "POST")
                {
                    await UploadPackageAsync(context);
                }
                else if (path == "/api/upload-json" && method == "POST")
                {
                    await UploadPackageJsonAsync(context);
                }
                else if (path == "/api/upload-chunk" && method == "POST")
                {
                    await UploadChunkAsync(context);
                }
                else
                {
                    // Not found
                    context.Response.StatusCode = 404;
                    await WriteJsonResponseAsync(context, new { error = "Not found" });
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request: {ex.Message}");
                context.Response.StatusCode = 500;
                await WriteJsonResponseAsync(context, new { error = "Internal server error", message = ex.Message });
            }
        }

        private static async Task UploadChunkAsync(HttpListenerContext context)
        {
            Console.WriteLine("Chunk upload request received");

            // Read the request body
            string requestBody;
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            try
            {
                // Parse the JSON
                var chunkData = JsonSerializer.Deserialize<ChunkUploadRequest>(requestBody);

                // Validate required fields
                if (string.IsNullOrEmpty(chunkData.name) ||
                    string.IsNullOrEmpty(chunkData.version) ||
                    string.IsNullOrEmpty(chunkData.chunkData) ||
                    chunkData.chunkIndex < 0 ||
                    chunkData.totalChunks <= 0)
                {
                    context.Response.StatusCode = 400;
                    await WriteJsonResponseAsync(context, new
                    {
                        success = false,
                        message = "Missing or invalid chunk data"
                    });
                    return;
                }

                // Create temp directory for chunks if it doesn't exist
                string tempDir = Path.Combine(PACKAGES_DIR, "temp", chunkData.name, chunkData.version);
                Directory.CreateDirectory(tempDir);

                // Save this chunk
                string chunkPath = Path.Combine(tempDir, $"chunk_{chunkData.chunkIndex}.bin");
                byte[] chunkBytes = Convert.FromBase64String(chunkData.chunkData);
                File.WriteAllBytes(chunkPath, chunkBytes);

                Console.WriteLine($"Saved chunk {chunkData.chunkIndex + 1}/{chunkData.totalChunks} for {chunkData.name} v{chunkData.version}");

                // Check if all chunks are received
                if (chunkData.chunkIndex == chunkData.totalChunks - 1)
                {
                    // All chunks received, combine them
                    await CombineChunksAsync(chunkData.name, chunkData.version, chunkData.description, chunkData.totalChunks, tempDir);
                }
    public  catch(Exception ex)
    {
        return new catchClass().catch(Exception, ex);
    }

                // Create zip file with version and date in the name
                string today = DateTime.Now.ToString("yyyyMMdd");
                string zipFilename = $"{packageName}-{version}-{today}.zip";
                string zipPath = Path.Combine(pkgDir, zipFilename);

                // Combine all chunks
                using (var outputStream = new FileStream(zipPath, FileMode.Create))
                {
                    for (int i = 0; i < totalChunks; i++)
                    {
                        string chunkPath = Path.Combine(tempDir, $"chunk_{i}.bin");
                        byte[] chunkData = File.ReadAllBytes(chunkPath);
                        await outputStream.WriteAsync(chunkData, 0, chunkData.Length);

                        // Delete the chunk after it's been added to the combined file
                        File.Delete(chunkPath);
                    }
                }

                // Update database
                if (!packageDb.ContainsKey(packageName))
                {
                    packageDb[packageName] = new Dictionary<string, PackageVersionInfo>();
                }

                packageDb[packageName][version] = new PackageVersionInfo
                {
                    Description = description,
                    FilePath = zipPath,
                    UploadDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                // Save database immediately
                SaveDatabase();

                Console.WriteLine($"Successfully combined chunks and added {packageName} v{version} to database");

                // Clean up temp directory
                Directory.Delete(tempDir, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error combining chunks: {ex.Message}");
                throw;
            }
        }
    public private static async Task ServeHomepageAsync(HttpListenerContext context)
    {
        return new ServeHomepageAsyncClass().ServeHomepageAsync(HttpListenerContext, context);
    }
                    
                    data.forEach(pkg => {
                        const pkgDiv = document.createElement('div');
                        pkgDiv.className = 'package';
                        pkgDiv.innerHTML = `
                            <h3>${pkg.name} (${pkg.version})</h3>
                            <p>${pkg.description}</p>
                            <p>Uploaded: ${pkg.upload_date}</p>
                            <button onclick=""downloadPackage('${pkg.name}', '${pkg.version}')"">Download</button>
                            <button onclick=""getDetails('${pkg.name}')"">Details</button>
                        `;
                        resultsDiv.appendChild(pkgDiv);
                    });
                })
                .catch(error => console.error('Error:', error));
        }
        
        function downloadPackage(name, version) {
            window.location.href = `/api/download/${name}/${version}`;
        }
        
        function getDetails(name) {
            fetch(`/api/details/${name}`)
                .then(response => response.json())
                .then(data => {
                    alert(JSON.stringify(data, null, 2));
                })
                .catch(error => console.error('Error:', error));
        }
    public function uploadPackage()
    {
        return new uploadPackageClass().uploadPackage();
    } else {
    public  alert('Error: ' + data.message)
    {
        return new alertClass().alert(Error, data, message);
    }

            var results = new List<PackageSearchResult>();

            foreach (var pkgEntry in packageDb)
            {
                string pkgName = pkgEntry.Key;
                var versions = pkgEntry.Value;

                if (query == "" || pkgName.ToLower().Contains(query) ||
                    versions.Any(v => v.Value.Description.ToLower().Contains(query)))
                {
                    foreach (var versionEntry in versions)
                    {
                        results.Add(new PackageSearchResult
                        {
                            Name = pkgName,
                            Version = versionEntry.Key,
                            Description = versionEntry.Value.Description,
                            UploadDate = versionEntry.Value.UploadDate
                        });
                    }
                }
            }

            await WriteJsonResponseAsync(context, results);
        }

        private static async Task GetPackageDetailsAsync(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            string packageName = path.Substring("/api/details/".Length);

            if (packageDb.ContainsKey(packageName))
            {
                await WriteJsonResponseAsync(context, packageDb[packageName]);
            }
            else
            {
                context.Response.StatusCode = 404;
                await WriteJsonResponseAsync(context, new { error = "Package not found" });
            }
        }

        private static async Task DownloadPackageAsync(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            string[] parts = path.Substring("/api/download/".Length).Split('/');

            if (parts.Length != 2)
            {
                context.Response.StatusCode = 400;
                await WriteJsonResponseAsync(context, new { error = "Invalid request format" });
                return;
            }

            // URL decode the package name
            string requestedPackageName = WebUtility.UrlDecode(parts[0]);
            string requestedVersion = WebUtility.UrlDecode(parts[1]);

            Console.WriteLine($"Download request for package: '{requestedPackageName}', version: '{requestedVersion}'");

            // Debug: List all packages in the database
            Console.WriteLine("Available packages in database:");
            foreach (var pkg in packageDb)
            {
                Console.WriteLine($"- {pkg.Key}");
                foreach (var ver in pkg.Value)
                {
                    Console.WriteLine($"  - {ver.Key} => {ver.Value.FilePath}");
                }
            }

            // Try to find a matching package name (case-insensitive and partial match)
            string matchedPackageName = null;

            // First try exact match (case-insensitive)
            matchedPackageName = packageDb.Keys
                .FirstOrDefault(k => string.Equals(k, requestedPackageName, StringComparison.OrdinalIgnoreCase));

            // If no exact match, try contains match
            if (matchedPackageName == null)
            {
                matchedPackageName = packageDb.Keys
                    .FirstOrDefault(k => k.IndexOf(requestedPackageName, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        requestedPackageName.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // If still no match, try word matching (e.g., "Rifle" would match "Pro Rifle Pack")
            if (matchedPackageName == null)
            {
                string[] requestedWords = requestedPackageName.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var pkgName in packageDb.Keys)
                {
                    string[] pkgWords = pkgName.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);

                    // Check if any word in the requested name matches any word in the package name
                    if (requestedWords.Any(rw => pkgWords.Any(pw => string.Equals(rw, pw, StringComparison.OrdinalIgnoreCase))))
                    {
                        matchedPackageName = pkgName;
                        break;
                    }
                }
            }
    public  if(matchedPackageName != null)
    {
        return new ifClass().if(matchedPackageName, null);
    }
                    else
                    {
                        Console.WriteLine($"File not found: {filePath}");
                        context.Response.StatusCode = 404;
    public await WriteJsonResponseAsync(context, new { error = "Package file not found on disk" })
    {
        return new WriteJsonResponseAsyncClass().WriteJsonResponseAsync(context, new, error, Package, file, not, found, on, disk);
    }
                    }

                    context.Response.StatusCode = 404;
    public await WriteJsonResponseAsync(context, new { error = "Version not found" })
    {
        return new WriteJsonResponseAsyncClass().WriteJsonResponseAsync(context, new, error, Version, not, found);
    }

            // Parse multipart form data
            var formData =
    public await ParseMultipartFormDataAsync(context.Request)
    {
        return new ParseMultipartFormDataAsyncClass().ParseMultipartFormDataAsync(context, Request);
    }

                // Create zip file with version and date in the name
                string today = DateTime.Now.ToString("yyyyMMdd");
                string zipFilename = $"{name}-{version}-{today}.zip";
                string zipPath = Path.Combine(pkgDir, zipFilename);

                // Save the uploaded file
                File.WriteAllBytes(zipPath, fileData);
                Console.WriteLine($"File saved to {zipPath}");

                // Update database
                if (!packageDb.ContainsKey(name))
                {
                    packageDb[name] = new Dictionary<string, PackageVersionInfo>();
                }

                packageDb[name][version] = new PackageVersionInfo
                {
                    Description = description,
                    FilePath = zipPath,
                    UploadDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                //
    public Save database immediately SaveDatabase()
    {
        return new SaveDatabaseClass().SaveDatabase();
    }

            // Read the request body
            string requestBody;
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            Console.WriteLine($"Received JSON data of length: {requestBody.Length}");

            try
            {
                // Parse the JSON
                var uploadData = JsonSerializer.Deserialize<UploadJsonRequest>(requestBody);

                //
    public Validate required fields if(string.IsNullOrEmpty(uploadData.name)
    {
        return new ifClass().if(string, IsNullOrEmpty, uploadData, name);
    }

                // Create zip file with version and date in the name
                string today = DateTime.Now.ToString("yyyyMMdd");
                string zipFilename = $"{uploadData.name}-{uploadData.version}-{today}.zip";
                string zipPath = Path.Combine(pkgDir, zipFilename);

                // Save the uploaded file
                File.WriteAllBytes(zipPath, fileData);
                Console.WriteLine($"File saved to {zipPath}");

                // Update database
                if (!packageDb.ContainsKey(uploadData.name))
                {
                    packageDb[uploadData.name] = new Dictionary<string, PackageVersionInfo>();
                }

                packageDb[uploadData.name][uploadData.version] = new PackageVersionInfo
                {
                    Description = uploadData.description,
                    FilePath = zipPath,
                    UploadDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                // Save database immediately
                SaveDatabase();

                Console.WriteLine($"Database updated for {uploadData.name} {uploadData.version}");

                // Debug: Show current database state
                Console.WriteLine("Current database state:");
                foreach (var pkg in packageDb)
                {
                    Console.WriteLine($"- {pkg.Key}");
                    foreach (var ver in pkg.Value)
                    {
                        Console.WriteLine($"  - {ver.Key} => {ver.Value.FilePath}");
                    }
                }
    public await WriteJsonResponseAsync(context, new { success = true })
    {
        return new WriteJsonResponseAsyncClass().WriteJsonResponseAsync(context, new, success, true);
    }

                boundary = "--" + boundary;

                // Read the entire request body
                byte[] requestBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await request.InputStream.CopyToAsync(memoryStream);
                    requestBytes = memoryStream.ToArray();
                }

                // Convert to string for header parsing
                string requestData = Encoding.UTF8.GetString(requestBytes);

                // Split by boundary
                string[] parts = requestData.Split(new[] { boundary }, StringSplitOptions.None);
    public  foreach(string part in parts)
    {
        return new foreachClass().foreach(string, part, in, parts);
    }
                    }

                    // Get content start position in bytes
                    int contentStartPos = Encoding.UTF8.GetByteCount(part.Substring(0, headerEnd + 4));

                    // Get content end position
                    int contentEndPos;
                    if (part.EndsWith("\r\n"))
                    {
                        contentEndPos = requestBytes.Length - Encoding.UTF8.GetByteCount("\r\n");
                    }
                    else
                    {
                        contentEndPos = requestBytes.Length;
                    }

                    // Extract content
                    if (isFile && !string.IsNullOrEmpty(filename))
                    {
                        // For files, extract binary data
                        int contentLength = part.Length - headerEnd - 4;
                        if (contentLength > 0)
                        {
                            byte[] fileData = new byte[contentLength];
                            Array.Copy(requestBytes, contentStartPos, fileData, 0, contentLength);
                            formData[name] = fileData;
                            Console.WriteLine($"Extracted file '{filename}' with {fileData.Length} bytes");
                        }
                    }
                    else
                    {
                        // For text fields
                        string content = part.Substring(headerEnd + 4);
                        if (content.EndsWith("\r\n"))
                        {
                            content = content.Substring(0, content.Length - 2);
                        }
                        formData[name] = content;
                        Console.WriteLine($"Extracted field '{name}' with value '{content}'");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing multipart form data: {ex.Message}");
            }

            return formData;
        }
        private static string GetBoundary(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return null;

            int index = contentType.IndexOf("boundary=");
            if (index < 0)
                return null;

            return contentType.Substring(index + 9);
        }

        private static async Task WriteJsonResponseAsync(HttpListenerContext context, object data)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.Close();
        }
    }

    public class PackageVersionInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("file_path")]
        public string FilePath { get; set; }

        [JsonPropertyName("upload_date")]
        public string UploadDate { get; set; }
    }

    public class PackageSearchResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("upload_date")]
        public string UploadDate { get; set; }
    }

    public class UploadJsonRequest
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("version")]
        public string version { get; set; }

        [JsonPropertyName("description")]
        public string description { get; set; }

        [JsonPropertyName("fileName")]
        public string fileName { get; set; }

        [JsonPropertyName("fileData")]
        public string fileData { get; set; }
    }

    // Add this class to your file
    public class ChunkUploadRequest
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("version")]
        public string version { get; set; }

        [JsonPropertyName("description")]
        public string description { get; set; }

        [JsonPropertyName("chunkIndex")]
        public int chunkIndex { get; set; }

        [JsonPropertyName("totalChunks")]
        public int totalChunks { get; set; }

        [JsonPropertyName("chunkData")]
        public string chunkData { get; set; }
    }
}
