# ---- build the static site ----
FROM node:26-slim AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci --no-audit --no-fund
COPY . .
# VITE_* values are baked into the bundle at build time
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL
RUN npm run build

# ---- serve with nginx (~5 MB RAM) ----
FROM nginx:1.29-alpine
RUN printf 'server {\n  listen 80;\n  root /usr/share/nginx/html;\n  gzip on;\n  gzip_types text/css application/javascript application/json image/svg+xml;\n  location /assets/ { expires 1y; add_header Cache-Control "public, immutable"; }\n  location / { try_files $uri $uri/ /index.html; }\n}\n' > /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
