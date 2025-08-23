# Online Marketplace

**Online Marketplace** is a modular, scalable **online marketplace platform** designed with **Clean Architecture**. It enables customets to interact seamlessly, incorporating features like user registration, product listing, secure transactions, and order tracking. And providing a secure, reliable, and extensible e-commerce experience.

## Table of Contents

- [System Architecture](#system-architecture)
- [Actors](#actors)
- [Features](#features)
  - [Authentication & Authorization](#authentication--authorization)
  - [Admin Capabilities](#admin-capabilities)
  - [Seller Capabilities](#seller-capabilities)
  - [Buyer Capabilities](#buyer-capabilities)
- [Real-Time Communication](#real-time-communication)
- [Database Design](#database-design)
- [Technology Stack](#technology-stack)

## System Architecture

Online Marketplace is structured using **Clean Architecture**:  

- **Domain Layer**: Core aggregates like `Product`, `Order`, `Cart`, and `User`.  
- **Application Layer**: Use case coordination, CQRS-based handlers, and business validation.  
- **Infrastructure Layer**: EF Core persistence, identity, and file storage.  
- **API Layer**: RESTful endpoints for marketplace functionality, exposing request/response contracts.  

Key design patterns and practices:  

- **CQRS**: Separation of queries and commands for scalability.  
- **Result Pattern**: Unified handling of success/failure without relying on exceptions.

## Actors

- **Admin**: Manages platform-wide activities, moderation, and oversight.
- **Customer**: Browses, purchases, and reviews products.

## Features

### Authentication & Authorization

- Secure login/sign-up with email or external providers.  
- Role-based access control (Admin, Customer).  
- Anonymous browsing of products.  

### Admin Capabilities

- Monitor transactions, disputes, and flagged content.  
- Manage categories and platform-wide settings.    

### Buyer Capabilities

- Browse and filter products by **category**<!--, **price**, **rating**, and more-->.
- Add products to **Cart**.
- Place secure orders and track deliveries.
- Leave reviews and ratings for products. 

### Global Exception Handling

The API centralizes exception handling for consistent error responses:

- Custom responses for **Validation**, **NotFound**, **Unauthorized** exceptions.
- Generic error fallback for unexpected exceptions.  

## Database Design

Entities are modeled as aggregates and follow normalized design:

- **Users**: Admins, Customers.
- **Products**: Core marketplace items.
- **Categories**: Product grouping.
- **Orders**: Aggregate linking custoemrs and products.
- **OrderItems**: Items belonging to orders.
- **Cart**: Customer's current selected items.
- **Payments**: Secure transaction records.
- **Reviews**: Ratings and feedback for products.

## Technology Stack

- **Backend**: ASP.NET Core MVC
- **Frontend**: HTML, CSS, Bootstrap, JavaScript, jQuery & Ajax
- **Database**: EF Core (Code-First) with SQL Server
- **Authentication**: ASP.NET Identity + Cookies
- **Architecture**: Clean Architecture
- **Validation**: FluentValidation
