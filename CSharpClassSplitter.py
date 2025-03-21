import argparse
import re
import os
import sys

def parse_method(method_text):
    # Previous parse_method implementation remains the same
    signature_pattern = r'(public|private|protected|internal)?\s*(static\s+)?([\w<>[\],\s]+)\s+(\w+)\s*\((.*?)\)'
    match = re.search(signature_pattern, method_text)
    if match:
        access, is_static, return_type, method_name, params = match.groups()
        return {
            'access': access if access else 'public',
            'is_static': bool(is_static),
            'return_type': return_type.strip(),
            'name': method_name,
            'params': params.strip(),
            'body': method_text
        }
    return None

def count_lines(text):
    return len(text.split('\n'))

def create_new_class(method_info, namespace):
    # Previous create_new_class implementation remains the same
    class_name = f"{method_info['name']}Class"
    class_template = f"""
using System;

namespace {namespace}
{{
    public class {class_name}
    {{
        {method_info['body']}
    }}
}}
"""
    return class_name, class_template

def refactor_large_methods(input_file, line_threshold, output_dir=None):
    if not os.path.exists(input_file):
        print(f"Error: Input file '{input_file}' not found.")
        return False

    # Set output directory
    output_dir = output_dir or os.path.dirname(input_file)
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    try:
        with open(input_file, 'r') as f:
            content = f.read()
    except Exception as e:
        print(f"Error reading input file: {e}")
        return False

    # Extract namespace
    namespace_match = re.search(r'namespace\s+([^\s{]+)', content)
    namespace = namespace_match.group(1) if namespace_match else "DefaultNamespace"

    # Find all methods
    method_pattern = r'((?:public|private|protected|internal)?\s*(?:static\s+)?[\w<>[\],\s]+\s+\w+\s*\(.*?\)\s*{[^{}]*(?:{[^{}]*}[^{}]*)*})'
    methods = re.finditer(method_pattern, content, re.DOTALL)

    new_classes = []
    modified_content = content
    methods_refactored = 0

    for method in methods:
        method_text = method.group(0)
        if count_lines(method_text) > line_threshold:
            method_info = parse_method(method_text)
            if method_info:
                # Create new class
                class_name, class_content = create_new_class(method_info, namespace)
                new_classes.append((f"{class_name}.cs", class_content))

                # Replace original method with delegation
                params_list = re.findall(r'(?:ref |out )?(\w+)', method_info['params'])
                delegation = f"""
    {method_info['access']}{' static' if method_info['is_static'] else ''} {method_info['return_type']} {method_info['name']}({method_info['params']})
    {{
        return new {class_name}().{method_info['name']}({', '.join(params_list)});
    }}"""
                modified_content = modified_content.replace(method_text, delegation)
                methods_refactored += 1

    if methods_refactored == 0:
        print("No methods found exceeding the specified line threshold.")
        return True

    # Save modified main file
    base_name = os.path.splitext(os.path.basename(input_file))[0]
    output_main = os.path.join(output_dir, f"{base_name}_refactored.cs")

    try:
        with open(output_main, 'w') as f:
            f.write(modified_content)

        # Save new class files
        for class_file, content in new_classes:
            class_path = os.path.join(output_dir, class_file)
            with open(class_path, 'w') as f:
                f.write(content)

        print(f"\nRefactoring completed successfully:")
        print(f"Methods refactored: {methods_refactored}")
        print(f"Files created:")
        print(f"- {output_main}")
        for class_file, _ in new_classes:
            print(f"- {os.path.join(output_dir, class_file)}")

        return True

    except Exception as e:
        print(f"Error writing output files: {e}")
        return False

def main():
    parser = argparse.ArgumentParser(description='Refactor large C# methods into separate classes.')
    parser.add_argument('input_file', help='Path to the input C# file')
    parser.add_argument('-t', '--threshold', type=int, default=30,
                        help='Line count threshold for method extraction (default: 30)')
    parser.add_argument('-o', '--output-dir', help='Output directory for refactored files')

    args = parser.parse_args()

    success = refactor_large_methods(args.input_file, args.threshold, args.output_dir)
    sys.exit(0 if success else 1)

if __name__ == "__main__":
    main()