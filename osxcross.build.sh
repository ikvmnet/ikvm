#!/bin/sh
curl https://github.com/joseluisq/macosx-sdks/releases/download/11.1/MacOSX11.1.sdk.tar.xz -o osxcross/tarballs/MacOSX11.1.sdk.tar.xz
cd osxcross
./build.sh
