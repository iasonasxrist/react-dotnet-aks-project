# Stage 1: Build the Vite application (This stage remains the same)
FROM node:22-alpine AS builder
LABEL stage=builder

# Set the working directory
WORKDIR /app

# Copy package.json and lock file for dependency installation
COPY package*.json ./

# Install dependencies using 'ci' for reproducible builds
RUN npm ci

# Copy the rest of the application source code
COPY . .

# Build the application for production
# For Vite, this outputs static files to the /app/dist directory
RUN npm run build


# Stage 2: Create a small production image to serve the files
# We use a clean Node image, not Nginx
FROM node:22-alpine AS final

WORKDIR /app

# Install the 'serve' package globally. This is our lightweight web server.
RUN npm install -g serve

# Copy the built static files from the 'builder' stage
# The source is '/app/dist' from the builder, the destination is the current directory '.'
COPY --from=builder /app/dist .

# Expose the port you want your application to run on
EXPOSE 5173

# The command to start the server
#   'serve':  The command to run
#   '-s':     Serve the content as a Single-Page Application (handles routing)
#   '-l 5173': Listen on port 5173
#   '.':      Serve the files from the current directory
CMD ["serve", "-s", "-l", "5173", "."]
