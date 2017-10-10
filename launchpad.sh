#!/bin/bash

EXEC=./bin/Launchpad.dll

if ! [ -f "$EXEC" ]
then
  dotnet publish -c Release -o ../../bin/ src/Launchpad/Launchpad.csproj
fi

dotnet $EXEC $*
