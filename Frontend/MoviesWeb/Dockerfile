# Stage 1: Build the React application
FROM node:20-alpine AS builder
LABEL stage=builder

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json (or yarn.lock/pnpm-lock.yaml)
COPY package*.json ./

# Install dependencies using ci for cleaner installs (uses lock file)
RUN npm ci

# Copy the rest of the application source code
COPY . .

# Build the application for production
# This will output files to the /app/dist directory
RUN npm run build

# Stage 2: Serve the static files using Nginx
FROM nginx:1.25-alpine AS runner

# Copy the built static files from the builder stage to the Nginx HTML directory
COPY --from=builder /app/dist /usr/share/nginx/html

# Copy a custom Nginx configuration (see Step 3)
# This handles SPA routing (serving index.html for non-file routes)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80 (the default Nginx port)
EXPOSE 80

# Command to run Nginx in the foreground
CMD ["nginx", "-g", "daemon off;"]