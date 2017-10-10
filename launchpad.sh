#!/bin/bash

pushd "${0%/*}" > /dev/null

EXEC=./bin/Launchpad.dll

if ! [ -f "$EXEC" ]
then
  dotnet publish -c Release -o ../../bin/ src/Launchpad/Launchpad.csproj
fi

dotnet $EXEC $*

popd > /dev/null
