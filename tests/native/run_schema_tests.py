"""Compile production schema dispatch/reader bodies against a fake engine boundary."""
from pathlib import Path
import os
import shlex
import subprocess
import tempfile

root = Path(__file__).resolve().parents[2]


def extract(path, signature):
    source = (root / path).read_text()
    start = source.index(signature)
    opening = source.index('{', start)
    depth = 0
    for pos in range(opening, len(source)):
        depth += (source[pos] == '{') - (source[pos] == '}')
        if depth == 0:
            return source[start:pos + 1]
    raise RuntimeError(f'Unterminated definition: {signature}')


parts = {
    'FIELD': extract('src/sdk/schema.h', 'struct SchemaField') + ';',
    'NETWORKVAR': extract('src/sdk/schema.h', 'class NetworkVar') + ';',
    'CHAINER': extract('src/sdk/schema.cpp', 'class CNetworkVarChainer') + ';',
    'READER': '\n'.join(extract('src/sdk/reader.cpp', signature) for signature in [
        'bool IsStandardLayoutClass(', 'void FindChainer(', 'void ReadClasses(']),
    'DISPATCH': extract('src/sdk/schema.cpp', 'void CSDKSchema::SetStateChanged('),
}
fixture = (root / 'tests/native/schema.cpp.in').read_text()
for name, value in parts.items():
    fixture = fixture.replace('@' + name + '@', value)
with tempfile.TemporaryDirectory(prefix='swiftlys2-schema-') as temporary:
    source = Path(temporary) / 'schema.cpp'
    binary = Path(temporary) / 'schema'
    source.write_text(fixture)
    subprocess.run(shlex.split(os.environ.get('CXX', 'g++')) + [
        '-std=c++20', '-O1', '-g', '-Wall', '-Wextra', '-Werror',
        '-fsanitize=address,undefined', '-fno-sanitize=vptr',
        '-fno-omit-frame-pointer', '-I', str(root / 'src'), str(source), '-o', str(binary)
    ], check=True)
    subprocess.run([str(binary)], check=True, timeout=15)
