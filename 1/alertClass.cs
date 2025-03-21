
using System;

namespace PackageManagerServer
{
    public class alertClass
    {
        
                    alert('Error: ' + data.message);
                }
            })
            .catch(error => console.error('Error:', error));
        }
    </script>
</body>
</html>
";

            byte[] buffer = Encoding.UTF8.GetBytes(html);
            context.Response.ContentType = "text/html";
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.Close();
        }

        private static async Task SearchPackagesAsync(HttpListenerContext context)
        {
            string query = "";
            if (context.Request.QueryString["q"] != null)
            {
                query = context.Request.QueryString["q"].ToLower();
            }
    }
}
