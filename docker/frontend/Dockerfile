# ==========================================
# Build Angular application
# ==========================================
FROM node:22-alpine AS build

WORKDIR /app

# Copy package files first for Docker caching
COPY frontend/purchase-order-frontend/package*.json ./

RUN npm ci

# Copy Angular source
COPY frontend/purchase-order-frontend/ .

# Build Angular application
RUN npm run build


# ==========================================
# Production web server
# ==========================================
FROM nginx:alpine

# Remove default Nginx content
RUN rm -rf /usr/share/nginx/html/*

# Angular 19 application output
COPY --from=build /app/dist/purchase-order-frontend/browser/ /usr/share/nginx/html/

# Custom Nginx configuration
COPY docker/frontend/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]