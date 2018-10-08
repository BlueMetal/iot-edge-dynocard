FROM node:9.3-alpine AS build

## install OS level packages
RUN apk update && apk add jq nginx

## deploy zipfile with application code
COPY dynocardweb/ /dynocardweb/
WORKDIR /dynocardweb 
RUN npm install

## build code
RUN node_modules/@angular/cli/bin/ng build  --prod

## define image for final container
FROM nginx:1.15.1-alpine AS final

ENV DATA_API_URL "http://localhost:8201"

## deploy nginx config
COPY nginx_defaults.conf /etc/nginx/conf.d/default.conf

## deploy built code into nginx web root
COPY --from=build /dynocardweb/dist/iot-edge-dynocard-web /usr/share/nginx/html
COPY config.json  /usr/share/nginx/html
RUN chmod 644 /usr/share/nginx/html/config.json

COPY init.sh ./

## set permissions
RUN chmod u+x init.sh

## set entrypoint/command
ENTRYPOINT [ "/bin/ash", "init.sh" ]