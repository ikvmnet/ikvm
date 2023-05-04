#!/bin/bash

xwin_version="0.2.12"

path="$(dirname "$0")/build/xwin"
mkdir -p $path $path/.bin/xwin

case "$(uname -s)" in
    Linux*)     xwin_prefix=xwin-$xwin_version-x86_64-unknown-linux-musl;;
    Darwin*)    xwin_prefix=xwin-$xwin_version-aarch64-apple-darwin;;
    *)          exit 1;
esac

curl --fail -L https://github.com/Jake-Shadle/xwin/releases/download/$xwin_version/$xwin_prefix.tar.gz | tar -xzv -C $path/.bin --strip-components=1 $xwin_prefix/xwin
$path/.bin/xwin --accept-license --cache-dir $path/.cache --arch x86,x86_64,aarch,aarch64 splat --output $path
