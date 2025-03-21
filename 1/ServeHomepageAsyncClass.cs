
using System;

namespace PackageManagerServer
{
    public class ServeHomepageAsyncClass
    {
        


        private static async Task ServeHomepageAsync(HttpListenerContext context)
        {
            string html = @"
<!DOCTYPE html>
<html>
<head>
    <title>Package Manager</title>
    <style>
        body { font-family: Arial, sans-serif; max-width: 800px; margin: 0 auto; padding: 20px; }
        h1 { color: #333; }
        .search-container { margin: 20px 0; }
        input[type=""text""] { padding: 8px; width: 70%; }
        button { padding: 8px 15px; background: #4CAF50; color: white; border: none; cursor: pointer; }
        .package { border: 1px solid #ddd; padding: 15px; margin: 10px 0; border-radius: 5px; }
        .package h3 { margin-top: 0; }
        .upload-form { margin: 30px 0; padding: 15px; background: #f5f5f5; border-radius: 5px; }
    </style>
</head>
<body>
    <h1>Package Manager</h1>
    
    <div class=""search-container"">
        <input type=""text"" id=""searchInput"" placeholder=""Search packages..."">
        <button onclick=""searchPackages()"">Search</button>
    </div>
    
    <div id=""results""></div>
    
    <div class=""upload-form"">
        <h2>Upload New Package</h2>
        <form id=""uploadForm"" enctype=""multipart/form-data"">
            <div>
                <label for=""packageName"">Package Name:</label>
                <input type=""text"" id=""packageName"" name=""packageName"" required>
            </div>
            <div>
                <label for=""version"">Version:</label>
                <input type=""text"" id=""version"" name=""version"" required>
            </div>
            <div>
                <label for=""description"">Description:</label>
                <textarea id=""description"" name=""description"" rows=""4"" cols=""50"" required></textarea>
            </div>
            <div>
                <label for=""packageFile"">Package File:</label>
                <input type=""file"" id=""packageFile"" name=""packageFile"" required>
            </div>
            <button type=""button"" onclick=""uploadPackage()"">Upload</button>
        </form>
    </div>
    
    <script>
        function searchPackages() {
            const query = document.getElementById('searchInput').value;
            fetch(`/api/search?q=${query}`)
                .then(response => response.json())
                .then(data => {
                    const resultsDiv = document.getElementById('results');
                    resultsDiv.innerHTML = '';
                    
                    if (data.length === 0) {
                        resultsDiv.innerHTML = '<p>No packages found.</p>';
                        return;
                    }
    }
}
