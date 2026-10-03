# Ink 1.2.1

Official inkle Ink runtime and compiler, vendored from tag `v1.2.1`, commit
`35c63e52f1d36060930dc7ed3cfba38ea224b528` at https://github.com/inkle/ink.
See LICENSE.txt (MIT). Source is unmodified.

Runtime is included in player builds. Compiler is under Editor and excluded from builds.
Project adapter: `Assets/_Game/Scripts/Dialogue/Editor/InkStoryCompiler.cs`.
Entry: `Assets/_Game/Data/Dialogue/Ink/Main.ink`; output: `Main.json` alongside it.
The adapter recompiles when the source Ink assets are imported; it can also be invoked
from **ThuyKieu > Dialogue > Compile Main Ink**. No custom Ink parser is used.

The upstream compiler's optional plugin loader can emit Unity warning UAC0020.
This project supplies no plugin directories and does not invoke that loader.
