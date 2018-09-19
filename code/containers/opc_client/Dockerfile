FROM microsoft/iot-edge-opc-publisher

## set default OPC endpoint URL
ENV OPC_ENDPOINT_URL "opc.tcp://192.168.10.37:49380"

## install OS-level dependencies
RUN apk add jq

## install moreutils from testing repo
RUN apk add moreutils --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted

## deploy application
COPY  init.sh publishednodes.json ./

## set permissions
RUN chmod u+x init.sh

## set entrypoint/command
ENTRYPOINT [ "/bin/ash", "init.sh" ]