#!/bin/bash -ex

sdk="$(dirname "$0")/sdk"
mkdir -p $sdk

url="https://alethicdevops.blob.core.windows.net/artifacts/MacOSX11.1.sdk.tar.xz?sv=2021-08-06&st=2023-05-11T05%3A24%3A28Z&se=2026-05-12T05%3A24%3A00Z&sr=b&sp=r&sig=jwUrA375PbpNh7oVbIIDzG7OGA8DifZxHH1Egx6LmJs%3D"
curl --fail -L $url | tar xJv -C $sdk --strip-components=1
