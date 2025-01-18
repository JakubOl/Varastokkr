# Varastokkr

**Varastokkr** is a microservices-based Inventory Management System designed to handle product catalogs, inventory tracking, order processing, and real-time notifications at scale. Built with a modern stack (.NET 9 Minimal APIs, Angular, Docker, Kubernetes, RabbitMQ, and more), Varastokkr demonstrates a distributed architecture suitable for cloud deployments and high availability.

---

## Table of Contents

1. [Overview](#overview)  
2. [Architecture](#architecture)  
3. [Features](#features)  
4. [Technology Stack](#technology-stack)  
5. [Microservices](#microservices)  
6. [Roadmap](#roadmap)  
7. [License](#license)  

---

## Overview

Varastokkr focuses on:
- **Scalability**: Each service can be deployed and scaled independently.  
- **Flexibility**: Polyglot persistence (SQL, NoSQL) and an event-driven approach enable diverse data handling and high throughput.  
- **Real-Time Updates**: SignalR/WebSockets push live updates to connected clients.  
- **Extensibility**: Adding new features or integrating external services (e.g., payment gateways, shipping providers) is straightforward.

---

## Architecture

Below is a simplified diagram of the Varastokkr microservices architecture:

![image](https://github.com/user-attachments/assets/c05eaede-be51-41ad-a983-27acc8a0c872)

- **API Gateway**: Acts as a single entry point for the Client Apps (Web and Mobile).  
- **Services**: Individual microservices (Identity, Product, Inventory, Order, Scheduler, Notification) each owning their own database and business logic.  
- **Message Broker**: Facilitates asynchronous, event-driven communication between services.  

### Highlights
- **Client Apps**: Web and Mobile interfaces consuming REST/GraphQL endpoints via the Gateway.  
- **Database Per Service**: Ensures loose coupling; each service has autonomy over its data model.  
- **Scheduler**: Manages background jobs and recurring tasks (e.g., reordering, cleanup) using a library like Hangfire.  
- **Notification**: Utilizes SignalR for pushing real-time updates (e.g., stock changes, order status) to connected clients.  

---

## Features

- **Product Management**: Create, read, update, and delete products with optional metadata.  
- **Inventory Tracking**: Maintain accurate stock levels, manage reorder thresholds, and handle multi-warehouse support.  
- **Order Processing**: Place, update, and cancel orders with automated event-driven status changes.  
- **User & Identity Management**: Control user logins, roles, and permissions (optionally integrated via OAuth/JWT).  
- **Real-Time Updates**: Leverage SignalR to push notifications to clients (e.g., new orders, stock updates).  
- **Background Jobs**: Scheduled tasks (via Hangfire or similar) for reorder triggers, data cleanup, and batch processing.  
- **Polyglot Persistence**: Combine relational (SQL) and NoSQL (MongoDB) data stores to optimize for each microservice’s needs.  
- **Scalable Deployment**: Docker containers, orchestrated by Kubernetes, allow horizontal scaling and resilience.

---

## Technology Stack

- **Backend**:  
  - [.NET 9 Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)  
  - [RabbitMQ](https://www.rabbitmq.com/) (or another broker like Kafka)  
  - [Hangfire](https://www.hangfire.io/) or Quartz.NET for scheduling tasks  
  - [SignalR](https://learn.microsoft.com/aspnet/core/signalr) for real-time communication

- **Databases**:  
  - [SQL Server](https://www.microsoft.com/en-us/sql-server) for transactional data  
  - [MongoDB](https://www.mongodb.com/) for unstructured/NoSQL needs

- **Frontend**:  
  - [Angular](https://angular.io/)

- **Containerization & Orchestration**:  
  - [Docker](https://www.docker.com/)  
  - [Kubernetes](https://kubernetes.io/)

- **API Gateway**:  
  - Could be [Ocelot](https://github.com/ThreeMammals/Ocelot), [YARP](https://github.com/microsoft/reverse-proxy), or NGINX Ingress on K8s

---

## Microservices

1. **Identity Service**  
   - Handles user sign-up/login, JWT token issuance, and role-based authorization.

2. **Product Service**  
   - Manages product catalog, publishes product-related events (create, update, delete).

3. **Inventory Service**  
   - Tracks stock levels, listens to product changes, publishes stock-level events, triggers reorder points.

4. **Order Service**  
   - Handles the order lifecycle (create, update, cancel). Collaborates with Inventory Service for stock reservations.

5. **Scheduler Service**  
   - Hosts background jobs (e.g., daily stock checks, data cleanup). Can integrate Hangfire or Quartz.NET.

6. **Notification Service**  
   - Provides a SignalR hub or WebSocket endpoint to deliver real-time updates to client applications.

---

## Roadmap

- **Multi-Warehouse Support**: Implement location-based stock tracking and inter-warehouse transfers.
- **Vendor & Purchase Orders**: Automate reordering from suppliers with purchase order management.
- **Analytics & Reporting**: Build dashboards to show KPIs (e.g., inventory turnover, sales trends).
- **Barcode/QR Integration**: Streamline stock operations with barcode scanning.
- **Extended E-Commerce Integration**: Connect with Shopify, WooCommerce, etc., for seamless order ingestion.

---

## License

MIT License © 2023 Your Name or Company.
Feel free to use, modify, and distribute this code; just include the original license.
