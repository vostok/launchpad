#!/bin/bash

DIR="$(dirname $(readlink -f $0))"
EXEC="$DIR/bin/Launchpad.dll"
PROJECT="$DIR/src/Launchpad/Launchpad.csproj"


if ! [ -f "$EXEC" ]
then
  dotnet publish -c Release -o ../../bin/ $PROJECT
fi

dotnet $EXEC $*
