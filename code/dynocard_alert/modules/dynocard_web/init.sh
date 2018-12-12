#!/bin/sh

## set default values
[ -z "$DATA_API_URL" ] && export DATA_API_URL="localhost:8201"

pwd
ls
cat /usr/share/nginx/html/config.json

## set the http data api endpoint url
sed -i "s/localhost:8201/$DATA_API_URL/" /usr/share/nginx/html/config.json

## start the app
echo "Dynocard visualization web app starting..."
echo "DATA_API_URL=$DATA_API_URL"

echo "Starting nginx..."
nginx -g 'daemon off;' 