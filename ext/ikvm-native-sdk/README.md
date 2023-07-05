## ikvm-native-sdk

Building certain portions of the IKVM project requires building native C code that targets Windows, Linux and Mac OS X. The IKVM project uses Clang, driven by ClangProj project types to build these libraries. These projects need access to the development headers and libraries for the various foreign platforms.

Building on Windows does not require the Windows packages. It does require you to have the Windows SDK installed locally, however. On non-Windows platforms, the Windows SDK files are required. Every platform requires the Linux and Mac OS X packages.

These packages are available from the https://github.com/ikvmnet/ikvm-native-sdk project. Download all of the .tar.gz files, and extract to this directory.
