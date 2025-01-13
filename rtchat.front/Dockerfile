# This is the newer version

FROM node:18-alpine

WORKDIR /app

COPY package.json .

RUN npm install

RUN npm i -g serve

COPY . .

RUN npm run build

EXPOSE 5174

CMD ["serve", "-s", "dist", "-l", "tcp://0.0.0.0:5174"]