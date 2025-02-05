FROM node:12 AS build

WORKDIR /app

COPY . .

COPY package* ./

RUN rm -rf dist*

RUN rm -rf node-mod*

RUN npm clean cache --force

RUN npm install --force 

RUN npm install -g @angular/cli@11 

RUN npm run build 
 
FROM nginx:latest

COPY --from=build /app/dist /usr/share/nginx/html

RUN rm -rf /usr/share/nginx/html/index.html

RUN mv /usr/share/nginx/html/NonTrading/index.html /usr/share/nginx/html

EXPOSE 80
 
