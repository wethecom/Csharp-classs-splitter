
using System;

namespace PackageManagerServer
{
    public class uploadPackageClass
    {
        
        
        function uploadPackage() {
            const form = document.getElementById('uploadForm');
            const formData = new FormData();
            
            formData.append('name', document.getElementById('packageName').value);
            formData.append('version', document.getElementById('version').value);
            formData.append('description', document.getElementById('description').value);
            formData.append('file', document.getElementById('packageFile').files[0]);
            
            fetch('/api/upload', {
                method: 'POST',
                body: formData
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Package uploaded successfully!');
                    form.reset();
                }
    }
}
