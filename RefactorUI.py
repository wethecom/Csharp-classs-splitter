import tkinter as tk
from tkinter import ttk, filedialog, messagebox
import subprocess
import os

class RefactorGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("C# Class Splitter")
        self.root.geometry("600x400")

        # Create main frame with padding
        main_frame = ttk.Frame(root, padding="20")
        main_frame.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))

        # File selection
        self.file_path = tk.StringVar()
        ttk.Label(main_frame, text="Select C# File:").grid(row=0, column=0, sticky=tk.W, pady=5)
        ttk.Entry(main_frame, textvariable=self.file_path, width=50).grid(row=1, column=0, padx=5)
        ttk.Button(main_frame, text="Browse", command=self.browse_file).grid(row=1, column=1)

        # Line threshold
        ttk.Label(main_frame, text="Line Threshold:").grid(row=2, column=0, sticky=tk.W, pady=(20, 5))
        self.threshold = tk.IntVar(value=30)
        threshold_scale = ttk.Scale(main_frame, from_=10, to=100, variable=self.threshold,
                                    orient=tk.HORIZONTAL, length=200)
        threshold_scale.grid(row=3, column=0, sticky=tk.W)
        self.threshold_label = ttk.Label(main_frame, text="30 lines")
        self.threshold_label.grid(row=3, column=1)
        threshold_scale.configure(command=self.update_threshold_label)

        # Output directory
        self.output_dir = tk.StringVar()
        ttk.Label(main_frame, text="Output Directory:").grid(row=4, column=0, sticky=tk.W, pady=(20, 5))
        ttk.Entry(main_frame, textvariable=self.output_dir, width=50).grid(row=5, column=0, padx=5)
        ttk.Button(main_frame, text="Browse", command=self.browse_output_dir).grid(row=5, column=1)

        # Refactor button
        ttk.Button(main_frame, text="Refactor Code", command=self.refactor_code).grid(row=6, column=0,
                                                                                     columnspan=2, pady=30)

        # Status message
        self.status_var = tk.StringVar()
        self.status_label = ttk.Label(main_frame, textvariable=self.status_var, wraplength=500)
        self.status_label.grid(row=7, column=0, columnspan=2, pady=10)

    def update_threshold_label(self, value):
        self.threshold_label.configure(text=f"{int(float(value))} lines")

    def browse_file(self):
        filename = filedialog.askopenfilename(filetypes=[("C# files", "*.cs")])
        if filename:
            self.file_path.set(filename)

    def browse_output_dir(self):
        directory = filedialog.askdirectory()
        if directory:
            self.output_dir.set(directory)

    def refactor_code(self):
        if not self.file_path.get():
            messagebox.showerror("Error", "Please select a C# file")
            return

        if not self.output_dir.get():
            messagebox.showerror("Error", "Please select an output directory")
            return

        try:
            # Run the CSharpClassSplitter.py script
            command = [
                "python", "CSharpClassSplitter.py",
                self.file_path.get(),
                "--threshold", str(self.threshold.get()),
                "--output-dir", self.output_dir.get()
            ]
            result = subprocess.run(command, capture_output=True, text=True)

            if result.returncode == 0:
                self.status_var.set("Refactoring completed successfully!")
                messagebox.showinfo("Success", "Code refactoring completed successfully!")
            else:
                self.status_var.set(f"Error: {result.stderr}")
                messagebox.showerror("Error", f"An error occurred:\n{result.stderr}")

        except Exception as e:
            self.status_var.set(f"Error: {str(e)}")
            messagebox.showerror("Error", f"An error occurred: {str(e)}")


def main():
    root = tk.Tk()
    app = RefactorGUI(root)
    root.mainloop()


if __name__ == "__main__":
    main()