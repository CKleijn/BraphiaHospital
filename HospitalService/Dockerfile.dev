# Use a slim version of Node.js
FROM node:slim

# Set environment variables
ENV NODE_ENV development

# Set the working directory inside the container
WORKDIR /express-docker

# Copy package.json and package-lock.json to the working directory
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code to the working directory
COPY . .

# Expose the application port
EXPOSE 3001

# Command to run the application
CMD [ "node", "app.js" ]
