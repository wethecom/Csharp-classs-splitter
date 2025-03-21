
using System;

namespace PackageManagerServer
{
    public class foreachClass
    {
        

                foreach (string part in parts)
                {
                    if (string.IsNullOrEmpty(part) || part == "--\r\n" || part == "--")
                        continue;

                    // Find the headers section
                    int headerEnd = part.IndexOf("\r\n\r\n");
                    if (headerEnd < 0)
                        continue;

                    string headers = part.Substring(0, headerEnd);

                    // Extract name from headers
                    int nameStart = headers.IndexOf("name=\"") + 6;
                    if (nameStart < 6)
                        continue;

                    int nameEnd = headers.IndexOf("\"", nameStart);
                    if (nameEnd < 0)
                        continue;

                    string name = headers.Substring(nameStart, nameEnd - nameStart);

                    // Check if this is a file
                    bool isFile = headers.Contains("filename=\"");
                    string filename = "";

                    if (isFile)
                    {
                        int filenameStart = headers.IndexOf("filename=\"") + 10;
                        if (filenameStart >= 10)
                        {
                            int filenameEnd = headers.IndexOf("\"", filenameStart);
                            if (filenameEnd > 0)
                            {
                                filename = headers.Substring(filenameStart, filenameEnd - filenameStart);
                            }
                        }
    }
}
