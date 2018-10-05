FROM node:9.3-alpine AS build

## install OS level packages
RUN apk update && apk add jq nginx

## deploy zipfile with application code
COPY dynocardweb/ /dynocardweb/
WORKDIR /dynocardweb 
RUN npm install

## deploy edge environment config and add it to angular-cli
# COPY environment.edge.ts ./src/environments/
# RUN  jq '.apps[].environments.edge = "environments/environment.edge.ts"' .angular-cli.json > .angular-cli.json.new &&\
#      mv .angular-cli.json     .angular-cli.json.old &&\
#      mv .angular-cli.json.new .angular-cli.json

## build code
RUN node_modules/@angular/cli/bin/ng build  --prod

## define image for final container
FROM nginx:1.15.1-alpine AS final

## deploy nginx config
COPY nginx_defaults.conf /etc/nginx/conf.d/default.conf

## deploy built code into nginx web root
COPY --from=build /dynocardweb/dist/iot-edge-dynocard-web /usr/share/nginx/html

## run the code with ng development server until we configure Nginx
##CMD ./node_modules/.bin/ng serve --target production --environment edge --host 0.0.0.0

