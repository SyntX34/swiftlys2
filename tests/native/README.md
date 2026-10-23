Run from the repository root with Python 3 and a C++20 compiler:

```sh
python3 tests/native/run_schema_tests.py
```

The runner compiles the production schema reader and notification method bodies
against a fake engine boundary, with AddressSanitizer and UndefinedBehaviorSanitizer.
It verifies embedded wrapper `this` pointers and local offsets, notifications from
nested schema handles, the original struct/entity classification, direct and
inherited chainers, null owners, unsupported one-byte structs, and unknown fields. It also checks that
schema enumeration order does not affect runtime wrapper dispatch.

Set `CXX` to override the compiler. In a sandbox that prevents LeakSanitizer from
inspecting processes, run with `ASAN_OPTIONS=detect_leaks=0`.
The fixture uses explicit engine-style vtables, so C++ vptr instrumentation is
disabled; bounds, alignment and other undefined-behavior checks remain enabled.
These tests do not replace a full native build or an in-game replication test.
