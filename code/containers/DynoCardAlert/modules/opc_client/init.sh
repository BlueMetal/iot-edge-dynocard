#!/bin/ash

## set default values
[ -z "$OPC_ENDPOINT_URL" ] && export OPC_ENDPOINT_URL="opc.tcp://104.209.190.252:49380"

## set the OPC endpoint url
jq --arg EndpointUrl "$OPC_ENDPOINT_URL" '.[].EndpointUrl=$EndpointUrl' publishednodes.json | sponge publishednodes.json

## start the app
echo "OPC Publishing service starting..."
echo "OPC_ENDPOINT_URL=$OPC_ENDPOINT_URL"
exec dotnet /app/opcpublisher.dll publisher  --di=120 --to --aa --si=1 --oi=150 --op=150