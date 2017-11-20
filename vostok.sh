#!/bin/bash

if [[ "$OSTYPE" == "darwin"* ]]; then
  DIR="$(dirname $(readlink $0))"
else
  DIR="$(dirname $(readlink -f $0))"
fi

EXEC="$DIR/bin/Launchpad.dll"
PROJECT="$DIR/src/Launchpad/Launchpad.csproj"

if ! [ -f "$EXEC" ]
then
  dotnet publish -c Release -o ../../bin/ $PROJECT
fi

dotnet $EXEC $*
