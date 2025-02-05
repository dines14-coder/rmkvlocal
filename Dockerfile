FROM node:12 AS build

WORKDIR /app

COPY . .
 
FROM nginx:latest

COPY --from=build /app/dist /usr/share/nginx/html

RUN rm -rf /usr/share/nginx/html/index.html

RUN mv /usr/share/nginx/html/NonTrading/index.html /usr/share/nginx/html

EXPOSE 80
 
