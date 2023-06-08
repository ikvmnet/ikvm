# IKVM Linux SDK

Building the native OpenJDK libraries for Linux requires that we have a number of headers and shared libraries available for compilation and linking. These SDK directories are built with specific (older) versions of GCC and GLIBC that should match the compatibility matrix of .NET Core.

crosstool-ng is used to build a sysroot for cross compilation for each platform. Each platform has a .config file within it's .dir directory.

Once finished, the build script uses the resulting cross compiler to build a second distribution of the headers and libraries for the target platform. This becomes the final SDK image.