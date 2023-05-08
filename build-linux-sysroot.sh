#!/bin/bash -ex

export cwd=$(dirname $(readlink -f $0))
export ext=$cwd/ext

cd $ext/crosstool-ng
./bootstrap
