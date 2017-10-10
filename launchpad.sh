#!/bin/bash

EXEC="${0%/*}/bin/Launchpad.dll"
PROJECT="${0%/*}/src/Launchpad/Launchpad.csproj"

if ! [ -f "$EXEC" ]
then
  dotnet publish -c Release -o ../../bin/ $PROJECT
fi

dotnet $EXEC $*