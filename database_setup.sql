-- MySQL Database Setup Script for SM LLM Reviews System
-- Run this script in your MySQL client to set up the database

-- Create the database
CREATE DATABASE IF NOT EXISTS sm_llm_reviews;

-- Use the database
USE sm_llm_reviews;

-- Create a user for the application (replace with your desired username/password)
CREATE USER IF NOT EXISTS 'sm_llm_user'@'localhost' IDENTIFIED BY 'your_secure_password_here';

-- Grant privileges to the user
GRANT ALL PRIVILEGES ON sm_llm_reviews.* TO 'sm_llm_user'@'localhost';

-- Flush privileges to apply changes
FLUSH PRIVILEGES;

-- Show the created database
SHOW DATABASES;

-- Show the user
SELECT User, Host FROM mysql.user WHERE User = 'sm_llm_user';

-- Note: The application will automatically create the tables when it first runs
-- using Entity Framework's EnsureCreatedAsync() method.
