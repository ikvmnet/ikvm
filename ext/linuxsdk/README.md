# IKVM Linux SDK

Building the native OpenJDK libraries for Linux requires that we have a number of headers and shared libraries available for compilation and linking. The 'linuxsdk' component builds these.

crosstool-ng is used to build a sysroot for cross compilation for each platform. Each platform has a .config file within it's .dir directory.

Once finished, the build script uses the resulting cross compiler to build a number of libraries for the target platform.

These directories need to be present for the MSBuild components to use for headers.
