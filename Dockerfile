FROM microsoft/dotnet:2-runtime-jessie

ADD . /app

WORKDIR /app

ENTRYPOINT ["./launchpad.sh"]

CMD ["-n"]